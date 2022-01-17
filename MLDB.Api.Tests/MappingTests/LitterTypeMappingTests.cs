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
    public class LitterTypeMappingTests {

        private IMapper mapper;
        private Fixture fixture;

        [SetUp]
        public void SetUp() {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new LitterTypeProfile()));
            mapper = configuration.CreateMapper();

            fixture = new Fixture();
        }

        [Test]
        public void litterTypeMapping_ShouldMapExpectedFields() {
            var testLT = fixture.Build<LitterType>()
                                .Create();

            // Perform mapping
            var litterTypeDTO  = mapper.Map<LitterTypeDTO>(testLT);

            litterTypeDTO.Id.Should().Be(testLT.Id);
            litterTypeDTO.OsparId.Should().Be(testLT.OsparId);
            litterTypeDTO.OsparCategory.Should().Be(testLT.OsparCategory);
            litterTypeDTO.Description.Should().Be(testLT.Description);
            litterTypeDTO.JointListTypeCode.Should().Be(testLT.JointListTypeCode);
            litterTypeDTO.JointListTypeCode.Should().Be(testLT.JointListTypeCode);

            litterTypeDTO.SourceCategory.Should().Be(testLT.SourceCategory.Name);
        }
    }
}