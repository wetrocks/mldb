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

namespace MLDB.Api.Tests.ServiceTests
{
    public class SiteServiceTests
    {
        private SiteSurveyContext testCtx;
        private ISiteService testSvc;

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

            testSvc = new SiteService(testCtx, userSvc.Object);
        }

        [Test]
        public async Task find_returnsSite_WhenExists()
        {
            
            var testSite = new Site();
            testSite.Id = System.Guid.NewGuid();
            var testSurvey = new Survey();
            testSurvey.StartTimeStamp = System.DateTime.Today.ToUniversalTime();
            testSurvey.SiteId = testSite.Id;
            testCtx.Add(testSite);
            testCtx.Add(testSurvey);
            testCtx.SaveChanges();

            userSvc.Setup( x => x.createFromClaimsPrinicpal(It.IsAny<ClaimsPrincipal>()))
                   .Returns(new User("testUserID"));

            var foundSite = await testSvc.find(testSite.Id);
            foundSite.Surveys.Should().HaveCount(1);
        }


        [Test]
        public async Task create_AddsNewSite()
        {
            const string TEST_USER = "testUserID";
            
            // store test user 
            var testUser = new User(TEST_USER);
            testUser.Name = "DB_User";
            testCtx.Users.Add(testUser);
            testCtx.SaveChanges();

            userSvc.Setup( x => x.createFromClaimsPrinicpal(It.IsAny<ClaimsPrincipal>()))
                   .Returns(new User(TEST_USER));

            testSite = new Site();
            testSite.Name = "siteName";


            var newSvc = await testSvc.create(testSite, new ClaimsPrincipal());

            newSvc.Id.Should().NotBe(System.Guid.Empty);
            newSvc.CreateUser.Should().BeEquivalentTo(testUser);
            // close enough
            newSvc.CreateTimestamp.Should().BeSameDateAs(System.DateTime.UtcNow);

            // only way to test if saved to db currently
            var createdSite = testCtx.Sites.Find(newSvc.Id);
            createdSite.Should().NotBe(null);
        }

        [Test]
        public async Task create_WhenUserNotExist_UserIsCreated()
        {
            const string TEST_USER = "notInDb";
            
            userSvc.Setup( x => x.createFromClaimsPrinicpal(It.IsAny<ClaimsPrincipal>()))
                   .Returns(new User(TEST_USER));

            testSite = new Site();
            testSite.Name = "siteName";

            var newSvc = await testSvc.create(testSite, new ClaimsPrincipal());
            newSvc.CreateUser.IdpId.Should().Be(TEST_USER);

            // only way to test if saved to db currently
            var createdSite = testCtx.Sites.Find(newSvc.Id);
            createdSite.Should().NotBe(null);

            var createdUser = testCtx.Users.Find(TEST_USER);
            createdUser.Should().NotBe(null);
        }
    }
}