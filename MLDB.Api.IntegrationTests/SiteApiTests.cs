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
    public class SiteApiTests
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
        public async Task GetSites_DoesNotAllowUnauthorizedAccess()
        {
            var result = await client.GetAsync("/site");
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task GetSites_ReturnsAllSites()
        {
            var existingSite = fixture.Build<Site>()
                                    .Create();
            dbCtx.Sites.Add(existingSite);
            dbCtx.SaveChanges();

            client.SetFakeBearerToken((object)testToken);

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
            client.SetFakeBearerToken((object)testToken);

            var response = await client.GetAsync($"/site/{Guid.NewGuid()}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task GetSite_WhenExists_ReturnsOK()
        {
            var existingSite = fixture.Build<Site>()
                                    .Create();
            dbCtx.Sites.Add(existingSite);
            dbCtx.SaveChanges();
            dbCtx.ChangeTracker.Clear();

            client.SetFakeBearerToken((object)testToken);

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
            var testDTO = fixture.Build<SiteDTO>()
                                 .Without( x => x.Id ).Create();
            
            client.SetFakeBearerToken((object)testToken);

            var result = await client.PostAsJsonAsync($"/site", testDTO);

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            
            var body = await result.Content.ReadAsStringAsync();
            JToken.Parse(body).Should().ContainSubtree(
                String.Format("{{ 'name' : '{0}' }}", testDTO.Name));
        }

        [Test]
        public async Task PutSite_DoesNotAllowUnauthorizedAccess()
        {
            var testDTO = fixture.Build<SiteDTO>().Create();
            
            var result = await client.PutAsJsonAsync($"/site/{testDTO.Id}", testDTO);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task PutSite_WhenDoesNotExist_Returns404()
        {
            var testDTO = fixture.Build<SiteDTO>().Create();

            client.SetFakeBearerToken((object)testToken);
            
            var result = await client.PutAsJsonAsync($"/site/{testDTO.Id}", testDTO);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task PutSite_WhenDoesExists_UpdatesSite()
        {
            var testSite = fixture.Build<Site>()
                                .Create();
            dbCtx.Sites.Add(testSite);
            dbCtx.SaveChanges();

            client.SetFakeBearerToken((object)testToken);
            
            var newName = fixture.Create<string>();
            var testDTO = new SiteDTO(){ Id = testSite.Id, Name = newName };

            var result = await client.PutAsJsonAsync($"/site/{testDTO.Id}", testDTO);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            var updatedSite = dbCtx.Sites.Single( x => x.Id == testDTO.Id );
            updatedSite.Name.Should().Equals(newName);
        }
    }
}