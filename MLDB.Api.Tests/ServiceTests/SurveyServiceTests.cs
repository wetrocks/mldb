using System.Security.Claims;
using Moq;
using MLDB.Api.DTO;
using MLDB.Api.Services;
using MLDB.Domain;
using AutoFixture;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;


namespace MLDB.Api.Tests.ServiceTests {

    public class SurveyServiceTests {

        private readonly ClaimsPrincipal TEST_USER_PRINCIPAL = 
            new ClaimsPrincipal(
                new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.NameIdentifier, "testIdpUserName") },"TEST_AUTH")
            );

        private ISurveyService testSvc;

        private Mock<IUserService> userSvc;

        private Mock<ISiteRepository> siteRepo;

        private Mock<ISurveyRepository> surveyRepo;

        private Fixture fixture;

        private User testUser;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();

            userSvc = new Mock<IUserService>();
            siteRepo = new Mock<ISiteRepository>();
            surveyRepo = new Mock<ISurveyRepository>();
            testSvc = new SurveyService(userSvc.Object, siteRepo.Object, surveyRepo.Object, new Mock<ILogger<SurveyService>>().Object);

            testUser = fixture.Build<User>().Create();
            userSvc.Setup( x => x.findOrAddUserAsync(It.IsAny<ClaimsPrincipal>()))
                   .ReturnsAsync(testUser);
        }

        private SurveyDTO createTestSurveyDTO(Fixture fixture) {
            //TODO: there is definitely a nicer way to do everything in this method
            var litterItems = fixture.CreateMany<KeyValuePair<int, int>>();

            var temp =  fixture.Build<SurveyDTO>()
                                .Without( x => x.Id )
                                .With( x => x.SurveyDate, "1970-04-20")
                                .With( x => x.StartTime, "16:20:09")
                                .With( x => x.EndTime, "18:09:42")
                                .With( x => x.LitterItems, litterItems.ToDictionary( l => l.Key.ToString(), l => l.Value))
                                .Create();

            return temp;
        }

        [Test]
        public async Task createSiteSurvey_CreatesUser() {
            var testDTO = this.createTestSurveyDTO(fixture);
            
            await testSvc.createSiteSurvey(TEST_USER_PRINCIPAL, fixture.Create<Guid>(), testDTO);

            userSvc.Verify( x => x.findOrAddUserAsync(TEST_USER_PRINCIPAL));
        }

        [Test]
        public async Task createSiteSurvey_insertsSurveyData() {
            var siteId = fixture.Create<Guid>();
            var testDTO = this.createTestSurveyDTO(fixture);

            Survey insertedSurvey = null;
            surveyRepo.Setup( x => x.insertAsync(It.IsAny<Survey>()))
                    .Callback<Survey>( s => insertedSurvey = s);
            
            await testSvc.createSiteSurvey(TEST_USER_PRINCIPAL, siteId, testDTO);

            surveyRepo.Verify( x => x.insertAsync(It.IsAny<Survey>()));
            insertedSurvey.Id.Should().BeEmpty();
            insertedSurvey.SiteId.Should().Be(siteId);
            insertedSurvey.StartTimeStamp.Should().Be(new DateTime(1970,4,20,16,20,09));
            insertedSurvey.EndTimeStamp.Should().Be(new DateTime(1970,4,20,18,09,42));            
            insertedSurvey.CoordinatorName.Should().Be(testDTO.Coordinator);
            insertedSurvey.VolunteerCount.Should().Be(testDTO.VolunteerCount);
            insertedSurvey.TotalKg.Should().Be(testDTO.TotalKg);
        }

        [Test]
        public async Task createSiteSurvey_insertsLitterItems() {
            var siteId = fixture.Create<Guid>();
            var testDTO = this.createTestSurveyDTO(fixture);

            Survey insertedSurvey = null;
            surveyRepo.Setup( x => x.insertAsync(It.IsAny<Survey>()))
                    .Callback<Survey>( s => insertedSurvey = s);

            await testSvc.createSiteSurvey(TEST_USER_PRINCIPAL, siteId, testDTO);

            insertedSurvey.LitterItems.Should().HaveSameCount(testDTO.LitterItems);
            foreach( LitterItem item in insertedSurvey.LitterItems ) {
                var dtoItemCount = testDTO.LitterItems[item.LitterTypeId.ToString()];
                item.Count.Should().Be(dtoItemCount);
            }
        }
        
        [Test]
        public async Task updateSiteSurvey_CreatesUser() {
            var testDTO = this.createTestSurveyDTO(fixture);
            
            await testSvc.updateSiteSurvey(TEST_USER_PRINCIPAL, testDTO);

            userSvc.Verify( x => x.findOrAddUserAsync(TEST_USER_PRINCIPAL));
        }

        [Test]
        public async Task updateSiteSurvey_updatesSurveyData() {
            var siteId = fixture.Create<Guid>();
            var surveyId = fixture.Create<Guid>();
            var testDTO = this.createTestSurveyDTO(fixture);
            testDTO = testDTO with { Id = surveyId, SiteId = siteId };

            Survey updatedSurvey = null;
            surveyRepo.Setup( x => x.updateAsync(It.IsAny<Survey>()))
                    .Callback<Survey>( s => updatedSurvey = s);
            
            await testSvc.updateSiteSurvey(TEST_USER_PRINCIPAL, testDTO);

            surveyRepo.Verify( x => x.updateAsync(It.IsAny<Survey>()));
            updatedSurvey.Id.Should().Be(surveyId);
            updatedSurvey.SiteId.Should().Be(siteId);
            updatedSurvey.StartTimeStamp.Should().Be(new DateTime(1970,4,20,16,20,09));
            updatedSurvey.EndTimeStamp.Should().Be(new DateTime(1970,4,20,18,09,42));            
            updatedSurvey.CoordinatorName.Should().Be(testDTO.Coordinator);
            updatedSurvey.VolunteerCount.Should().Be(testDTO.VolunteerCount);
            updatedSurvey.TotalKg.Should().Be(testDTO.TotalKg);
        }

        [Test]
        public async Task updateSiteSurvey_insertsLitterItems() {
            var siteId = fixture.Create<Guid>();
            var surveyId = fixture.Create<Guid>();
            var testDTO = this.createTestSurveyDTO(fixture);
            testDTO = testDTO with { Id = surveyId, SiteId = siteId };

            Survey updatedSurvey = null;
            surveyRepo.Setup( x => x.updateAsync(It.IsAny<Survey>()))
                    .Callback<Survey>( s => updatedSurvey = s);

            await testSvc.updateSiteSurvey(TEST_USER_PRINCIPAL, testDTO);

            updatedSurvey.LitterItems.Should().HaveSameCount(testDTO.LitterItems);
            foreach( LitterItem item in updatedSurvey.LitterItems ) {
                var dtoItemCount = testDTO.LitterItems[item.LitterTypeId.ToString()];
                item.Count.Should().Be(dtoItemCount);
            }
        }
    }
}