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

            modelBuilder.Entity("LitterTypeLitterTypesList", b =>
                {
                    b.Property<int>("LitterTypesId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LitterTypesListId")
                        .HasColumnType("TEXT");

                    b.HasKey("LitterTypesId", "LitterTypesListId");

                    b.HasIndex("LitterTypesListId");

                    b.ToTable("LitterTypeLitterTypesList");

                    b.HasData(
                        new
                        {
                            LitterTypesId = 42,
                            LitterTypesListId = "1.0"
                        },
                        new
                        {
                            LitterTypesId = 43,
                            LitterTypesListId = "1.0"
                        },
                        new
                        {
                            LitterTypesId = 44,
                            LitterTypesListId = "1.0"
                        },
                        new
                        {
                            LitterTypesId = 45,
                            LitterTypesListId = "1.0"
                        });
                });

            modelBuilder.Entity("LitterTypeSurveyTemplate", b =>
                {
                    b.Property<int>("LitterTypesId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SurveysId")
                        .HasColumnType("TEXT");

                    b.HasKey("LitterTypesId", "SurveysId");

                    b.HasIndex("SurveysId");

                    b.ToTable("LitterTypeSurveyTemplate");
                });

            modelBuilder.Entity("MLDB.Domain.LitterType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DadId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("OsparId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("LitterTypes");

                    b.HasData(
                        new
                        {
                            Id = 42,
                            DadId = "1",
                            Description = "Bags",
                            OsparId = 1
                        },
                        new
                        {
                            Id = 43,
                            DadId = "2",
                            Description = "Caps/Lids",
                            OsparId = 2
                        },
                        new
                        {
                            Id = 44,
                            DadId = "3",
                            Description = "Bottle",
                            OsparId = 3
                        },
                        new
                        {
                            Id = 45,
                            DadId = "4",
                            Description = "Styrofoam",
                            OsparId = 4
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

            modelBuilder.Entity("MLDB.Domain.SurveyTemplate", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SurveyTemplates");
                });

            modelBuilder.Entity("MLDB.Infrastructure.Repositories.DataLoad.LitterTypesList", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("LitterTypesList");

                    b.HasData(
                        new
                        {
                            Id = "1.0",
                            Description = "Initial litter types"
                        });
                });

            modelBuilder.Entity("LitterTypeLitterTypesList", b =>
                {
                    b.HasOne("MLDB.Domain.LitterType", null)
                        .WithMany()
                        .HasForeignKey("LitterTypesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MLDB.Infrastructure.Repositories.DataLoad.LitterTypesList", null)
                        .WithMany()
                        .HasForeignKey("LitterTypesListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LitterTypeSurveyTemplate", b =>
                {
                    b.HasOne("MLDB.Domain.LitterType", null)
                        .WithMany()
                        .HasForeignKey("LitterTypesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MLDB.Domain.SurveyTemplate", null)
                        .WithMany()
                        .HasForeignKey("SurveysId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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
#pragma warning restore 612, 618
        }
    }
}
