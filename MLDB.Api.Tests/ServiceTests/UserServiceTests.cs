using NUnit.Framework;
using FluentAssertions;
using System.Security.Claims;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

using MLDB.Api.Services;
using MLDB.Api.Models;

namespace MLDB.Api.Tests.ServiceTests
{
    public class UserServiceTests
    {
        private IUserService testSvc;

        private  Claim[] ALL_CLAIMS =  {
            new Claim(ClaimTypes.NameIdentifier, "testIdpUserName"),
            new Claim(IUserService.EMAIL_CLAIMTYPE, "test@test.net"),
            new Claim(IUserService.EMAIL_VERIFIED_CLAIMTYPE, "true"),
            new Claim(IUserService.NAME_CLAIMTYPE, "First Last")
        };

        private ClaimsPrincipal createPrinicipal(Claim[] claims) {
            return new ClaimsPrincipal(
                new ClaimsIdentity(claims,"TEST_AUTH")
            );
        }

        [SetUp]
        public void Setup()
        {
            testSvc = new UserService();
        }

        [Test]
        public void createFromClaimsPrinicpal_ShouldSetUserProperties()
        {
            var testUser = testSvc.createFromClaimsPrinicpal(createPrinicipal(ALL_CLAIMS));

            testUser.IdpId.Should().Be("testIdpUserName");
            testUser.Email.Should().Be("test@test.net");
            testUser.EmailVerified.Should().Be(true);
            testUser.Name.Should().Be("First Last");
            // close enough
            testUser.CreateTime.Should().BeSameDateAs(System.DateTime.UtcNow);
            testUser.UpdateTime.Should().Be(System.DateTime.MinValue);
        }

        [Test]
        public void createFromClaimsPrinicpal_ShouldCreateWithNoEmail()
        {
            var testUser = testSvc.createFromClaimsPrinicpal(
                createPrinicipal(ALL_CLAIMS.Where(x => x.Type != IUserService.EMAIL_CLAIMTYPE).ToArray())
            );

            testUser.Email.Should().Be(null);
            testUser.EmailVerified.Should().Be(false);
        }

        [Test]
        public void createFromClaimsPrinicpal_ShouldDefaultToUnverified()
        {
            var testUser = testSvc.createFromClaimsPrinicpal(
                createPrinicipal(ALL_CLAIMS.Where(x => x.Type != IUserService.EMAIL_VERIFIED_CLAIMTYPE).ToArray())
            );

            testUser.Email.Should().Be("test@test.net");
            testUser.EmailVerified.Should().Be(false);
        }

        [Test]
        public void createFromClaimsPrinicpal_ShouldCreateWithNoName()
        {
            var testUser = testSvc.createFromClaimsPrinicpal(
                createPrinicipal(ALL_CLAIMS.Where(x => x.Type != IUserService.NAME_CLAIMTYPE).ToArray())
            );

            testUser.Name.Should().Be(null);
        }

    }
}