using NUnit.Framework;
using MLDB.Domain;
using FluentAssertions;
using System;
using AutoFixture;
using System.Collections.Generic;
using System.Linq;

namespace MLDB.Domain.Tests
{
    [TestOf(typeof(Survey))]
    public class SurveyTests
    {
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new AutoFixture.Fixture();
        }

        private Survey newSurvey() {
            return fixture.Build<Survey>()
                        .With( x => x.CreateTimestamp, DateTime.UtcNow)
                        .With( x => x.StartTimeStamp, DateTime.UtcNow)
                        .With( x => x.EndTimeStamp, DateTime.UtcNow)
                        .Create();
        }

        [Test]
        public void CreateSurvey_setsCreateTimestamp()
        {
            var testSurvey = new Survey(fixture.Create<Guid>(), fixture.Create<string>());  

            testSurvey.CreateTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Test]
        public void updateLitterItems_UpdatesLitterItems()
        {
            var testSurvey = newSurvey();
            var testItems = fixture.CreateMany<LitterItem>().ToList();
            
            testSurvey.updateLitterItems(testItems);

            testSurvey.LitterItems.Should().Contain(testItems);
        }

        [Test]
        public void updateLitterItems_DoesNotAllowDuplicateTypes()
        {
            var testSurvey = newSurvey();
            var testItems = fixture.CreateMany<LitterItem>().ToList();
            
            testItems.Add(new LitterItem(testItems.First().LitterTypeId));

             Assert.Throws<ArgumentException>( () => { 
                testSurvey.updateLitterItems(testItems);
            });
        }

        [Test]
        public void CreateTS_onlyAllowsUTC()
        {
            Assert.Throws<ArgumentException>( () => { 
                var testSite = new Survey(fixture.Create<Guid>(), fixture.Create<string>()) { CreateTimestamp = DateTime.Now };
            });
        }

        [Test]
        public void StartTS_onlyAllowsUTC()
        {
            Assert.Throws<ArgumentException>( () => { 
                var testSite = new Survey(fixture.Create<Guid>(), fixture.Create<string>());
                testSite.StartTimeStamp = DateTime.Now;
            });
        }

        [Test]
        public void EndTS_onlyAllowsUTC()
        {
            Assert.Throws<ArgumentException>( () => { 
                var testSite = new Survey(fixture.Create<Guid>(), fixture.Create<string>());
                testSite.EndTimeStamp = DateTime.Now;
            });
        }
    }
}