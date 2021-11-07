using NUnit.Framework;
using MLDB.Domain;
using FluentAssertions;
using System;
using AutoFixture;
using System.Collections.Generic;
using System.Linq;

namespace MLDB.Domain.Tests
{
    public class Tests
    {
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new AutoFixture.Fixture();
        }

        [Test]
        public void CreateSurvey_WhenTemplateIsNull_throwsException()
        {
            Assert.Throws<ArgumentException>( () => 
            { 
                var Survey = new Survey(fixture.Create<Guid>(), (IList<int>)null, fixture.Create<string>());  
            });
        }

        [Test]
        public void CreateSurvey_WhenTemplateHasNoLitterTypes_throwsException()
        {            
            Assert.Throws<ArgumentException>( () => 
            { 
                var Survey = new Survey(fixture.Create<Guid>(), new List<int>(), fixture.Create<string>());  
            });
        }

        [Test]
        public void CreateSurvey_setsCreateTimestamp()
        {
            var testSurvey = new Survey(fixture.Create<Guid>(), fixture.CreateMany<int>().ToList(), fixture.Create<string>());  

            testSurvey.CreateTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Test]
        public void CreateSurvey_InitializesLitterItems()
        {
            var itemTypes = fixture.CreateMany<int>().ToList();
            var testSurvey = new Survey(fixture.Create<Guid>(), itemTypes, fixture.Create<string>()); 

            testSurvey.LitterItems.Should().HaveSameCount(itemTypes);
        }
    }
}