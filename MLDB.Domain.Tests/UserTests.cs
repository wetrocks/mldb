using NUnit.Framework;
using MLDB.Domain;
using FluentAssertions;
using System;
using AutoFixture;
using System.Collections.Generic;
using System.Linq;

namespace MLDB.Domain.Tests
{
    [TestOf(typeof(User))]
    public class UserTests
    {
        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new AutoFixture.Fixture();
        }

        [Test]
        public void CreateUser_setsCreateTimestamp()
        {
            var testUser = new User(fixture.Create<string>());
            
            testUser.CreateTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Test]
        public void CreateUser_setsUpdatedTimestamp()
        {
            var testUser = new User(fixture.Create<string>());
            
            testUser.UpdateTimestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Test]
        public void SetCreateTS_onlyAllowsUTC()
        {
            Assert.Throws<ArgumentException>( () => { 
                var testUser = new User(fixture.Create<string>());
                testUser.CreateTimestamp = DateTime.Now;
            });
        }

        [Test]
        public void SetUpdateTS_onlyAllowsUTC()
        {
            Assert.Throws<ArgumentException>( () => { 
                var testUser = new User(fixture.Create<string>());
                testUser.UpdateTimestamp = DateTime.Now;
            });
        }
    }
}