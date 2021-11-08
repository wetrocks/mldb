using System;
using MLDB.Infrastructure.Repositories;
using NUnit.Framework;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FluentAssertions;
using MLDB.Domain;
using AutoFixture;

namespace MLDB.Infrastructure.IntegrationTests
{
    [TestOf(typeof(SiteRepository))]
    public class SiteRepositoryTests
    {
        private SqliteConnection conn;
        private DbContextOptions<SiteSurveyContext> ctxOptions;
        private SiteSurveyContext testCtx;

        private ISiteRepository testRepo;

        private Fixture fixture;

        private Site seedSite;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();

            conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();

            ctxOptions = new DbContextOptionsBuilder<SiteSurveyContext>()
                        //  .LogTo( Console.WriteLine)
                        // .EnableSensitiveDataLogging()
                        .UseSqlite(conn)
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
            conn.Close();
        }

        private void seedTestData() {
            using( var seedCtx = new SiteSurveyContext(ctxOptions)) {
                seedSite = fixture.Build<Site>()
                                    .Create();
                seedCtx.Add(seedSite);

                seedCtx.SaveChanges();
            };
        }

        [Test]
        public async Task getAll_ReturnsAll()
        {
            var allSites = await testRepo.getAll();

            allSites.Should().ContainEquivalentOf(seedSite);
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

            testSite.Should().BeEquivalentTo(seedSite);
        }

        [Test]
        public async Task insert_AddsNewSite()
        {
            var testSite = fixture.Build<Site>().Create();

            var created = await testRepo.insertAsync(testSite);
            testCtx.SaveChanges();

            created.Id.Should().NotBeEmpty();

            using(var assertCtx = new SiteSurveyContext(ctxOptions)) {
                var inserted = assertCtx.Sites.Find(created.Id);
                inserted.Should().BeEquivalentTo(created);
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