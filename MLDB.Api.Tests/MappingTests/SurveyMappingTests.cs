using System;
using AutoMapper;
using NUnit.Framework;
using MLDB.Api.DTO;
using MLDB.Domain;
using FluentAssertions;
using System.Collections.Generic;
using AutoFixture;
using System.Linq;

namespace MLDB.Api.Tests.MappingTests {

    [TestOf(typeof(MLDB.Api.Mapping.SurveyProfile))]
    public class SurveyMappingTests {

        private IMapper mapper = null;
        
        private Survey testSurvey = null;

        private Fixture fixture = null;

        [SetUp]
        public void setup() { 
            fixture = new Fixture();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MLDB.Api.Mapping.SurveyProfile()));
            configuration.AssertConfigurationIsValid();
            mapper = configuration.CreateMapper();

            testSurvey = fixture.Build<Survey>()
                                .With( x => x.CreateTimestamp, DateTime.UtcNow )
                                .With( x => x.StartTimeStamp, new DateTime(1969,4,20,16,20,09, DateTimeKind.Utc))
                                .With( x => x.EndTimeStamp, new DateTime(1969,4,20,18,09,42, DateTimeKind.Utc))
                                .Create();
            testSurvey.updateLitterItems(fixture.CreateMany<LitterItem>());               
        }

        [Test]
        public void surveyMapping_ToDTO_ShouldMapBasicFields() {

            var surveyDTO  = mapper.Map<SurveyDTO>(testSurvey);

            surveyDTO.Id.Should().Be(testSurvey.Id);
            surveyDTO.SurveyDate.Should().Be("1969-04-20");
            surveyDTO.StartTime.Should().Be("16:20:09");
            surveyDTO.EndTime.Should().Be("18:09:42");
            surveyDTO.Coordinator.Should().Be(testSurvey.CoordinatorName);
            surveyDTO.VolunteerCount.Should().Be(testSurvey.VolunteerCount);
            surveyDTO.TotalKg.Should().Be(testSurvey.TotalKg);
        }

        [Test]
        public void surveyMapping_ToDTO_ShouldMapLitterItems() {
            Assume.That(testSurvey.LitterItems, Is.Not.Zero);

            var surveyDTO  = mapper.Map<SurveyDTO>(testSurvey);

            surveyDTO.LitterItems.Should().HaveSameCount(testSurvey.LitterItems);
            foreach( LitterItem item in testSurvey.LitterItems ) {
                var dtoItemCount = surveyDTO.LitterItems[item.LitterTypeId.ToString()];
                item.Count.Should().Be(dtoItemCount);
            }
        }

    }
}