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
    [TestOf(typeof(UserRepository))]
    public class UserRepositoryTests
    {
        private DbContextOptions<SiteSurveyContext> ctxOptions;
        private SiteSurveyContext testCtx;

        private IUserRepository testRepo;

        private Fixture fixture;

        private User seedUser;

        [SetUp]
        public async Task Setup()
        {
            fixture = new Fixture();

            TestcontainerDatabase dbContainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
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

            testRepo = new UserRepository(testCtx);
        }

        [TearDown]
        public void TearDown()
        {
            testCtx.Dispose();
        }

        private User newUser() {
            return fixture.Build<User>()
                                  .Without( u => u.Id )
                                  .With( u => u.CreateTimestamp, DateTime.UtcNow)
                                  .With( u => u.UpdateTimestamp, DateTime.UtcNow)
                                  .Create();
        }

        private void seedTestData() {
            using( var seedCtx = new SiteSurveyContext(ctxOptions)) {
                seedUser = newUser();

                seedUser = seedCtx.Users.Add(seedUser).Entity;

                seedCtx.SaveChanges();
            };
        }

        [Test]
        public async Task find_WhenNotExists_ReturnsNull()
        {
            var testUser = await testRepo.findAsync(fixture.Create<UInt16>());

            testUser.Should().BeNull();
        }

        [Test]
        public async Task find_WhenExists_ReturnsUser()
        {
            var testUser = await testRepo.findAsync(seedUser.Id);

            testUser.Should().BeEquivalentTo(seedUser, opts => opts
                    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(0,0,0,1)))
                    .When(info => info.Path.EndsWith("Timestamp")));
        }

        [Test]
        public async Task findByIdpId_WhenNotExists_ReturnsNull()
        {
            var testUser = await testRepo.findByIdpIdAsync(fixture.Create<string>());

            testUser.Should().BeNull();
        }

        [Test]
        public async Task findByIdpId_WhenExists_ReturnsUser()
        {
            var testUser = await testRepo.findByIdpIdAsync(seedUser.IdpId);

            testUser.Should().BeEquivalentTo(seedUser, opts => opts
                    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(0,0,0,1)))
                    .When(info => info.Path.EndsWith("Timestamp")));
        }

        [Test]
        public async Task insert_AddsNewUser()
        {
            var testUser = newUser();

            var created = await testRepo.insertAsync(testUser);
            testCtx.SaveChanges();

            created.Id.Should().NotBe(0);

            using(var assertCtx = new SiteSurveyContext(ctxOptions)) {
                var inserted = assertCtx.Users.Find(created.Id);
                inserted.Should().BeEquivalentTo(created, opts => opts
                    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(0,0,0,1)))
                    .When(info => info.Path.EndsWith("Timestamp")));
            }
        }

        [Test]
        public void insert_WhenIdpIdExists_ThrowsException()
        {
            var testUser = fixture.Build<User>()
                                  .Without( u => u.Id )
                                  .With( u => u.IdpId,  seedUser.IdpId )
                                  .With( u => u.CreateTimestamp, DateTime.UtcNow)
                                  .With( u => u.UpdateTimestamp, DateTime.UtcNow)
                                  .Create();

            Assert.ThrowsAsync<DbUpdateException>( async () => { 
                var created = await testRepo.insertAsync(testUser);
                testCtx.SaveChanges();
            });
        }
    }
}