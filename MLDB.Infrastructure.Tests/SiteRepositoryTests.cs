using System;
using MLDB.Infrastructure.Repositories;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FluentAssertions;
using MLDB.Domain;
using AutoFixture;
using Npgsql;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Configurations;

namespace MLDB.Infrastructure.IntegrationTests
{
    [TestOf(typeof(SiteRepository))]
    public class SiteRepositoryTests
    {
        private DbContextOptions<SiteSurveyContext> ctxOptions;
        private SiteSurveyContext testCtx;

        private ISiteRepository testRepo;

        private Fixture fixture;

        private Site seedSite;

        [SetUp]
        public async Task Setup()
        {
            fixture = new Fixture();

            TestcontainerDatabase dbContainer =  new TestcontainersBuilder<PostgreSqlTestcontainer>()
                .WithDatabase(new PostgreSqlTestcontainerConfiguration {
                    Database = "db",
                    Username = "postgres",
                    Password = "password",
                 })
                .Build();

            await dbContainer.StartAsync();

            ctxOptions = new DbContextOptionsBuilder<SiteSurveyContext>()
                        //  .LogTo( Console.WriteLine)
                        // .EnableSensitiveDataLogging()
                        .UseNpgsql(new NpgsqlConnection(dbContainer.ConnectionString))
                        .Options;
            testCtx = new SiteSurveyContext(ctxOptions);
            testCtx.Database.EnsureCreated();

            seedTestData();

            testRepo = new SiteRepository(testCtx);
        }

        [TearDown]
        public void TearDown()
        {
            testCtx.Dispose();
        }

        private void seedTestData() {
            using( var seedCtx = new SiteSurveyContext(ctxOptions)) {
                seedSite = fixture.Build<Site>()
                                    .With( x => x.CreateTimestamp, DateTime.UtcNow)
                                    .Create();
                seedCtx.Add(seedSite);

                seedCtx.SaveChanges();
            };
        }

        [Test]
        public async Task getAll_ReturnsAll()
        {
            var allSites = await testRepo.getAll();

            allSites.Should().ContainEquivalentOf(seedSite, opts => opts
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(0,0,0,1)))
                .When(info => info.Path.EndsWith("Timestamp")));
                // TODO: ^^^^^^^^^^ that better everywhere
        }

        [Test]
        public async Task find_WhenNotExists_ReturnsNull()
        {
            var testSite = await testRepo.findAsync(fixture.Create<Guid>());

            testSite.Should().BeNull();
        }

        [Test]
        public async Task find_WhenExists_ReturnsSite()
        {
            var testSite = await testRepo.findAsync(seedSite.Id);

            testSite.Should().BeEquivalentTo(seedSite, opts => opts
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(0,0,0,1)))
                .When(info => info.Path.EndsWith("Timestamp")));
        }

        [Test]
        public async Task insert_AddsNewSite()
        {
            var testSite = fixture.Build<Site>()
                                  .With( x => x.CreateTimestamp, DateTime.UtcNow)
                                  .Create();

            var created = await testRepo.insertAsync(testSite);
            testCtx.SaveChanges();

            created.Id.Should().NotBeEmpty();

            using(var assertCtx = new SiteSurveyContext(ctxOptions)) {
                var inserted = assertCtx.Sites.Find(created.Id);
                inserted.Should().BeEquivalentTo(created, opts => opts
                    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(0,0,0,1)))
                    .When(info => info.Path.EndsWith("Timestamp")));
            }
        }

        [Test]
        public async Task update_withChangeToFields_UpdatesSite()
        {
            var testSite = new Site(seedSite.Id, seedSite.Name, seedSite.CreateUserId);
            
            testSite.Name = fixture.Create<string>("newName");

            var updated = await testRepo.updateAsync(testSite);
            testCtx.SaveChanges();
            
            using( var assertCtx = new SiteSurveyContext(ctxOptions) ) {
                var testUpdated = assertCtx.Sites.Find(seedSite.Id);
                testUpdated.Should().BeEquivalentTo(testSite, opt => opt
                    .Excluding( x => x.CreateUserId )
                    .Excluding( x => x.CreateTimestamp )
                );
           }
        }
    }
}