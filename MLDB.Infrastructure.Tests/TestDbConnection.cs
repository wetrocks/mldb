namespace MLDB.Infrastructure.IntegrationTests;

using MLDB.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Npgsql;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Configurations; 
using System;

internal class TestDbConnection {

    internal static async Task<DbContextOptions<SiteSurveyContext>> GetDbContextOptions( 
        Action<string> logger = null,
        bool logSensitiveData = false)
    {
        TestcontainerDatabase dbContainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithImage("postgres:14.3-alpine")
            .WithDatabase(new PostgreSqlTestcontainerConfiguration {
                Database = "db",
                Username = "postgres",
                Password = "password",
                })
            .Build();

        await dbContainer.StartAsync();
        
        var builder =  new DbContextOptionsBuilder<SiteSurveyContext>()
                            .UseNpgsql(new NpgsqlConnection(dbContainer.ConnectionString));
        if( logger != null ) {
            builder.LogTo(logger).EnableSensitiveDataLogging(logSensitiveData);
        }
        return builder.Options;
    }
}