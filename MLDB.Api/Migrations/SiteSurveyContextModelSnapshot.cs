﻿// <auto-generated />
using System;
using MLDB.Api.Models;
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
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("LitterType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DadID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("OsparID")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("LitterTypes");

                    b.HasData(
                        new
                        {
                            Id = 42,
                            Description = "Bags",
                            OsparID = 1
                        },
                        new
                        {
                            Id = 43,
                            Description = "Caps/Lids",
                            OsparID = 2
                        });
                });

            modelBuilder.Entity("MLDB.Api.Models.Site", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateUserIdpId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreateUserIdpId");

                    b.ToTable("Sites");
                });

            modelBuilder.Entity("MLDB.Api.Models.Survey", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Coordinator")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateTimestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreateUserIdpId")
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

                    b.HasIndex("CreateUserIdpId");

                    b.HasIndex("SiteId");

                    b.ToTable("Surveys");
                });

            modelBuilder.Entity("MLDB.Api.Models.User", b =>
                {
                    b.Property<string>("IdpId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailVerified")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("TEXT");

                    b.HasKey("IdpId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MLDB.Api.Models.Site", b =>
                {
                    b.HasOne("MLDB.Api.Models.User", "CreateUser")
                        .WithMany()
                        .HasForeignKey("CreateUserIdpId");

                    b.Navigation("CreateUser");
                });

            modelBuilder.Entity("MLDB.Api.Models.Survey", b =>
                {
                    b.HasOne("MLDB.Api.Models.User", "CreateUser")
                        .WithMany()
                        .HasForeignKey("CreateUserIdpId");

                    b.HasOne("MLDB.Api.Models.Site", null)
                        .WithMany("Surveys")
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("LitterItem", "LitterItems", b1 =>
                        {
                            b1.Property<Guid>("SurveyId")
                                .HasColumnType("TEXT");

                            b1.Property<int>("LitterTypeId")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Count")
                                .HasColumnType("INTEGER");

                            b1.HasKey("SurveyId", "LitterTypeId");

                            b1.HasIndex("LitterTypeId");

                            b1.ToTable("LitterItem");

                            b1.HasOne("LitterType", "LitterType")
                                .WithMany()
                                .HasForeignKey("LitterTypeId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("SurveyId");

                            b1.Navigation("LitterType");
                        });

                    b.Navigation("CreateUser");

                    b.Navigation("LitterItems");
                });

            modelBuilder.Entity("MLDB.Api.Models.Site", b =>
                {
                    b.Navigation("Surveys");
                });
#pragma warning restore 612, 618
        }
    }
}
