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
using AutoFixture;

namespace MLDB.Api.Tests.ServiceTests
{
    [TestOf(typeof(SiteSurveyService))]
    public class SiteSurveyServiceTests
    {
        private ISiteSurveyService testSvc;

        private Mock<IUserService> userSvc;

        private Mock<ISurveyRepository> surveyRepo;

        private User testUser;

        private Site testSite;


        [SetUp]
        public void Setup()
        {
            surveyRepo = new Mock<ISurveyRepository>();

            testSvc = new SiteSurveyService(surveyRepo.Object);

            var fixture = new Fixture();
            testUser  = fixture.Build<User>().Create();

            testSite = fixture.Build<Site>()
                              .With( x => x.CreateUser, testUser )
                              .Without( x => x.Surveys )
                              .Create();
        }

        [Test]
        public async Task create_AddsNewSiteSurvey()
        {
            Survey inserted = null;
            surveyRepo.Setup( x => x.insertAsync(It.IsAny<Survey>()))
                    .Callback<Survey>( x => inserted = x );

            var fixture = new Fixture();
            var testSurvey = fixture.Build<Survey>()
                                    .Without( x => x.Id )
                                    .Create();

            var newSurvey = await testSvc.create(testSurvey, testSite.Id, testUser);

            surveyRepo.Verify( x => x.insertAsync(testSurvey) );
            inserted.SiteId.Should().Be(testSite.Id);
            inserted.CreateUser.Should().Be(testUser);
            inserted.CreateTimestamp.Should().BeCloseTo(System.DateTime.UtcNow, TimeSpan.FromSeconds(10));
        }

        [Test]
        public async Task update_UpdatesSurvey()
        {
            Survey updated = null;
            surveyRepo.Setup( x => x.updateAsync(It.IsAny<Survey>()))
                      .Callback<Survey>( x => updated = x );

            var fixture = new Fixture();
            var testSurvey = fixture.Build<Survey>()
                                    .Create();

            var updatedSurvey = await testSvc.update(testSurvey, testSite.Id, testUser);

            surveyRepo.Verify( x => x.updateAsync( It.IsAny<Survey>()) );

            updated.SiteId.Should().Be(testSite.Id);
            updated.Should().BeEquivalentTo(testSurvey, options => options.Excluding( x => x.SiteId));
        }
    }
}