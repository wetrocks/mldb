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
    [TestOf(typeof(SurveyTemplateRepository))]
    public class SurveyTemplateRepositoryTests
    {
        private SqliteConnection conn;
        private DbContextOptions<SiteSurveyContext> ctxOptions;
        private SiteSurveyContext testCtx;

        private ISurveyTemplateRepository testRepo; 

        private Fixture fixture;

        [SetUp]
        public void Setup()
        {
            conn = new SqliteConnection("DataSource=:memory:");
            conn.Open(); 
            
            ctxOptions = new DbContextOptionsBuilder<SiteSurveyContext>()
                       //  .LogTo( Console.WriteLine)
                        // .EnableSensitiveDataLogging()
                        .UseSqlite(conn)
                        .Options;
            testCtx = new SiteSurveyContext(ctxOptions);
            testCtx.Database.EnsureCreated();

            // seedTestData();

            testRepo = new SurveyTemplateRepository(testCtx);

            fixture = new AutoFixture.Fixture();
            
            // // make user "exist"
            // testCtx.Attach(seedUser);
        }

        [TearDown]
        public void TearDown()
        {
            testCtx.Dispose();
            conn.Close();
        }

        [Test]
        public async Task getTemplate_whenNotExists_ReturnsNull() {
            var testTemplate = await testRepo.getTemplateAsync(fixture.Create<string>());

            testTemplate.Should().BeNull();
        }
    }
}