using System;
using MLDB.Infrastructure.Repositories;
using NUnit.Framework;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FluentAssertions;
using MLDB.Domain;
using AutoFixture;
using System.Linq;



namespace MLDB.Infrastructure.IntegrationTests
{
    [TestOf(typeof(SurveyRepository))]
    public class SurveyRepositoryTests
    {
        private SqliteConnection conn;
        private DbContextOptions<SiteSurveyContext> ctxOptions;
        private SiteSurveyContext testCtx;

        private ISurveyRepository testRepo;

        private Fixture fixture;

   //     private User seedUser;

        private Site seedSite;

        private Survey seedSurvey;

        [SetUp]
        public void Setup()
        {
            fixture = new AutoFixture.Fixture();

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

            testRepo = new SurveyRepository(testCtx);
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

                seedSurvey = new Survey(seedSite.Id, 
                                        fixture.CreateMany<int>().ToList(),
                                        fixture.Create<string>());
                seedCtx.Add(seedSurvey);

                seedCtx.SaveChanges();
            };
        }

        [Test]
        public async Task findSurvey_whenNotExists_ReturnsNull()
        {
            var testSurvey = await testRepo.findAsync(fixture.Create<Guid>());

            testSurvey.Should().BeNull();
        }

        [Test]
        public async Task insert_addsNewSurvey()
        {
            var testSurvey = new Survey(seedSite.Id, 
                                        fixture.CreateMany<int>().ToList(),
                                        fixture.Create<string>());

            var created = await testRepo.insertAsync(testSurvey);
            testCtx.SaveChanges();

            created.Id.Should().NotBeEmpty();

            using(var assertCtx = new SiteSurveyContext(ctxOptions)) {
                var inserted = assertCtx.Surveys.Find(testSurvey.Id);
                inserted.Should().BeEquivalentTo(created);
            }
        }

        [Test]
        [Ignore("check fk")]
        public void insert_whenSiteNotExist_throwsException()
        {
            var testSurvey = new Survey(fixture.Create<Guid>(), 
                                        fixture.CreateMany<int>().ToList(),
                                        fixture.Create<string>());

            Assert.ThrowsAsync<DbUpdateException>( async () => { 
                await testRepo.insertAsync(testSurvey); 
            });
        }

        [Test]
        public async Task update_withChangeToFields_UpdatesSurvey()
        {
            var testSurvey = new Survey(seedSurvey.Id, seedSite.Id, 
                                        fixture.CreateMany<int>().ToList(),
                                        fixture.Create<string>());
            
            testSurvey.CoordinatorName = fixture.Create<string>("coordinator");
            testSurvey.StartTimeStamp = fixture.Create<DateTime>().ToUniversalTime();
            testSurvey.EndTimeStamp = testSurvey.StartTimeStamp.AddHours(1);
            testSurvey.VolunteerCount = fixture.Create<Int16>();
            testSurvey.TotalKg = fixture.Create<Decimal>();

            var updated = await testRepo.updateAsync(testSurvey);
            testCtx.SaveChanges();

            using( var assertCtx = new SiteSurveyContext(ctxOptions) ) {
                var testUpdated = assertCtx.Surveys.Find(seedSurvey.Id);
                testUpdated.Should().BeEquivalentTo(testSurvey, opt => opt
                    .Excluding( x => x.CreateUserId )
                    .Excluding( x => x.CreateTimestamp )
                    .Excluding( x => x.LitterItems )
                );
           }
        }
    }
}