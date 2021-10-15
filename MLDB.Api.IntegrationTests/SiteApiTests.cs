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
using MLDB.Api.Models;
using FluentAssertions.Json;
using AutoFixture;
using Newtonsoft.Json.Linq;


namespace MLDB.Api.IntegrationTests
{
    public class SiteApiTests
    {
        APITestWebApplicationFactory factory;
        HttpClient client;
        SiteSurveyContext dbCtx;

        [SetUp]
        public void Setup()
        {
            factory = new APITestWebApplicationFactory();
            client  = factory.CreateClient();

            var scope = (factory.Services.GetRequiredService<IServiceScopeFactory>()).CreateScope();
            dbCtx = scope.ServiceProvider.GetRequiredService<SiteSurveyContext>();

            dbCtx.Database.EnsureCreated();
        }

        [Test]
        public async Task DoesNotAllowUnauthorizedAccess()
        {
            var result = await client.GetAsync("/api/site/1");
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task GetSites_ReturnsAllSites()
        {
            var fixture = new Fixture();
            var existingSite = fixture.Build<Site>()
                                    .Without( x => x.Surveys )
                                    .Create();
            dbCtx.Sites.Add(existingSite);
            dbCtx.SaveChanges();
            dbCtx.ChangeTracker.Clear();

            dynamic data = new ExpandoObject();
            data.sub = "testuser";
            data.role = new [] {"sub_role","admin"};

            client.SetFakeBearerToken((object)data);

            var response = await client.GetAsync("/api/site");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await response.Content.ReadAsStringAsync();
            JToken.Parse(body).Should().ContainSubtree(String.Format("[{{ 'id' : '{0}' }}]", existingSite.Id));
        }
    }
}