using System;
using MLDB.Infrastructure.Repositories;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FluentAssertions;
using MLDB.Domain;
using AutoFixture;
using Npgsql;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Configurations;

namespace MLDB.Infrastructure.IntegrationTests
{
    [TestOf(typeof(LitterTypeRepository))]
    public class LitterTypeRepositoryTests
    {
        private SiteSurveyContext testCtx;

        private ILitterTypeRepository testRepo;

        private Fixture fixture;

        [SetUp]
        public async Task Setup()
        {
           TestcontainerDatabase dbContainer =  new TestcontainersBuilder<PostgreSqlTestcontainer>()
                .WithDatabase(new PostgreSqlTestcontainerConfiguration {
                    Database = "db",
                    Username = "postgres",
                    Password = "password",
                 })
                .Build();

            await dbContainer.StartAsync();

            var ctxOptions = new DbContextOptionsBuilder<SiteSurveyContext>()
                        // .LogTo(Console.WriteLine)
                        //.EnableSensitiveDataLogging()                
                        .UseNpgsql(new NpgsqlConnection(dbContainer.ConnectionString))
                        .Options;
            testCtx = new SiteSurveyContext(ctxOptions);
            testCtx.Database.EnsureCreated();

            testRepo = new LitterTypeRepository(testCtx);

            fixture = new Fixture();
        }

        [TearDown]
        public void TearDown()
        {
            testCtx.Dispose();
        }

        [Test]
        public async Task getAll_ReturnsAll()
        {
            var allTypes = await testRepo.getAll();

            // TODO: get values from reference data json?
            allTypes.Should().HaveCount(4);
        }

        [Test]
        public async Task find_WhenNotExists_ReturnsNull()
        {
            var litterType = await testRepo.findAsync(fixture.Create<UInt32>());

            litterType.Should().BeNull();
        }

        [Test]
        public async Task find_WhenExists_ReturnsLitterType()
        {
            var litterType = await testRepo.findAsync(1);

            // TODO: get values from reference data json or preload?
            litterType.Id.Should().Be(1);
            litterType.Description.Should().Be("Bags");
            litterType.SourceCategory.Should().BeEquivalentTo(new LitterSourceCategory(1, "SUP", "Stand up paddleboards"));
            litterType.OsparId.Should().Be(42);
            litterType.OsparCategory.Should().Be("Plastic");
            litterType.JointListTypeCode.Should().Be("pl_nn_bag_cabg_");
            litterType.JointListJCode.Should().Be("J3");
        }
    }
}