using NUnit.Framework;
using Moq;
using FluentAssertions;
using System.Security.Claims;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

using MLDB.Api.Services;
using MLDB.Api.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace MLDB.Api.Tests.ServiceTests
{
    public class SiteSurveyServiceTests
    {
        private SqliteConnection conn;
        private DbContextOptions<SiteSurveyContext> ctxOptions;
        private SiteSurveyContext testCtx;
        private ISiteSurveyService testSvc;

        private Mock<IUserService> userSvc;

        const string TEST_USER_ID = "testUserID";

        private User testUser;

        private Site testSite;


        [SetUp]
        public void Setup()
        {
            conn = new SqliteConnection("DataSource=:memory:");
            conn.Open(); 

            ctxOptions = new DbContextOptionsBuilder<SiteSurveyContext>()
                        .UseSqlite(conn)
                        .Options;
            testCtx = new SiteSurveyContext(ctxOptions);
            testCtx.Database.EnsureCreated();

            userSvc = new Mock<IUserService>();
            userSvc.Setup( x => x.createFromClaimsPrinicpal(It.IsAny<ClaimsPrincipal>()))
                   .Returns(new User(TEST_USER_ID));

            testSvc = new SiteSurveyService(testCtx, userSvc.Object);

            testSite = createTestSite();

            testUser  = new User(TEST_USER_ID);
            testUser.Name = "test user name";
        }

        [TearDown]
        public void TearDown()
        {
            conn.Close();
        }

        private Site createTestSite(String siteName = "test site") {
            var newSite = new Site();
            newSite.Id = System.Guid.NewGuid();
            newSite.Name = siteName;

            return newSite;
        }

        [Test]
        public async Task create_AddsNewSiteSurvey()
        {
            // can't really mock so store test data
            testSite.CreateUser = testUser;
            testCtx.Sites.Add(testSite);
            testCtx.SaveChanges();


            var testSurvey = new Survey();
            testSurvey.StartTimeStamp = DateTime.Today.ToUniversalTime();
            testSurvey.LitterItems =  new List<LitterItem>() {
                new LitterItem() { LitterType = new LitterType() { Id = 42 }, Count = 1 },
                new LitterItem() { LitterType = new LitterType() { Id = 43 }, Count = 3 }
            };

            var newSurvey = await testSvc.create(testSurvey, testSite.Id, new ClaimsPrincipal());

            newSurvey.Id.Should().NotBe(System.Guid.Empty);
            newSurvey.CreateUser.Should().BeEquivalentTo(testUser);
            // close enough
            newSurvey.CreateTimestamp.Should().BeSameDateAs(System.DateTime.UtcNow);

            // only way to test if saved to db currently
            var createdSurvey = testCtx.Surveys.Find(testSurvey.Id);
            createdSurvey.Should().NotBe(null);
            createdSurvey.LitterItems.Should().HaveCount(2);

            var updatedSite = testCtx.Sites.Find(testSurvey.SiteId);
            updatedSite.Should().NotBe(null);
            updatedSite.Surveys.Should().Contain(createdSurvey);
        }

        [Test]
        public async Task create_WhenSiteNotExist_ThrowsException()
        {
            var testSurvey = new Survey();

            Func<Task> create = () => testSvc.create(testSurvey, System.Guid.NewGuid(), new ClaimsPrincipal());

            await create.Should().ThrowAsync<InvalidOperationException>()
                        .WithMessage("Site * does not exist");
        }

        [Test]
        public async Task create_WhenUserNotExist_UserIsCreated()
        {
            // can't really mock so store test data
            testSite.CreateUser = testUser;
            testCtx.Sites.Add(testSite);
            testCtx.SaveChanges();
            
            userSvc.Setup( x => x.createFromClaimsPrinicpal(It.IsAny<ClaimsPrincipal>()))
                   .Returns(new User("DIFFERENT_USER"));

            var testSurvey = new Survey();
            testSurvey.StartTimeStamp = DateTime.Today.ToUniversalTime();

            var newSurvey = await testSvc.create(testSurvey, testSite.Id, new ClaimsPrincipal());
            newSurvey.CreateUser.IdpId.Should().Be("DIFFERENT_USER");

            // only way to test if saved to db currently
            var createdSurvey = testCtx.Surveys.Find(newSurvey.Id);
            createdSurvey.Should().NotBe(null);

            var createdUser = testCtx.Users.Find("DIFFERENT_USER");
            createdUser.Should().NotBe(null);
        }

        [Test]
        public async Task update_UpdatesSurvey()
        {
            testSite.CreateUser = testUser;
            testCtx.Sites.Add(testSite);

            var testSurvey = new Survey();
            testSurvey.StartTimeStamp = DateTime.Today.ToUniversalTime();
            testSurvey.Id = Guid.NewGuid();
            testSurvey.SiteId = testSite.Id;
            testSurvey.Coordinator = "orig coordinator";
            testCtx.Surveys.Add(testSurvey);
            testCtx.SaveChanges();

            var updateSurvey = new Survey();
            updateSurvey.Id = testSurvey.Id;
            updateSurvey.SiteId = testSite.Id;
            updateSurvey.StartTimeStamp = testSurvey.StartTimeStamp;
            updateSurvey.Coordinator = "new coordinator";

            var newCtx = new SiteSurveyContext(ctxOptions);
            var updateService = new SiteSurveyService(newCtx, userSvc.Object);

            var foo = await updateService.update(updateSurvey, testSite.Id, new ClaimsPrincipal());

            var updatedSurvey = newCtx.Surveys.Find(updateSurvey.Id);
            updatedSurvey.Should().NotBeNull();
        }
    }
}