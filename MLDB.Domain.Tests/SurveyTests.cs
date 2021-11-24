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

        [Test]
        public void CreateSurvey_setsCreateTimestamp()
        {
            var testSurvey = new Survey(fixture.Create<Guid>(), fixture.Create<string>());  

            testSurvey.CreateTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }
    }
}