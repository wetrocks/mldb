using NUnit.Framework;
using AutoFixture;
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
using System.Collections.Generic;


namespace MLDB.Api.IntegrationTests.ServiceTests
{
    [TestOf(typeof(SiteService))]
    public class SiteServiceTests
    {
        private SqliteConnection conn;
        private DbContextOptions<SiteSurveyContext> ctxOptions;
        private SiteSurveyContext testCtx;

        private ISiteService testSvc;

        private User seedUser;

        private Site seedSite;

        private readonly Fixture fixture = new Fixture();

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

            testSvc = new SiteService(testCtx);
            
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
                seedUser  = new User("seedIdpID");
                seedUser.Name = "seed user name";
                seedCtx.Add(seedUser);

                seedSite = fixture.Build<Site>()
                                .With( x => x.CreateUser, seedUser )
                                .Without( x => x.Surveys )
                                .Create();
                seedCtx.Add(seedSite);

                seedCtx.SaveChanges();
            };
        }

        [Test]
        public async Task getSite_WhenExists_returnsSite()
        {
            var foundSite = await testSvc.getSite(seedSite.Id);
            foundSite.Should().BeEquivalentTo(seedSite, opt => opt.Excluding( x => x.Surveys) );
        }

        [Test]
        public async Task create_AddsNewSite()
        {
            var testSite = fixture.Build<Site>()
                            .Without( x => x.Id )
                            .Without( x => x.Surveys )
                            .Create();

            var newSite = await testSvc.create(testSite, seedUser);

            newSite.Should().BeEquivalentTo(testSite);
            
            using( var assertCtx = new SiteSurveyContext(ctxOptions)) {
                var createdSite = assertCtx.Sites.Include( x => x.CreateUser )
                                                .SingleOrDefault( x => x.Id == newSite.Id );
                createdSite.Should().BeEquivalentTo(testSite);
            }
        }

        [Test]
        public async Task update_WhenSiteExists_Updates()
        {
            var testSite = fixture.Build<Site>()
                            .With( x => x.Id, seedSite.Id)
                            .Without( x => x.Surveys )
                            .Create();

            var returnedSite = await testSvc.update(testSite, seedUser);

            returnedSite.Should().BeEquivalentTo(seedSite, opt => opt.Excluding( x => x.Name) );
            returnedSite.Name.Should().Be(testSite.Name);

            using( var assertCtx = new SiteSurveyContext(ctxOptions)) {
                var updatedSite = assertCtx.Sites.Include( x => x.CreateUser )
                                                .SingleOrDefault( x => x.Id == seedSite.Id );
                updatedSite.Should().BeEquivalentTo(seedSite, opt => opt.Excluding( x => x.Name) );
                updatedSite.Name.Should().Be(testSite.Name);
            }
        }

        [Test]
        public async Task update_WhenSiteNotExists_ThrowsException()
        {
            var testSite = fixture.Build<Site>()
                            .Without( x => x.Surveys )
                            .Create();

            Func<Task> updateAction = () => testSvc.update(testSite, seedUser);

            await updateAction.Should().ThrowAsync<System.Data.RowNotInTableException>();
        }
    }
}