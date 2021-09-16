using Microsoft.EntityFrameworkCore;

using MLDB.Api.Models;

namespace MLDB.Api.Models
{
    public class SiteSurveyContext : DbContext
    {
        public SiteSurveyContext(DbContextOptions<SiteSurveyContext> options)
            : base(options)
        {
        }

        public DbSet<Site> Sites { get; set; }

        public DbSet<Survey> Surveys { get; set; }

        public DbSet<User> Users { get;  set; }
    }
}