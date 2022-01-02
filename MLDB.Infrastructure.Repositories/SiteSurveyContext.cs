
using System;
using Microsoft.EntityFrameworkCore;
using MLDB.Domain;
using MLDB.Infrastructure.Repositories.Model;
using MLDB.Infrastructure.Repositories.DataLoad;


namespace MLDB.Infrastructure.Repositories
{
    public class SiteSurveyContext : DbContext
    {
        // TODO: probably inject a static instance
        private ISeedDataLoader<LitterSourceCategory> categoryLoader = new SourceCategoryDataLoader();
        private ISeedDataLoader<OsparLitterType> osparLoader = new OsparLitterTypeDataLoader();

        private ISeedDataLoader<JointListLitterType> jlistLoader = new JointListLitterTypeDataLoader();

        private ISeedDataLoader<LitterTypeMapping<UInt32, OsparLitterType>> osparMappingLoader = new OsparMappingLoader();

        private ISeedDataLoader<LitterTypeMapping<String, JointListLitterType>> jlistMappingLoader = new JointListMappingLoader();

        public SiteSurveyContext(DbContextOptions<SiteSurveyContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get;  set; }

        public DbSet<Survey> Surveys { get; set; }

        public DbSet<Site> Sites { get; set; }
        
        internal DbSet<InternalLitterType> InternalLitterTypes { get; set; }

        internal DbSet<OsparLitterType> OsparLitterTypes { get; set; }

        internal DbSet<JointListLitterType> JointListLitterTypes{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                        .HasIndex( x => x.IdpId )
                        .IsUnique();
            
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


            // configure litter type vocabs, etc
            this.CreateLitterTypesModel(modelBuilder);
        }

        private void CreateLitterTypesModel(ModelBuilder modelBuilder) {
            var categories = this.categoryLoader.readSeedData();

            // TODO: need to not create this here, and maybe make additional data a function param
            var internalLTLoader = new InternalLitterTypeDataLoader(categories);

            modelBuilder.Entity<LitterSourceCategory>()
                        .HasData(categories);


            modelBuilder.Entity<InternalLitterType>()
                        .HasKey( x => x.Code );

            modelBuilder.Entity<InternalLitterType>()
                        .HasOne( x => x.SourceCategory )
                        .WithMany()
                        .HasForeignKey( x => x.SourceCategoryId );

            modelBuilder.Entity<InternalLitterType>()
                        .HasData(internalLTLoader.readSeedData());
        
            modelBuilder.Entity<InternalLitterType>()
                        .HasOne( x => x.OsparMapping )
                        .WithOne()
                        .HasForeignKey<LitterTypeMapping<UInt32, OsparLitterType>>( x => x.InternalLitterTypeCode );

            modelBuilder.Entity<InternalLitterType>()
                        .HasOne( x => x.JointListMapping )
                        .WithOne()
                        .HasForeignKey<LitterTypeMapping<String, JointListLitterType>>( x => x.InternalLitterTypeCode );

            // OSPAR litter type
            modelBuilder.Entity<OsparLitterType>()
                        .HasKey( x => x.Code );
            modelBuilder.Entity<OsparLitterType>()
                        .HasData(osparLoader.readSeedData());

            // OSPAR type mapping
            modelBuilder.Entity<LitterTypeMapping<UInt32, OsparLitterType>>()
                        .HasData(osparMappingLoader.readSeedData());

            modelBuilder.Entity<LitterTypeMapping<UInt32, OsparLitterType>>()
                        .ToTable("LitterTypeMapping_Ospar")
                        .HasKey( m => m.InternalLitterTypeCode );
            
            modelBuilder.Entity<LitterTypeMapping<UInt32, OsparLitterType>>()
                        .HasOne( x => x.MappedType)
                        .WithMany()
                        .HasForeignKey( x => x.MappedTypeKey );

            // JList litter type
            modelBuilder.Entity<JointListLitterType>()
                        .HasKey( x => x.TypeCode );
            
            modelBuilder.Entity<JointListLitterType>()
                        .Property( x => x.J_Code )
                        .IsRequired();

            modelBuilder.Entity<JointListLitterType>()
                        .HasData(jlistLoader.readSeedData());

            // JList type mapping
            modelBuilder.Entity<LitterTypeMapping<String, JointListLitterType>>()
                        .ToTable("LitterTypeMapping_JointList")
                        .HasKey( m => m.InternalLitterTypeCode );

            modelBuilder.Entity<LitterTypeMapping<String, JointListLitterType>>()
                        .Property( x => x.MappedTypeKey)
                        .IsRequired();

            modelBuilder.Entity<LitterTypeMapping<String, JointListLitterType>>()
                        .HasOne( x => x.MappedType )
                        .WithMany()
                        .HasForeignKey( x => x.MappedTypeKey );

            modelBuilder.Entity<LitterTypeMapping<String, JointListLitterType>>()
                        .HasData(jlistMappingLoader.readSeedData());
        }
    }
}