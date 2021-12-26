using System;
using MLDB.Infrastructure.Repositories;
using NUnit.Framework;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FluentAssertions;
using MLDB.Domain;
using AutoFixture;

namespace MLDB.Infrastructure.IntegrationTests
{
    [TestOf(typeof(LitterTypeRepository))]
    public class LitterTypeRepositoryTests
    {
        private SqliteConnection conn;
        private DbContextOptions<SiteSurveyContext> ctxOptions;
        private SiteSurveyContext testCtx;

        private ILitterTypeRepository testRepo;

        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();

            conn = new SqliteConnection("DataSource=:memory:");
            conn.Open();

            ctxOptions = new DbContextOptionsBuilder<SiteSurveyContext>()
                        //  .LogTo( Console.WriteLine)
                        // .EnableSensitiveDataLogging()
                        .UseSqlite(conn)
                        .Options;
            testCtx = new SiteSurveyContext(ctxOptions);
            testCtx.Database.EnsureCreated();

            testRepo = new LitterTypeRepository(testCtx);

        }

        [TearDown]
        public void TearDown()
        {
            testCtx.Dispose();
            conn.Close();
        }


        [Test]
        public async Task getAll_ReturnsAll()
        {
            var allTypes = await testRepo.getAll();

            // TODO: get values from reference data json
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

            // TODO: get values from reference data json
            litterType.Id.Should().Be(1);
            litterType.Description.Should().Be("Bags");
            litterType.SourceCategory.Should().BeEquivalentTo(new LitterSourceCategory(1, "SUP", "Stand up paddleboards"));
            litterType.OsparId.Should().Be(42);
            litterType.OsparCategory.Should().Be("Plastic");
        }
    }
}