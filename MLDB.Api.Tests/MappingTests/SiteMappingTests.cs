using System;
using AutoMapper;
using NUnit.Framework;
using MLDB.Api.DTO;
using MLDB.Api.Mapping;
using FluentAssertions;
using AutoFixture;
using MLDB.Domain;
using System.Collections.Generic;

namespace MLDB.Api.Tests.MappingTests {
    public class SiteMappingTests {

        private IMapper mapper;
        private Fixture fixture;

        [SetUp]
        public void SetUp() {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new SiteProfile()));
            mapper = configuration.CreateMapper();

            fixture = new Fixture();
        }

        [Test]
        public void siteMapping_ToDTO_ShouldMapExpectedFields() {
            var testSite = fixture.Build<Site>()
                                .With( x => x.CreateTimestamp, DateTime.UtcNow )
                                .Create();

            // Perform mapping
            var siteDTO  = mapper.Map<SiteDTO>(testSite);

            siteDTO.Id.Should().Be(testSite.Id);
            siteDTO.Name.Should().Be(testSite.Name);
            siteDTO.CreatedBy.Should().Be(testSite.CreateUserId);
        }

        [Test]
        public void siteMapping_FromDTO_ShouldMapExpectedFields()  {
            var testDTO = fixture.Build<SiteDTO>().Create();

            var testSite = mapper.Map<Site>(testDTO);

            testSite.Id.Should().Be(testDTO.Id);
            testSite.Name.Should().Be(testDTO.Name);
            testSite.CreateUserId.Should().Be(testDTO.CreatedBy);
        }
    }
}