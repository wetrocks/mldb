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
using MLDB.Api.DTO;
using System.Net.Http.Json;


namespace MLDB.Api.IntegrationTests
{
    [TestOf(typeof(MLDB.Api.Controllers.SiteController))]
    public class SiteApiTests
    {
        APITestWebApplicationFactory factory;
        HttpClient client;
        SiteSurveyContext dbCtx;
        Fixture fixture;

        [SetUp]
        public void Setup()
        {
            factory = new APITestWebApplicationFactory();
            client  = factory.CreateClient();

            var scope = (factory.Services.GetRequiredService<IServiceScopeFactory>()).CreateScope();
            dbCtx = scope.ServiceProvider.GetRequiredService<SiteSurveyContext>();

            dbCtx.Database.EnsureCreated();

            fixture = new Fixture();
        }

        [Test]
        public async Task GetSites_DoesNotAllowUnauthorizedAccess()
        {
            var result = await client.GetAsync("/site");
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task GetSites_ReturnsAllSites()
        {
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

            var response = await client.GetAsync("/site");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await response.Content.ReadAsStringAsync();
            JToken.Parse(body).Should().ContainSubtree(String.Format("[{{ 'id' : '{0}' }}]", existingSite.Id));
        }

        [Test]
        public async Task GetSite_DoesNotAllowUnauthorizedAccess()
        {
            var result = await client.GetAsync($"/site/{Guid.NewGuid()}");
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task GetSite_WhenNotExists_Returns404()
        {
            dynamic data = new ExpandoObject();
            data.sub = "testuser";
            data.role = new [] {"sub_role","admin"};

            client.SetFakeBearerToken((object)data);

            var response = await client.GetAsync($"/site/{Guid.NewGuid()}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task GetSite_WhenExists_ReturnsOK()
        {
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

            var response = await client.GetAsync($"/site/{existingSite.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await response.Content.ReadAsStringAsync();
            JToken.Parse(body).Should().ContainSubtree(String.Format("{{ 'id' : '{0}' }}", existingSite.Id));
        }

        [Test]
        public async Task PostSite_DoesNotAllowUnauthorizedAccess()
        {
            var testDTO = fixture.Build<SiteDTO>().Create();
            
            var result = await client.PostAsJsonAsync($"/site", testDTO);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task PostSite_CreatesSiteAndReturnsJson()
        {
            var testDTO = fixture.Build<SiteDTO>().Create();
            
            dynamic data = new ExpandoObject();
            data.sub = "testuser";
            data.role = new [] {"sub_role","admin"};

            client.SetFakeBearerToken((object)data);

            var result = await client.PostAsJsonAsync($"/site", testDTO);

            // need this to check return json
            var createdSite = dbCtx.Sites.OrderByDescending( x => x.CreateTimestamp ).SingleOrDefault();

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var body = await result.Content.ReadAsStringAsync();
            JToken.Parse(body).Should().ContainSubtree(
                String.Format("{{ 'id' : '{0}', 'name' : '{1}' }}", createdSite.Id, testDTO.Name));
        }
    }
}