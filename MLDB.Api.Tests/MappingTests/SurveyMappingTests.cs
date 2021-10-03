using System;
using AutoMapper;
using NUnit.Framework;
using MLDB.Api.DTO;
using MLDB.Api.Models;
using FluentAssertions;
using System.Collections.Generic;

namespace MLDB.Api.Tests.MappingTests {
    public class SurveyMappingTests {

        private IMapper mapper = null;
        
        private Survey testSurvey = null;
        
        private SurveyDTO testDTO = new SurveyDTO() { 
            Id = Guid.NewGuid(),
            SurveyDate = "1969-04-20",
            StartTime = "16:20:09",
            EndTime = "18:09:42",
            Coordinator = "Test Coordinator",
            VolunteerCount = 17,
            TotalKg = 21.12m
        };

        [SetUp]
        public void setup() { 
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MLDB.Api.Mapping.SurveyProfile()));
            configuration.AssertConfigurationIsValid();
            mapper = configuration.CreateMapper();

            testSurvey = new Survey();
            testSurvey.Id = Guid.NewGuid();
            testSurvey.StartTimeStamp = new DateTime(1969,4,20,16,20,09);
            testSurvey.EndTimeStamp = new DateTime(1969,4,20,18,09,42);
            testSurvey.Coordinator = "Test Coordinator";
            testSurvey.VolunteerCount = 17;
            testSurvey.TotalKg = 21.12m;
        }

        [Test]
        public void surveyMapping_ToDTO_ShouldMapExpectedFields() {

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

            var mappedSurvey = mapper.Map<Survey>(testDTO);

            mappedSurvey.Id.Should().Be(testDTO.Id);
            mappedSurvey.StartTimeStamp.Should().Be(new DateTime(1969,4,20,16,20,09));
            mappedSurvey.EndTimeStamp.Should().Be(new DateTime(1969,4,20,18,09,42));            
            mappedSurvey.Coordinator.Should().Be(testDTO.Coordinator);
            mappedSurvey.VolunteerCount.Should().Be(testDTO.VolunteerCount);
            mappedSurvey.TotalKg.Should().Be(testDTO.TotalKg);
        }

        [Test]
        public void surveyMapping_FromDTO_ShouldMapLitterItems() {
            var dtoWithItems = testDTO with {
                LitterItems = new Dictionary<String, int>() { 
                    ["42"] = 1,
                    ["43"] = 3
                }
            };

            var mappedSurvey = mapper.Map<Survey>(dtoWithItems);

            mappedSurvey.LitterItems.Should().HaveCount(2);
            mappedSurvey.LitterItems.Should().ContainEquivalentOf( new LitterItem() { SurveyId = testDTO.Id, LitterTypeId = 42, Count = 1} );
            mappedSurvey.LitterItems.Should().ContainEquivalentOf( new LitterItem() { SurveyId = testDTO.Id, LitterTypeId = 43, Count = 3} );
        }

        [Test]
        public void surveyMapping_FromDTO_SetsDefaultStartTime_IfNotSet() {
            var dtoWithNoStartTime = testDTO with {
                StartTime = null
            }; 

            var mappedSurvey = mapper.Map<Survey>(dtoWithNoStartTime);

            mappedSurvey.StartTimeStamp.Should().Be(new DateTime(1969,4,20,00,00,00));
        }

        [Test]
        public void surveyMapping_FromDTO_SetsDefaultEndTime_IfNotSet() {
            var dtoWithNoEndTime = testDTO with {
                EndTime = null
            }; 

            var mappedSurvey = mapper.Map<Survey>(dtoWithNoEndTime);

            mappedSurvey.EndTimeStamp.Should().Be(new DateTime(1969,4,20,00,00,00));
        }
    }
}