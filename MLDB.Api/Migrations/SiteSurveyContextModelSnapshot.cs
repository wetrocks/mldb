﻿// <auto-generated />
using System;
using MLDB.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MLDB.Api.Migrations
{
    [DbContext(typeof(SiteSurveyContext))]
    partial class SiteSurveyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("MLDB.Domain.LitterSourceCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.None);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("LitterSourceCategory");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Stand up paddleboards",
                            Name = "SUP"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Text about fisheries",
                            Name = "Fisheries"
                        });
                });

            modelBuilder.Entity("MLDB.Domain.Site", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Sites");
                });

            modelBuilder.Entity("MLDB.Domain.Survey", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CoordinatorName")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("text");

                    b.Property<DateTime>("EndTimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("SiteId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("StartTimeStamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("TotalKg")
                        .HasColumnType("numeric");

                    b.Property<short>("VolunteerCount")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

                    b.ToTable("Surveys");
                });

            modelBuilder.Entity("MLDB.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.None);

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("IdpId")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateTimestamp")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("IdpId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MLDB.Infrastructure.Repositories.Model.InternalLitterType", b =>
                {
                    b.Property<long>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.None);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("SourceCategoryId")
                        .HasColumnType("integer");

                    b.HasKey("Code");

                    b.HasIndex("SourceCategoryId");

                    b.ToTable("InternalLitterTypes");

                    b.HasData(
                        new
                        {
                            Code = 1L,
                            Description = "Bags",
                            SourceCategoryId = 1
                        },
                        new
                        {
                            Code = 2L,
                            Description = "Caps/Lids",
                            SourceCategoryId = 1
                        },
                        new
                        {
                            Code = 3L,
                            Description = "Bottle",
                            SourceCategoryId = 1
                        },
                        new
                        {
                            Code = 4L,
                            Description = "Styrofoam",
                            SourceCategoryId = 2
                        });
                });

            modelBuilder.Entity("MLDB.Infrastructure.Repositories.Model.JointListLitterType", b =>
                {
                    b.Property<string>("TypeCode")
                        .HasColumnType("text");

                    b.Property<string>("Definition")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("J_Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("TypeCode");

                    b.ToTable("JointListLitterTypes");

                    b.HasData(
                        new
                        {
                            TypeCode = "pl_nn_bag_cabg_",
                            Definition = "Shopping bags are medium-sized bags, typically around 10-20 litres in volume (though much larger versions exist, especially for non-grocery shopping), that are used by shoppers to carry home their purchases. Shopping bags can be made with a variety of plastics.",
                            J_Code = "J3"
                        },
                        new
                        {
                            TypeCode = "another_one",
                            Definition = "Another JLIST description",
                            J_Code = "J42"
                        });
                });

            modelBuilder.Entity("MLDB.Infrastructure.Repositories.Model.LitterTypeMapping<string, MLDB.Infrastructure.Repositories.Model.JointListLitterType>", b =>
                {
                    b.Property<long>("InternalLitterTypeCode")
                        .HasColumnType("bigint");

                    b.Property<string>("MappedTypeKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("InternalLitterTypeCode");

                    b.HasIndex("MappedTypeKey");

                    b.ToTable("LitterTypeMapping_JointList");

                    b.HasData(
                        new
                        {
                            InternalLitterTypeCode = 1L,
                            MappedTypeKey = "pl_nn_bag_cabg_"
                        });
                });

            modelBuilder.Entity("MLDB.Infrastructure.Repositories.Model.LitterTypeMapping<uint, MLDB.Infrastructure.Repositories.Model.OsparLitterType>", b =>
                {
                    b.Property<long>("InternalLitterTypeCode")
                        .HasColumnType("bigint");

                    b.Property<long>("MappedTypeKey")
                        .HasColumnType("bigint");

                    b.HasKey("InternalLitterTypeCode");

                    b.HasIndex("MappedTypeKey");

                    b.ToTable("LitterTypeMapping_Ospar");

                    b.HasData(
                        new
                        {
                            InternalLitterTypeCode = 1L,
                            MappedTypeKey = 42L
                        });
                });

            modelBuilder.Entity("MLDB.Infrastructure.Repositories.Model.OsparLitterType", b =>
                {
                    b.Property<long>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.None);

                    b.Property<string>("Category")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.HasKey("Code");

                    b.ToTable("OsparLitterTypes");

                    b.HasData(
                        new
                        {
                            Code = 42L,
                            Category = "Plastic",
                            Description = "Ospar Bags"
                        },
                        new
                        {
                            Code = 43L,
                            Category = "Metal",
                            Description = "Ospar Caps/Lids"
                        },
                        new
                        {
                            Code = 44L,
                            Category = "Glass",
                            Description = "Ospar Bottle"
                        },
                        new
                        {
                            Code = 45L,
                            Category = "Polystyrene",
                            Description = "Ospar Styrofoam"
                        });
                });

            modelBuilder.Entity("MLDB.Domain.Survey", b =>
                {
                    b.HasOne("MLDB.Domain.Site", null)
                        .WithMany()
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("LitterItem", "LitterItems", b1 =>
                        {
                            b1.Property<Guid>("SurveyId")
                                .HasColumnType("uuid");

                            b1.Property<int>("LitterTypeId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                            b1.Property<int>("Count")
                                .HasColumnType("integer");

                            b1.HasKey("SurveyId", "LitterTypeId");

                            b1.ToTable("LitterItem");

                            b1.WithOwner()
                                .HasForeignKey("SurveyId");
                        });

                    b.Navigation("LitterItems");
                });

            modelBuilder.Entity("MLDB.Infrastructure.Repositories.Model.InternalLitterType", b =>
                {
                    b.HasOne("MLDB.Domain.LitterSourceCategory", "SourceCategory")
                        .WithMany()
                        .HasForeignKey("SourceCategoryId");

                    b.Navigation("SourceCategory");
                });

            modelBuilder.Entity("MLDB.Infrastructure.Repositories.Model.LitterTypeMapping<string, MLDB.Infrastructure.Repositories.Model.JointListLitterType>", b =>
                {
                    b.HasOne("MLDB.Infrastructure.Repositories.Model.InternalLitterType", null)
                        .WithOne("JointListMapping")
                        .HasForeignKey("MLDB.Infrastructure.Repositories.Model.LitterTypeMapping<string, MLDB.Infrastructure.Repositories.Model.JointListLitterType>", "InternalLitterTypeCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MLDB.Infrastructure.Repositories.Model.JointListLitterType", "MappedType")
                        .WithMany()
                        .HasForeignKey("MappedTypeKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MappedType");
                });

            modelBuilder.Entity("MLDB.Infrastructure.Repositories.Model.LitterTypeMapping<uint, MLDB.Infrastructure.Repositories.Model.OsparLitterType>", b =>
                {
                    b.HasOne("MLDB.Infrastructure.Repositories.Model.InternalLitterType", null)
                        .WithOne("OsparMapping")
                        .HasForeignKey("MLDB.Infrastructure.Repositories.Model.LitterTypeMapping<uint, MLDB.Infrastructure.Repositories.Model.OsparLitterType>", "InternalLitterTypeCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MLDB.Infrastructure.Repositories.Model.OsparLitterType", "MappedType")
                        .WithMany()
                        .HasForeignKey("MappedTypeKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MappedType");
                });

            modelBuilder.Entity("MLDB.Infrastructure.Repositories.Model.InternalLitterType", b =>
                {
                    b.Navigation("JointListMapping");

                    b.Navigation("OsparMapping");
                });
#pragma warning restore 612, 618
        }
    }
}
