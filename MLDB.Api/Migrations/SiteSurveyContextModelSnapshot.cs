﻿// <auto-generated />
using System;
using MLDB.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MLDB.Api.Migrations
{
    [DbContext(typeof(SiteSurveyContext))]
    partial class SiteSurveyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.11");

            modelBuilder.Entity("MLDB.Domain.LitterSourceCategory", b =>
                {
                    b.Property<ushort>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("LitterSourceCategory");

                    b.HasData(
                        new
                        {
                            Id = (ushort)1,
                            Description = "Stand up paddleboards",
                            Name = "SUP"
                        },
                        new
                        {
                            Id = (ushort)2,
                            Description = "Text about fisheries",
                            Name = "Fisheries"
                        });
                });

            modelBuilder.Entity("MLDB.Domain.Site", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("beachCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("beachName")
                        .HasColumnType("TEXT");

                    b.Property<string>("countryCode")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Sites");
                });

            modelBuilder.Entity("MLDB.Domain.Survey", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("CoordinatorName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndTimeStamp")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SiteId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTimeStamp")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalKg")
                        .HasColumnType("TEXT");

                    b.Property<short>("VolunteerCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

                    b.ToTable("Surveys");
                });

            modelBuilder.Entity("MLDB.Domain.User", b =>
                {
                    b.Property<ushort>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailVerified")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IdpId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdateTimestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IdpId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MLDB.Infrastructure.Repositories.Model.InternalLitterType", b =>
                {
                    b.Property<uint>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<ushort?>("SourceCategoryId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Code");

                    b.HasIndex("SourceCategoryId");

                    b.ToTable("InternalLitterTypes");

                    b.HasData(
                        new
                        {
                            Code = 1u,
                            Description = "Bags",
                            SourceCategoryId = (ushort)1
                        },
                        new
                        {
                            Code = 2u,
                            Description = "Caps/Lids",
                            SourceCategoryId = (ushort)1
                        },
                        new
                        {
                            Code = 3u,
                            Description = "Bottle",
                            SourceCategoryId = (ushort)1
                        },
                        new
                        {
                            Code = 4u,
                            Description = "Styrofoam",
                            SourceCategoryId = (ushort)2
                        });
                });

            modelBuilder.Entity("MLDB.Infrastructure.Repositories.Model.JointListLitterType", b =>
                {
                    b.Property<string>("TypeCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("Definition")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("J_Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

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
                    b.Property<uint>("InternalLitterTypeCode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MappedTypeKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("InternalLitterTypeCode");

                    b.HasIndex("MappedTypeKey");

                    b.ToTable("LitterTypeMapping_JointList");

                    b.HasData(
                        new
                        {
                            InternalLitterTypeCode = 1u,
                            MappedTypeKey = "pl_nn_bag_cabg_"
                        });
                });

            modelBuilder.Entity("MLDB.Infrastructure.Repositories.Model.LitterTypeMapping<uint, MLDB.Infrastructure.Repositories.Model.OsparLitterType>", b =>
                {
                    b.Property<uint>("InternalLitterTypeCode")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("MappedTypeKey")
                        .HasColumnType("INTEGER");

                    b.HasKey("InternalLitterTypeCode");

                    b.HasIndex("MappedTypeKey");

                    b.ToTable("LitterTypeMapping_Ospar");

                    b.HasData(
                        new
                        {
                            InternalLitterTypeCode = 1u,
                            MappedTypeKey = 42u
                        });
                });

            modelBuilder.Entity("MLDB.Infrastructure.Repositories.Model.OsparLitterType", b =>
                {
                    b.Property<uint>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.HasKey("Code");

                    b.ToTable("OsparLitterTypes");

                    b.HasData(
                        new
                        {
                            Code = 42u,
                            Category = "Plastic",
                            Description = "Ospar Bags"
                        },
                        new
                        {
                            Code = 43u,
                            Category = "Metal",
                            Description = "Ospar Caps/Lids"
                        },
                        new
                        {
                            Code = 44u,
                            Category = "Glass",
                            Description = "Ospar Bottle"
                        },
                        new
                        {
                            Code = 45u,
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
                                .HasColumnType("TEXT");

                            b1.Property<int>("LitterTypeId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Count")
                                .HasColumnType("INTEGER");

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
