using Microsoft.EntityFrameworkCore;
using MLDB.Domain;
using MLDB.Infrastructure.Repositories.Configuration;

namespace MLDB.Infrastructure.Repositories
{
    public class SiteSurveyContext : DbContext
    {
        public SiteSurveyContext(DbContextOptions<SiteSurveyContext> options)
            : base(options)
        {
        }

        public DbSet<Survey> Surveys { get; set; }

        // public DbSet<User> Users { get;  set; }

        public DbSet<SurveyTemplate> SurveyTemplates{ get; set; }

        public DbSet<Site> Sites { get; set; }

        public DbSet<LitterType> LitterTypes { get;  set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SurveyTemplate>()
                        .HasMany( st => st.LitterTypes )
                        .WithMany("Surveys");
                        
            modelBuilder.Entity<Survey>().OwnsMany(
                s => s.LitterItems, li => 
                {
                    li.WithOwner().HasForeignKey("SurveyId");
                    li.HasKey("SurveyId", "LitterTypeId");
                });

            modelBuilder.Entity<Survey>()
                        .HasOne<Site>()
                        .WithMany()
                        .HasForeignKey( x => x.SiteId );

            // read lists of litter types
            var litterTypesLists = DataLoad.ReferenceDataLoader.readLitterTypes();
            
            var litterTypeConfigs =  new LitterTypeEntityConfigurations(litterTypesLists);
            litterTypeConfigs.Configure(modelBuilder.Entity<LitterType>());
            litterTypeConfigs.Configure(modelBuilder.Entity<DataLoad.LitterTypesList>());
        }
    }
}