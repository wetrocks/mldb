using NUnit.Framework;
using MLDB.Domain;
using FluentAssertions;
using System;
using AutoFixture;
using System.Collections.Generic;

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
                var Survey = new Survey(fixture.Create<Guid>(), null, fixture.Create<string>());  
            });
        }

        [Test]
        public void CreateSurvey_WhenTemplateHasNoLitterTypes_throwsException()
        {
            var emptyTemplate = new SurveyTemplate( fixture.Create<string>(), new List<LitterType>() );
            
            Assert.Throws<ArgumentException>( () => 
            { 
                var Survey = new Survey(fixture.Create<Guid>(), emptyTemplate, fixture.Create<string>());  
            });
        }

        [Test]
        public void CreateSurvey_setsCreateTimestamp()
        {
            var testTemplate = fixture.Build<SurveyTemplate>().Create();

            var testSurvey = new Survey(fixture.Create<Guid>(), testTemplate, fixture.Create<string>());  
        }

        [Test]
        public void CreateSurvey_InitializesLitterItems()
        {
            var testTemplate = fixture.Build<SurveyTemplate>().Create();

            var testSurvey = new Survey(fixture.Create<Guid>(), testTemplate, fixture.Create<string>()); 

            testSurvey.LitterItems.Should().HaveCount( testTemplate.LitterTypes.Count);
        }
    }
}