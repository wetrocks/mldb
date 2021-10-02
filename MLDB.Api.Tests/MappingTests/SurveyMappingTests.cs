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
        public void surveyMapping_ToDTO_ShouldMapExpectedFields() {
            
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
        public void surveyMapping_ToDTO_ShouldMapLitterItems() {

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


        [Test]
        public void surveyMapping_FromDTO_ShouldMapExpectedFields() {
            var surveyDTO = new SurveyDTO() { 
                Id = Guid.NewGuid(),
                SurveyDate = "1969-04-20",
                StartTime = "16:20:09",
                EndTime = "18:09:42",
                Coordinator = "Test Coordinator",
                VolunteerCount = 17,
                TotalKg = 21.12m
            };

            var testSurvey = mapper.Map<Survey>(surveyDTO);

            testSurvey.Id.Should().Be(surveyDTO.Id);
            testSurvey.StartTimeStamp.Should().Be(new DateTime(1969,4,20,16,20,09));
            testSurvey.EndTimeStamp.Should().Be(new DateTime(1969,4,20,18,09,42));            
            testSurvey.Coordinator.Should().Be(surveyDTO.Coordinator);
            testSurvey.VolunteerCount.Should().Be(surveyDTO.VolunteerCount);
            testSurvey.TotalKg.Should().Be(surveyDTO.TotalKg);
        }

        [Test]
        public void surveyMapping_FromDTO_ShouldMapLitterItems() {
            var surveyDTO = new SurveyDTO() { 
                SurveyDate = "1969-04-20",
                StartTime = "16:20:09",
                EndTime = "18:09:42",
                LitterItems = new Dictionary<String, int>() { 
                    ["42"] = 1,
                    ["43"] = 3
                }
            };

            var testSurvey = mapper.Map<Survey>(surveyDTO);

            testSurvey.LitterItems.Should().HaveCount(2);
            testSurvey.LitterItems.Should().ContainEquivalentOf( new LitterItem() { LitterTypeId = 42, Count = 1} );
            testSurvey.LitterItems.Should().ContainEquivalentOf( new LitterItem() { LitterTypeId = 43, Count = 3} );
        }
    }
}