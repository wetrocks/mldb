using NUnit.Framework;
using Moq;
using FluentAssertions;
using System.Security.Claims;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Sqlite;

using MLDB.Api.Services;
using MLDB.Api.Models;
using System.Threading.Tasks;
using System;
using MLDB.Api.Repositories;
using System.Collections.Generic;

using AutoFixture;

namespace MLDB.Api.Tests.RepoIntegrationTests
{
    [TestOf(typeof(SurveyRepository))]
    public class SurveyRepositoryTests
    {
        private SqliteConnection conn;
        private DbContextOptions<SiteSurveyContext> ctxOptions;
        private SiteSurveyContext testCtx;

        private User seedUser;

        private Site seedSite;

        private Survey seedSurvey;

        private ISurveyRepository testRepo;

        [SetUp]
        public void Setup()
        {
            conn = new SqliteConnection("DataSource=:memory:");
            conn.Open(); 
            
            ctxOptions = new DbContextOptionsBuilder<SiteSurveyContext>()
                        // .LogTo( Console.WriteLine)
                        // .EnableSensitiveDataLogging()
                        .UseSqlite(conn)
                        .Options;
            testCtx = new SiteSurveyContext(ctxOptions);
            testCtx.Database.EnsureCreated();

            seedTestData();

            testRepo = new SurveyRepository(testCtx);
            
            // make user "exist"
            testCtx.Attach(seedUser);
        }

        [TearDown]
        public void TearDown()
        {
            testCtx.Dispose();
            conn.Close();
        }

        private void seedTestData() {
            using( var seedCtx = new SiteSurveyContext(ctxOptions)) {
                // litter item types
                var item42 = new LitterType();
                item42.Id = 42;
                seedCtx.Add(item42);

                var item43 = new LitterType();
                item43.Id = 43;
                seedCtx.Add(item43);

                var fixture = new Fixture();

                seedUser  = new User("seedIdpID");
                seedUser.Name = "seed user name";
                seedCtx.Add(seedUser);

                seedSite = fixture.Build<Site>()
                                .With( x => x.CreateUser, seedUser )
                                .Without( x => x.Surveys )
                                .Create();
                seedCtx.Add(seedSite);

                seedSurvey = fixture.Build<Survey>()
                                .With( x => x.SiteId, seedSite.Id )
                                .With( x => x.CreateUser, seedUser )
                                .Without( x => x.LitterItems )
                                .Create();
                seedCtx.Add(seedSurvey);

                seedCtx.SaveChanges();
            };
        }


        [Test]
        public async Task insert_AddsNewSurvey()
        {
            var fixture = new Fixture();
            var testSurvey = fixture.Build<Survey>()
                            .Without( x => x.Id )
                            .With( x => x.SiteId, seedSite.Id )
                            .With( x => x.CreateUser, seedUser )
                            .With( x => x.LitterItems, new List<LitterItem>() )
                            .Create();


            var created = await testRepo.insertAsync(testSurvey);

            using(var assertCtx = new SiteSurveyContext(ctxOptions)) {
                var inserted = assertCtx.Surveys.Where( x => x.Id == created.Id )
                                                .Include( x => x.CreateUser )
                                                .FirstOrDefault();
                inserted.Should().BeEquivalentTo(created);
            }
        }

        [Test]
        public async Task insert_WhenHasLitterItems_AddsThem()
        {
            var testSurvey = new Survey();
            testSurvey.StartTimeStamp = DateTime.Today.ToUniversalTime();
            testSurvey.SiteId = seedSite.Id;
            testSurvey.CreateUser = seedUser;
            testSurvey.LitterItems =  new List<LitterItem>() {
                new LitterItem() { LitterTypeId = 42, Count = 1 },
                new LitterItem() { LitterTypeId = 43, Count = 3 }
            };

            var created = await testRepo.insertAsync(testSurvey);

            var assertCtx = new SiteSurveyContext(ctxOptions);
            var inserted = assertCtx.Surveys.Find( created.Id );
            inserted.LitterItems.Should().HaveCount(2);
        }

        [Test]
        public async Task insert_returnSurveyWithId()
        {
            var testSurvey = new Survey();
            testSurvey.StartTimeStamp = DateTime.Today.ToUniversalTime();
            testSurvey.SiteId = seedSite.Id;
            testSurvey.CreateUser = seedUser;

            var inserted = await testRepo.insertAsync(testSurvey);

            inserted.Id.Should().NotBeEmpty();
        }

        [Test]
        public void insert_whenSiteNotExist_throwsException()
        {
            var nonExistentSite = Guid.NewGuid();
            
            var testSurvey = new Survey();
            testSurvey.StartTimeStamp = DateTime.Today.ToUniversalTime();
            testSurvey.SiteId = nonExistentSite;
            testSurvey.CreateUser = new User("testIdpID");

            Assert.ThrowsAsync<DbUpdateException>( async () => { 
                await testRepo.insertAsync(testSurvey); 
            });
        }

        [Test]
        public async Task update_UpdatesSurvey()
        {
            var fixture = new Fixture();
            var testSurvey = fixture.Build<Survey>()
                            .With( x => x.Id, seedSurvey.Id )
                            .With( x => x.SiteId, seedSite.Id )
                            .With( x => x.CreateUser, seedUser )
                            .With( x => x.CreateTimestamp, seedSurvey.CreateTimestamp )
                            .With( x => x.LitterItems, new List<LitterItem>() )
                            .Create();

            await testRepo.updateAsync(testSurvey);

            using( var assertCtx = new SiteSurveyContext(ctxOptions) ) {
                var updated = assertCtx.Surveys
                                .Include( x => x.CreateUser )
                                .SingleOrDefault();
                updated.Should().BeEquivalentTo(testSurvey);
            }
        }

        [Test]
        public async Task update_WithNewLitterItems_AddsThem()
        {
            var testSurvey = new Survey();
            testSurvey.Id = seedSurvey.Id;
            testSurvey.SiteId = seedSite.Id;
            testSurvey.CreateUser = seedUser;
            testSurvey.LitterItems = new List<LitterItem>() {
                new LitterItem() { LitterTypeId = 42, Count = 1 }
            };

            await testRepo.updateAsync(testSurvey);

            var assertCtx = new SiteSurveyContext(ctxOptions);
            var updated = assertCtx.Surveys.SingleOrDefault();
            updated.LitterItems.Should().HaveCount(1);
        }

        [Test]
        public async Task update_WithChangedCount_UpdateItem()
        {
            // add litter item to existing survey
            var updateCtx = new SiteSurveyContext(ctxOptions);
            var existing = testCtx.Surveys.Find(seedSurvey.Id);
            existing.LitterItems = new List<LitterItem>() {
                new LitterItem() { LitterTypeId = 42, Count = 1 }
            };
            updateCtx.SaveChanges();
            updateCtx.Dispose();

            // TODO: replace w/ autofixture (everywhere)
            var testSurvey = new Survey();
            testSurvey.Id = seedSurvey.Id;
            testSurvey.SiteId = seedSite.Id;
            testSurvey.VolunteerCount = seedSurvey.VolunteerCount;
            testSurvey.CreateUser = seedUser;
            testSurvey.LitterItems = new List<LitterItem>() {
                new LitterItem() { LitterTypeId = 42, Count = 419 }
            };

            await testRepo.updateAsync(testSurvey);

            var assertCtx = new SiteSurveyContext(ctxOptions);
            var updated = assertCtx.Surveys.Find(seedSurvey.Id);
            updated.LitterItems.Should().HaveCount(1);
            updated.LitterItems.Should().OnlyContain( x => x.LitterTypeId == 42 && x.Count == 419 );
        }

        [Test]
        public async Task update_WithRemovedItem_DeletesItem()
        {
            // add litter item to existing survey
            var updateCtx = new SiteSurveyContext(ctxOptions);
            var existing = testCtx.Surveys.Find(seedSurvey.Id);
            existing.LitterItems = new List<LitterItem>() {
                new LitterItem() { LitterTypeId = 42, Count = 1 },
                new LitterItem() { LitterTypeId = 43, Count = 1 }
            };
            updateCtx.SaveChanges();
            updateCtx.Dispose();


            var testSurvey = new Survey();
            testSurvey.Id = seedSurvey.Id;
            testSurvey.SiteId = seedSite.Id;
            testSurvey.VolunteerCount = seedSurvey.VolunteerCount;
            testSurvey.CreateUser = seedUser;
            testSurvey.LitterItems = new List<LitterItem>() {
                new LitterItem() { LitterTypeId = 42, Count = 2 }
            };

            await testRepo.updateAsync(testSurvey);

            var assertCtx = new SiteSurveyContext(ctxOptions);
            var updated = assertCtx.Surveys.Find(seedSurvey.Id);
            updated.LitterItems.Should().HaveCount(1);
            updated.LitterItems.Should().OnlyContain( x => x.LitterTypeId == 42 && x.Count == 2 );
        }
    }
}