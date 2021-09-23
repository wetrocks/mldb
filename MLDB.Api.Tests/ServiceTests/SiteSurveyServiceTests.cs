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

namespace MLDB.Api.Tests.ServiceTests
{
    public class SiteSurveyServiceTests
    {
        private SiteSurveyContext testCtx;
        private ISiteSurveyService testSvc;

        private Mock<IUserService> userSvc;

        private Site testSite;


        [SetUp]
        public void Setup()
        {
            var conn = new SqliteConnection("DataSource=:memory:");
            conn.Open(); 

            var options = new DbContextOptionsBuilder<SiteSurveyContext>()
                        .UseSqlite(conn)
                        .Options;
            testCtx = new SiteSurveyContext(options);
            testCtx.Database.EnsureCreated();

            userSvc = new Mock<IUserService>();

            testSvc = new SiteSurveyService(testCtx, userSvc.Object);
        }

        [Test]
        public async Task create_AddsNewSiteSurvey()
        {
            const string TEST_USER = "testUserID";
            
            // can't really mock so store test data
            var testUser = new User(TEST_USER);
            testUser.Name = "DB_User";
            testCtx.Users.Add(testUser);

            var testSite = new Site();
            testSite.Id = System.Guid.NewGuid();
            testSite.Name = "siteName";
            testSite.CreateUser = testUser;

            testCtx.Sites.Add(testSite);
            testCtx.SaveChanges();


            userSvc.Setup( x => x.createFromClaimsPrinicpal(It.IsAny<ClaimsPrincipal>()))
                   .Returns(new User(TEST_USER));

            var testSurvey = new Survey();
            testSurvey.Date = DateTime.Today.ToUniversalTime();

            var newSurvey = await testSvc.create(testSurvey, testSite.Id, new ClaimsPrincipal());

            newSurvey.Id.Should().NotBe(System.Guid.Empty);
            newSurvey.CreateUser.Should().BeEquivalentTo(testUser);
            // close enough
            newSurvey.CreateTimestamp.Should().BeSameDateAs(System.DateTime.UtcNow);

            // only way to test if saved to db currently
            var createdSurvey = testCtx.Surveys.Find(testSurvey.Id);
            createdSurvey.Should().NotBe(null);

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
            const string TEST_USER = "testUserID";
            // can't really mock so store test data
            var testUser = new User(TEST_USER);
            testUser.Name = "DB_User";
            testCtx.Users.Add(testUser);

            var testSite = new Site();
            testSite.Id = System.Guid.NewGuid();
            testSite.Name = "siteName";
            testSite.CreateUser = testUser;
            testCtx.Sites.Add(testSite);
            testCtx.SaveChanges();
            
            userSvc.Setup( x => x.createFromClaimsPrinicpal(It.IsAny<ClaimsPrincipal>()))
                   .Returns(new User("DIFFERENT_USER"));

            var testSurvey = new Survey();
            testSurvey.Date = DateTime.Today.ToUniversalTime();

            var newSurvey = await testSvc.create(testSurvey, testSite.Id, new ClaimsPrincipal());
            newSurvey.CreateUser.IdpId.Should().Be("DIFFERENT_USER");

            // only way to test if saved to db currently
            var createdSurvey = testCtx.Surveys.Find(newSurvey.Id);
            createdSurvey.Should().NotBe(null);

            var createdUser = testCtx.Users.Find("DIFFERENT_USER");
            createdUser.Should().NotBe(null);
        }
    }
}