using NUnit.Framework;
using MLDB.Domain;
using FluentAssertions;
using System;
using AutoFixture;
using System.Collections.Generic;
using System.Linq;

namespace MLDB.Domain.Tests
{
    [TestOf(typeof(Site))]
    public class SiteTests
    {
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new AutoFixture.Fixture();
        }

        [Test]
        public void CreateSite_setsCreateTimestamp()
        {
            var testSite = new Site(fixture.Create<string>(), fixture.Create<string>());  

            testSite.CreateTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Test]
        public void CreateTS_onlyAllowsUTC()
        {
            Assert.Throws<ArgumentException>( () => { 
                var testSite = new Site(fixture.Create<string>(), fixture.Create<string>()) { CreateTimestamp = DateTime.Now };
            });
        }
    }
}