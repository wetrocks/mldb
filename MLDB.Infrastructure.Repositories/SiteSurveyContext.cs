using Microsoft.EntityFrameworkCore;
using MLDB.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using System.Linq;
using System;

namespace MLDB.Infrastructure.Repositories
{
    public class SiteSurveyContext : DbContext
    {
        public SiteSurveyContext(DbContextOptions<SiteSurveyContext> options)
            : base(options)
        {
        }

        // public DbSet<Site> Sites { get; set; }

        // public DbSet<Survey> Surveys { get; set; }

        // public DbSet<User> Users { get;  set; }

        public DbSet<SurveyTemplate> SurveyTemplates{ get; set; }

        // public DbSet<LitterType> LitterTypes { get;  set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SurveyTemplate>()
                        .HasMany( st => st.LitterTypes )
                        .WithMany("Surveys");
                        
        //     modelBuilder.Entity<Survey>().OwnsMany(
        //         s => s.LitterItems, li => 
        //         {
        //             li.WithOwner().HasForeignKey("SurveyId");
        //             li.HasKey("SurveyId", "LitterTypeId");
        //         });

        //     this.PopulateSurveyTemplates(modelBuilder);
        // }
        // private void PopulateSurveyTemplates(ModelBuilder builder)  
        // {  
        //     var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
        //     using (var fileStream = embeddedProvider.GetFileInfo("seedData/seedLitterTypes.json").CreateReadStream())
        //     {
        //         var templates = ReadSurveyTemplates(fileStream);
     
        //         builder.Entity<SurveyTemplate>()
        //             .HasData(templates.Select( x => new SurveyTemplate { Id = x.Id, Description = x.Description}));

        //         var allTypes = templates.Aggregate(new List<LitterType>(),
        //                 (typeList,template) => typeList.Union(template.LitterTypes).ToList());

        //         builder.Entity<LitterType>().HasData(allTypes);

        //         // this seem weird but its apparently how its done
        //         var templateTypes = templates.Aggregate(new List<(string,int)>(), 
        //                 (relList, template) => {
        //                     var relationships = template.LitterTypes.Select( x => (template.Id, x.Id ));

        //                     return relList.Concat(relationships).ToList();
        //                 });
        //         builder.Entity<SurveyTemplate>()
        //             .HasMany( x => x.LitterTypes)
        //             .WithMany( y => y.SurveyTemplates)
        //             .UsingEntity( j => j.HasData(templateTypes.Select( x => 
        //                 new { SurveyTemplatesId = x.Item1, LitterTypesId = x.Item2} )));
        //     }
        } 

        // public List<SurveyTemplate> ReadSurveyTemplates(Stream jsonStream) {
        //     var templates = new List<SurveyTemplate>();
        //     using (StreamReader r = new StreamReader(jsonStream))
        //     {
        //         var json = r.ReadToEnd();
        //         templates = JsonConvert.DeserializeObject<List<SurveyTemplate>>(json);
        //     }

        //     return templates;
        // }
    }
}