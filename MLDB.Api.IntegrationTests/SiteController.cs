using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MLDB.Api.IntegrationTests
{
    public class SiteController
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task DoesNotAllowUnauthorizedAccess()
        {
            var factory = new APITestWebApplicationFactory();
            var client  = factory.CreateClient();


            var result = await client.GetAsync("/api/site/1");
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}