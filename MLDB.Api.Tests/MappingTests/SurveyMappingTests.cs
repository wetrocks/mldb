using System;
using AutoMapper;
using NUnit.Framework;
using MLDB.Api.DTO;
using MLDB.Api.Models;
using FluentAssertions;
using System.Collections.Generic;

namespace MLDB.Api.Tests.MappingTests {
    public class SurveyMappingTests {

        IMapper mapper;

        [SetUp]
        public void setup() { 
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MLDB.Api.Mapping.SurveyProfile()));
            configuration.AssertConfigurationIsValid();
            mapper = configuration.CreateMapper();
        }

        [Test]
        public void surveyMapping_ShouldMapExpectedFields() {
            
            var testSurvey = new Survey();
            testSurvey.Id = Guid.NewGuid();
            testSurvey.StartTimeStamp = new DateTime(1969,4,20,16,20,09);
            testSurvey.EndTimeStamp = new DateTime(1969,4,20,18,09,42);
            testSurvey.Coordinator = "Test Coordinator";
            testSurvey.VolunteerCount = 17;
            testSurvey.TotalKg = 21.12m;

            var surveyDTO  = mapper.Map<SurveyDTO>(testSurvey);

            surveyDTO.Id.Should().Be(testSurvey.Id);
            surveyDTO.SurveyDate.Should().Be("1969-04-20");
            surveyDTO.StartTime.Should().Be("16:20:09");
            surveyDTO.EndTime.Should().Be("18:09:42");
            surveyDTO.Coordinator.Should().Be(testSurvey.Coordinator);
            surveyDTO.VolunteerCount.Should().Be(testSurvey.VolunteerCount);
            surveyDTO.TotalKg.Should().Be(testSurvey.TotalKg);
        }

        [Test]
        public void surveyMapping_MapsLitterItems() {

            var testSurvey = new Survey();
            testSurvey.Id = Guid.NewGuid();
            testSurvey.LitterItems = new List<LitterItem>() {
                new LitterItem() { LitterType = new LitterType() { Id = 42 }, Count = 1 },
                new LitterItem() { LitterType = new LitterType() { Id = 43 }, Count = 3 }
            };

            var surveyDTO  = mapper.Map<SurveyDTO>(testSurvey);

            surveyDTO.LitterItems.Should().HaveCount(2);
            surveyDTO.LitterItems.Should().Contain("42", 1);
            surveyDTO.LitterItems.Should().Contain("43", 3);
        }
    }
}