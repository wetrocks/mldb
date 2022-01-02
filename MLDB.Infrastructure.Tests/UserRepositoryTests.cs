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
    [TestOf(typeof(UserRepository))]
    public class UserRepositoryTests
    {
        private SqliteConnection conn;
        private DbContextOptions<SiteSurveyContext> ctxOptions;
        private SiteSurveyContext testCtx;

        private IUserRepository testRepo;

        private Fixture fixture;

        private User seedUser;

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

            testRepo = new UserRepository(testCtx);

        }

        [TearDown]
        public void TearDown()
        {
            testCtx.Dispose();
            conn.Close();
        }

        private void seedTestData() {
            using( var seedCtx = new SiteSurveyContext(ctxOptions)) {
                seedUser = fixture.Build<User>()
                                  .Without( u => u.Id )
                                  .Create();
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

            testUser.Should().BeEquivalentTo(seedUser);
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

            testUser.Should().BeEquivalentTo(seedUser);
        }

        [Test]
        public async Task insert_AddsNewUser()
        {
            var testUser = fixture.Build<User>()
                                  .Without( u => u.Id )
                                  .Create();

            var created = await testRepo.insertAsync(testUser);
            testCtx.SaveChanges();

            created.Id.Should().NotBe(0);

            using(var assertCtx = new SiteSurveyContext(ctxOptions)) {
                var inserted = assertCtx.Users.Find(created.Id);
                inserted.Should().BeEquivalentTo(created);
            }
        }

        [Test]
        public void insert_WhenIdpIdExists_ThrowsException()
        {
            var testUser = fixture.Build<User>()
                                  .Without( u => u.Id )
                                  .With( u => u.IdpId,  seedUser.IdpId )
                                  .Create();

            Assert.ThrowsAsync<DbUpdateException>( async () => { 
                var created = await testRepo.insertAsync(testUser);
                testCtx.SaveChanges();
            });
        }
    }
}