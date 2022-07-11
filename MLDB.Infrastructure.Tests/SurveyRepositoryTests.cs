using System;
using MLDB.Infrastructure.Repositories;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FluentAssertions;
using MLDB.Domain;
using AutoFixture;
using System.Linq;
using System.Collections.Generic;
using Npgsql;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Configurations;


namespace MLDB.Infrastructure.IntegrationTests
{
    [TestOf(typeof(SurveyRepository))]
    public class SurveyRepositoryTests
    {
        private DbContextOptions<SiteSurveyContext> ctxOptions;
        private SiteSurveyContext testCtx;

        private ISurveyRepository testRepo;

        private Fixture fixture;

        private Site seedSite;

        private Survey seedSurvey, seedSurveyWithItems;

        private List<LitterItem> seedLitterItems;

        [SetUp]
        public async Task Setup()
        {
            fixture = new AutoFixture.Fixture();

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

            testRepo = new SurveyRepository(testCtx);
        }

        [TearDown]
        public void TearDown()
        {
            testCtx.Dispose();
        }

        private void seedTestData() {
            using( var seedCtx = new SiteSurveyContext(ctxOptions)) {
                seedSite = fixture.Build<Site>()
                                    .Create();
                seedCtx.Add(seedSite);

                seedSurvey = new Survey(seedSite.Id, 
                                        fixture.Create<string>());
                seedCtx.Add(seedSurvey);

                seedLitterItems = fixture.CreateMany<LitterItem>().ToList();
                seedSurveyWithItems = fixture.Build<Survey>()
                                            .With( x => x.SiteId, seedSite.Id)
                                            .Create();
                seedSurveyWithItems.updateLitterItems(seedLitterItems);
                seedCtx.Add(seedSurveyWithItems);

                seedCtx.SaveChanges();
            };
        }

        [Test]
        public void exists_whenNotExists_ReturnsFalse()
        {
            var surveyExists = testRepo.exists(fixture.Create<Guid>());

            surveyExists.Should().BeFalse();
        }

        [Test]
        public void exists_whenExists_ReturnsTrue()
        {
            var surveyExists = testRepo.exists(seedSurvey.Id);

            surveyExists.Should().BeTrue();
        }

        [Test]
        public async Task findSurvey_whenNotExists_ReturnsNull()
        {
            var testSurveys = await testRepo.findAsync(fixture.Create<Guid>());

            testSurveys.Should().BeNull();
        }

        
        public async Task findSurvey_whenExists_ReturnsSurvey()
        {
            var testSurvey = await testRepo.findAsync(seedSurveyWithItems.Id);

            testSurvey.Should().BeEquivalentTo( seedSurveyWithItems );
        }

        [Test]
        public async Task getSurveysForSite_whenNotExists_ReturnsEmpty()
        {
            var testSurveys = await testRepo.getSurveysForSite(fixture.Create<Guid>());

            testSurveys.Should().BeEmpty();
        }

        [Test]
        public async Task getSurveysForSite_whenExists_ReturnsSurveys()
        {
            var testSurveys = await testRepo.getSurveysForSite(seedSite.Id);

            testSurveys.Should().HaveCount(2);
            testSurveys.Should().OnlyContain( x => x.Id == seedSurvey.Id || x.Id == seedSurveyWithItems.Id);
        }

        [Test]
        public async Task insert_addsNewSurvey()
        {
            var testSurvey = new Survey(seedSite.Id, 
                                        fixture.Create<string>());

            var created = await testRepo.insertAsync(testSurvey);
            testCtx.SaveChanges();

            created.Id.Should().NotBeEmpty();

            using(var assertCtx = new SiteSurveyContext(ctxOptions)) {
                var inserted = assertCtx.Surveys.Find(testSurvey.Id);
                inserted.Should().BeEquivalentTo(created, opts => opts
                    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(0,0,0,1)))
                    .When(info => info.Path.EndsWith("Timestamp")));
            }
        }

        [Test]
        public void insert_whenSiteNotExist_throwsException()
        {
            var testSurvey = new Survey(fixture.Create<Guid>(), 
                                        fixture.Create<string>());

            Assert.ThrowsAsync<DbUpdateException>( async () => { 
                await testRepo.insertAsync(testSurvey); 
                testCtx.SaveChanges();
            });
        }

        [Test]
        public async Task update_withChangeToFields_UpdatesSurvey()
        {
            Survey  testSurvey;
            using( var readCtx = new SiteSurveyContext(ctxOptions) ) {
                testSurvey = readCtx.Surveys.Find(seedSurvey.Id);
            }
            
            testSurvey.CoordinatorName = fixture.Create<string>("coordinator");
            testSurvey.StartTimeStamp = fixture.Create<DateTime>().ToUniversalTime();
            testSurvey.EndTimeStamp = testSurvey.StartTimeStamp.AddHours(1);
            testSurvey.VolunteerCount = fixture.Create<Int16>();
            testSurvey.TotalKg = fixture.Create<Decimal>();

            var updated = await testRepo.updateAsync(testSurvey);
            testCtx.SaveChanges();

            using( var assertCtx = new SiteSurveyContext(ctxOptions) ) {
                var testUpdated = assertCtx.Surveys.Find(seedSurvey.Id);
                testUpdated.Should().BeEquivalentTo(testSurvey, opts => opts
                    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, new TimeSpan(0,0,0,1)))
                    .When(info => info.Path.EndsWith("TimeStamp")));
           }
        }

        [Test]
        public async Task update_withNewLitterItems_AddsThemToSurvey()
        {
            Survey  testSurvey;
            using( var readCtx = new SiteSurveyContext(ctxOptions) ) {
                testSurvey = readCtx.Surveys.Find(seedSurvey.Id);
            }
            var testItems = seedLitterItems;
            
            testSurvey.updateLitterItems(testItems);

            var updated = await testRepo.updateAsync(testSurvey);
            testCtx.SaveChanges();

            using( var assertCtx = new SiteSurveyContext(ctxOptions) ) {
                var testUpdated = assertCtx.Surveys.Find(seedSurvey.Id);
                testUpdated.LitterItems.Should().HaveSameCount(testItems);
                testUpdated.LitterItems.Should().Contain(testItems);
           }
        }

        [Test]
        public async Task update_withRemovedLitterItems_RemovesThemFromSurvey()
        {
            Survey  testSurvey;
            using( var readCtx = new SiteSurveyContext(ctxOptions) ) {
                testSurvey = readCtx.Surveys.Find(seedSurveyWithItems.Id);
            }
            var testItems = seedLitterItems.GetRange(0,1);
            
            testSurvey.updateLitterItems(testItems);

            var updated = await testRepo.updateAsync(testSurvey);
            testCtx.SaveChanges();

            using( var assertCtx = new SiteSurveyContext(ctxOptions) ) {
                var testUpdated = assertCtx.Surveys.Find(seedSurveyWithItems.Id);
                testUpdated.LitterItems.Should().HaveSameCount(testItems);
                testUpdated.LitterItems.Should().Contain(testItems);
           }
        }

        [Test]
        public async Task update_withChangedLitterItems_UpdatesSurvey()
        {
            Survey  testSurvey;
            using( var readCtx = new SiteSurveyContext(ctxOptions) ) {
                testSurvey = readCtx.Surveys.Find(seedSurveyWithItems.Id);
            }
            var updateItem = testSurvey.LitterItems.First();
            updateItem.Count = fixture.Create<int>();

            var updated = await testRepo.updateAsync(testSurvey);
            testCtx.SaveChanges();

            using( var assertCtx = new SiteSurveyContext(ctxOptions) ) {
                var testUpdated = assertCtx.Surveys.Find(testSurvey.Id);
                testUpdated.LitterItems.Should().Contain( x => x.LitterTypeId == updateItem.LitterTypeId && x.Count == updateItem.Count);
           }
        }
    }
}