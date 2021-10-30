using NUnit.Framework;
using Moq;
using FluentAssertions;
using System.Security.Claims;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

using MLDB.Api.Services;
using MLDB.Api.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using AutoFixture;

namespace MLDB.Api.Tests.ServiceTests
{
    [TestOf(typeof(LitterTypeService))]
    public class LitterTypeServiceTests
    {
        private ILitterTypeService testSvc;

        private Mock<ILitterTypeRepository> testRepo;

        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            testRepo = new Mock<ILitterTypeRepository>();

            testSvc = new LitterTypeService(testRepo.Object);

            fixture = new Fixture();
        }

        [Test]
        public async Task getForVersion_WhenNotExist_ReturnsEmpty() {
            testRepo.Setup( x => x.getAllAsync() )
                    .ReturnsAsync((IList<LitterType>)null);
            
            var litterTypes = await testSvc.getLitterTypesForVersion(fixture.Create<Decimal>());

            litterTypes.Should().BeEmpty();
        }

    }
}