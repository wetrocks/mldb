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
            fixture = new Fixture();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MLDB.Api.Mapping.SurveyProfile()));
            configuration.AssertConfigurationIsValid();
            mapper = configuration.CreateMapper();

            testSurvey = new Survey(fixture.Create<Guid>(), fixture.Create<Guid>(), 
                                    new List<int>{ 42, 43 },
                                    fixture.Create<string>());
            testSurvey.StartTimeStamp = new DateTime(1969,4,20,16,20,09);
            testSurvey.EndTimeStamp = new DateTime(1969,4,20,18,09,42);
            testSurvey.CoordinatorName = "Test Coordinator";
            testSurvey.VolunteerCount = 17;
            testSurvey.TotalKg = 21.12m;
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

            testSurvey.updateLitterCount(42, 1);
            testSurvey.updateLitterCount(43, 3);

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
            mappedSurvey.CoordinatorName.Should().Be(testDTO.Coordinator);
            mappedSurvey.VolunteerCount.Should().Be(testDTO.VolunteerCount);
            mappedSurvey.TotalKg.Should().Be(testDTO.TotalKg);
        }

        [Ignore("need to fix litter items first")]
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
            mappedSurvey.LitterItems.Should().Contain( x => x.LitterTypeId == 42 && x.Count == 1);
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