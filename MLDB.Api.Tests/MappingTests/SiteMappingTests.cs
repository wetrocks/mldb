using System;
using AutoMapper;
using NUnit.Framework;
using MLDB.Api.DTO;
using MLDB.Api.Models;
using FluentAssertions;

namespace MLDB.Api.Tests.MappingTests {
    public class SiteMappingTests {

        [Test]
        public void siteMapping_ShouldMapExpectedFields() {
            
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MLDB.Api.Mapping.SiteProfile()));
            var mapper = configuration.CreateMapper();

            var testSite = new Site();
            testSite.Name = "foo";

            // Perform mapping
            var siteDTO  = mapper.Map<SiteDTO>(testSite);

            siteDTO.Name.Should().Be("foo");
        }

    }
}