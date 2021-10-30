using System;
using AutoMapper;
using NUnit.Framework;
using MLDB.Api.DTO;
using MLDB.Api.Models;
using FluentAssertions;
using AutoFixture;
using MLDB.Api.Mapping;
using System.Collections.Generic;

namespace MLDB.Api.Tests.MappingTests {
    public class SiteMappingTests {

        private IMapper mapper;
        private Fixture fixture;

        [SetUp]
        public void SetUp() {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new SiteProfile()));
            mapper = configuration.CreateMapper();

            fixture = new Fixture();
        }

        [Test]
        public void siteMapping_ToDTO_ShouldMapExpectedFields() {
            var testSite = fixture.Build<Site>()
                                .With( x => x.CreateUser )
                                .Without( x => x.Surveys )
                                .Create();

            // Perform mapping
            var siteDTO  = mapper.Map<SiteDTO>(testSite);

            siteDTO.Id.Should().Be(testSite.Id);
            siteDTO.Name.Should().Be(testSite.Name);
            siteDTO.CreatedBy.Should().Be(testSite.CreateUser.Name);
        }

        [Test]
        public void siteMapping_ToDTO_ShouldIncludeSurveyInfo() {

            var testSurvey = fixture.Build<Survey>()
                                    .Without( x => x.LitterItems )
                                    .With( x => x.StartTimeStamp, new DateTime(1969,4,20,16,20,09))
                                    .Create();

            var testSite = fixture.Build<Site>()
                                .With( x => x.CreateUser )
                                .With( x => x.Surveys, new List<Survey>() { testSurvey })
                                .Create();

            // Perform mapping
            var siteDTO  = mapper.Map<SiteDTO>(testSite);

            siteDTO.Surveys.Should().HaveCount(1);
            siteDTO.Surveys[0].Id.Should().Be(testSurvey.Id);
            siteDTO.Surveys[0].SurveyDate.Should().Be("1969-04-20");
            siteDTO.Surveys[0].Coordinator.Should().Be(testSurvey.Coordinator);
            siteDTO.Surveys[0].VolunteerCount.Should().Be(testSurvey.VolunteerCount);
            siteDTO.Surveys[0].TotalKg.Should().Be(testSurvey.TotalKg);
            siteDTO.Surveys[0].LitterItems.Should().BeNullOrEmpty();
        }

        [Test]
        public void siteMapping_FromDTO_ShouldMapExpectedFields()  {
            var testDTO = fixture.Build<SiteDTO>().Create();

            var testSite = mapper.Map<Site>(testDTO);

            testSite.Id.Should().Be(testDTO.Id);
            testSite.Name.Should().Be(testDTO.Name);
            testSite.Surveys.Should().BeNullOrEmpty();
        }
    }
}