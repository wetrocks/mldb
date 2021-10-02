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

        public DbSet<LitterType> LitterTypes { get;  set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Survey>().OwnsMany(
                s => s.LitterItems, li => 
                {
                    li.WithOwner().HasForeignKey("SurveyId");
                    li.HasKey("SurveyId", "LitterTypeId");
                });
        }
    }
}