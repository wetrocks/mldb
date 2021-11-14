using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Dynamic;
using System;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Json;
using AutoFixture;
using Newtonsoft.Json.Linq;
using MLDB.Api.DTO;
using System.Net.Http.Json;
using MLDB.Infrastructure.Repositories;
using MLDB.Domain;


namespace MLDB.Api.IntegrationTests
{
    [TestOf(typeof(MLDB.Api.Controllers.SiteController))]
    public class LitterTypeApiTests
    {
        APITestWebApplicationFactory factory;
        HttpClient client;

        SiteSurveyContext dbCtx;
        Fixture fixture;

        dynamic testToken;

        [SetUp]
        public void Setup()
        {
            factory = new APITestWebApplicationFactory();
            client  = factory.CreateClient();

            var scope = (factory.Services.GetRequiredService<IServiceScopeFactory>()).CreateScope();
            dbCtx = scope.ServiceProvider.GetRequiredService<SiteSurveyContext>();
            dbCtx.Database.EnsureCreated();

            fixture = new Fixture();

            testToken = new ExpandoObject();
            testToken.sub = "testuser";
        }

        [Test]
        public async Task GetLitterTypes_DoesNotAllowUnauthorizedAccess()
        {
            var result = await client.GetAsync("/litterType");
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task GetLitterTypes_ReturnsLitterTypes()
        {
            var litterType = dbCtx.LitterTypes.FirstOrDefault();

            client.SetFakeBearerToken((object)testToken);

            var response = await client.GetAsync("/litterType");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await response.Content.ReadAsStringAsync();
            JToken.Parse(body).Should().ContainSubtree(String.Format("[{{ 'id' : {0} }}]", litterType.Id));
        }
    }
}