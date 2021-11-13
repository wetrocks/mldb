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
    [TestOf(typeof(MLDB.Api.Controllers.SurveyController))]
    public class SurveyApiTests
    {
        APITestWebApplicationFactory factory;
        HttpClient client;

        SiteSurveyContext dbCtx;
        Fixture fixture;

        dynamic testToken;

        Site seedSite;

        Survey seedSurvey;

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

            seedTestData(dbCtx);
        }

        [TearDown]
        public void tearDown()
        {
            dbCtx.Sites.Remove(seedSite);
        }

        private void seedTestData(SiteSurveyContext ctx) {
            seedSite = fixture.Build<Site>()
                                  .Create();
            ctx.Sites.Add(seedSite);

            seedSurvey = new Survey(seedSite.Id, 
                                    fixture.CreateMany<int>().ToList(),
                                    fixture.Create<string>());
            dbCtx.Surveys.Add(seedSurvey);

            ctx.SaveChanges();
            ctx.ChangeTracker.Clear();
        }

        private string createSurveyUrl(Guid? surveyId = null) {
            if( surveyId is null ) {
                return $"/site/{seedSite.Id}/survey/";
            }
            else {
                return $"/site/{seedSite.Id}/survey/{surveyId}";
            }
        }

        [Test]
        public async Task GetSurvey_DoesNotAllowUnauthorizedAccess()
        {
            var badId = fixture.Create<Guid>();
            
            var result = await client.GetAsync(createSurveyUrl(badId));
            
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task GetSurvey_WhenNotExists_Returns404()
        {
            client.SetFakeBearerToken((object)testToken);

            var badId = fixture.Create<Guid>();

            var response = await client.GetAsync($"/site/{badId}/survey/{badId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task GetSurvey_WhenExists_ReturnsOK()
        {
            client.SetFakeBearerToken((object)testToken);

            var response = await client.GetAsync(createSurveyUrl(seedSurvey.Id));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await response.Content.ReadAsStringAsync();
            JToken.Parse(body).Should().ContainSubtree(String.Format("{{ 'id' : '{0}' }}", seedSurvey.Id));
        }

        [Test]
        public async Task GetSurvey_ReturnsAllSites()
        {
            client.SetFakeBearerToken((object)testToken);

            var response = await client.GetAsync(createSurveyUrl());
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var body = await response.Content.ReadAsStringAsync();
            JToken.Parse(body).Should().ContainSubtree(String.Format("[{{ 'id' : '{0}' }}]", seedSurvey.Id));
        }

        [Test]
        public async Task PostSurvey_DoesNotAllowUnauthorizedAccess()
        {
            var testDTO = fixture.Build<SurveyDTO>().Create();
            
            var result = await client.PostAsJsonAsync(createSurveyUrl(), testDTO);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task PostSurvey_CreatesSurveyAndReturnsJson()
        {
            var testDTO = fixture.Build<SurveyDTO>()
                                 .Without( x => x.Id )
                                 .Without( x => x.LitterItems)
                                 .With( x => x.SurveyDate, "1970-05-20")
                                 .With( x => x.StartTime, "16:20:09")
                                 .With( x => x.EndTime, "18:09:42")
                                 .Create();
            
            client.SetFakeBearerToken((object)testToken);

            var result = await client.PostAsJsonAsync(createSurveyUrl(), testDTO);

            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            
            var body = await result.Content.ReadAsStringAsync();
            JToken.Parse(body).Should().ContainSubtree(
                String.Format("{{ 'siteId' : '{0}', 'coordinator' : '{1}' }}", seedSite.Id, testDTO.Coordinator));
        }

        [Test]
        public async Task PutSurvey_DoesNotAllowUnauthorizedAccess()
        {
            var testDTO = fixture.Build<SurveyDTO>()
                                 .Without( x => x.LitterItems)
                                 .With( x => x.SurveyDate, "1970-05-20")
                                 .With( x => x.StartTime, "16:20:09")
                                 .With( x => x.EndTime, "18:09:42")
                                 .Create();
            
            var result = await client.PutAsJsonAsync(createSurveyUrl(testDTO.Id), testDTO);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public async Task PutSurvey_WhenDoesNotExist_Returns404()
        {
            var testDTO = fixture.Build<SurveyDTO>()
                                 .Without( x => x.LitterItems)
                                 .With( x => x.SurveyDate, "1970-05-20")
                                 .With( x => x.StartTime, "16:20:09")
                                 .With( x => x.EndTime, "18:09:42")
                                 .Create();

            client.SetFakeBearerToken((object)testToken);
            
            var result = await client.PutAsJsonAsync(createSurveyUrl(testDTO.Id), testDTO);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task PutSurvey_WhenDoesExists_UpdatesSurvey()
        {            
            var newName = fixture.Create<string>();
            var testDTO = fixture.Build<SurveyDTO>()
                                 .With(  x => x.Id, seedSurvey.Id)
                                 .With( x => x.SiteId, seedSite.Id)
                                 .Without( x => x.LitterItems)
                                 .With( x => x.Coordinator, newName)
                                 .With( x => x.SurveyDate, "1970-05-20")
                                 .With( x => x.StartTime, "16:20:09")
                                 .With( x => x.EndTime, "18:09:42")
                                 .Create();

            client.SetFakeBearerToken((object)testToken);
            var result = await client.PutAsJsonAsync(createSurveyUrl(testDTO.Id), testDTO);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

            var updatedSurvey = dbCtx.Surveys.Single( x => x.Id == testDTO.Id );
            updatedSurvey.CoordinatorName.Should().Equals(newName);
        }
    }
}