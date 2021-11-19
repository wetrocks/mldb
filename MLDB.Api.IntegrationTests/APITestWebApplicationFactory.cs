using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
using WebMotions.Fake.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using MLDB.Infrastructure.Repositories;

namespace MLDB.Api.IntegrationTests
{
    public class APITestWebApplicationFactory : WebApplicationFactory<MLDB.Api.Startup>
    {
        private SqliteConnection _conn;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            _conn = new SqliteConnection("DataSource=:memory:");
            _conn.Open();

            builder.UseTestServer()
                .ConfigureTestServices(services =>
                {
                    // remove the existing context configuration
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SiteSurveyContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    services.AddDbContext<SiteSurveyContext>(opt => {
                            opt.UseSqlite(_conn);
                    });
                   
                    services.AddAuthentication(options => 
                    {   
                        options.DefaultAuthenticateScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = FakeJwtBearerDefaults.AuthenticationScheme;
                    }).AddFakeJwtBearer();
                });
        }
        
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _conn.Close();
        }
    }
}
