using NUnit.Framework;
using FluentAssertions;
using System.Security.Claims;
using System.Linq;
using MLDB.Api.Services;
using MLDB.Api.Jwt;
using Moq;
using MLDB.Domain;
using AutoFixture;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MLDB.Api.Tests.ServiceTests
{
    public class UserServiceTests
    { 
        private IUserService testSvc;

        private Mock<IUserRepository> userRepo;

        private Fixture fixture;

        private readonly Claim[] ALL_CLAIMS =  {
            new Claim(ClaimTypes.NameIdentifier, "testIdpUserName"),
            new Claim(TokenClaims.EMAIL_CLAIMTYPE, "test@test.net"),
            new Claim(TokenClaims.EMAIL_VERIFIED_CLAIMTYPE, "true"),
            new Claim(TokenClaims.NAME_CLAIMTYPE, "First Last")
        };

        private ClaimsPrincipal createPrinicipal(Claim[] claims) {
            return new ClaimsPrincipal(
                new ClaimsIdentity(claims,"TEST_AUTH")
            );
        }

        [SetUp]
        public void Setup()
        {
            userRepo = new Mock<IUserRepository>();
            testSvc = new UserService(userRepo.Object, new Mock<ILogger<UserService>>().Object);

            fixture = new Fixture();
        }

        [Test]
        public void createFromClaimsPrinicpal_ShouldSetUserProperties()
        {
            var testUser = testSvc.createFromClaimsPrinicpal(createPrinicipal(ALL_CLAIMS)); 

            testUser.IdpId.Should().Be("testIdpUserName");
            testUser.Email.Should().Be("test@test.net");
            testUser.EmailVerified.Should().Be(true);
            testUser.Name.Should().Be("First Last");
        }

        [Test]
        public void createFromClaimsPrinicpal_WithoutEmail_ShouldCreate()
        {
            var testUser = testSvc.createFromClaimsPrinicpal(
                createPrinicipal(ALL_CLAIMS.Where(x => x.Type != TokenClaims.EMAIL_CLAIMTYPE).ToArray())
            );

            testUser.Email.Should().Be(null);
            testUser.EmailVerified.Should().Be(false);
        }

        [Test]
        public void createFromClaimsPrinicpal_WithoutEmailVerified_ShouldDefaultToUnverified()
        {
            var testUser = testSvc.createFromClaimsPrinicpal(
                createPrinicipal(ALL_CLAIMS.Where(x => x.Type != TokenClaims.EMAIL_VERIFIED_CLAIMTYPE).ToArray())
            );

            testUser.Email.Should().Be("test@test.net");
            testUser.EmailVerified.Should().Be(false);
        }

        [Test]
        public void createFromClaimsPrinicpal_WithoutName_ShouldCreate()
        {
            var testUser = testSvc.createFromClaimsPrinicpal(
                createPrinicipal(ALL_CLAIMS.Where(x => x.Type != TokenClaims.NAME_CLAIMTYPE).ToArray())
            );

            testUser.Name.Should().Be(null);
        }

        [Test]
        public async Task findOrAddUser_WhenExists_ReturnsUser() {
            var testPrincipal =  createPrinicipal(ALL_CLAIMS);
            var testIdpUser = testPrincipal.Claims.Single(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;

            var dbUser = fixture.Build<User>()
                                .With( u => u.IdpId, testIdpUser)
                                .Create();

            userRepo.Setup(  x => x.findByIdpIdAsync(testIdpUser))
                    .ReturnsAsync( dbUser );

            var user = await testSvc.findOrAddUserAsync(testPrincipal);

            userRepo.Verify( x => x.findByIdpIdAsync(testIdpUser), Times.Once);
            user.Should().BeEquivalentTo(dbUser);
        }

        [Test]
        public async Task findOrAddUser_WhenNotExists_CreatesUser() {
            var testPrincipal =  createPrinicipal(ALL_CLAIMS);
            var testIdpUser = testPrincipal.Claims.Single(x => x.Type.Equals(ClaimTypes.NameIdentifier)).Value;

            User insertUser;
            userRepo.Setup( x => x.insertAsync(It.IsAny<User>()).Result)
                                  .Callback<User>( x => insertUser = x )
                                  .Returns<User>( x => x );

            var testUser = await testSvc.findOrAddUserAsync(testPrincipal);

            userRepo.Verify( x => x.insertAsync(It.IsAny<User>()));

            testUser.IdpId.Should().Be("testIdpUserName");
            testUser.Email.Should().Be("test@test.net");
            testUser.EmailVerified.Should().Be(true);
            testUser.Name.Should().Be("First Last");
        }

    }
}