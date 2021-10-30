using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

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

            this.SeedLitterTypes(modelBuilder);
        }
        private void SeedLitterTypes(ModelBuilder builder)  
        {  
            var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
            using (var fileStream = embeddedProvider.GetFileInfo("seedData/seedLitterTypes.json").CreateReadStream())
            {
                var litterTypes = ReadLitterTypes(fileStream);

                builder.Entity<LitterType>().HasData(litterTypes);
            }
        } 

        public List<LitterType> ReadLitterTypes(Stream jsonStream) {
            var litterTypes = new List<LitterType>();
            using (StreamReader r = new StreamReader(jsonStream))
            {
                var json = r.ReadToEnd();
                litterTypes = JsonConvert.DeserializeObject<List<LitterType>>(json);
            }

            return litterTypes;
        }
    }
}