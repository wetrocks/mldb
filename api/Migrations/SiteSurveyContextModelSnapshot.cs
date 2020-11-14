﻿// <auto-generated />
using System;
using MLDB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace api.Migrations
{
    [DbContext(typeof(SiteSurveyContext))]
    partial class SiteSurveyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8");

            modelBuilder.Entity("Site", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Named")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Sites");
                });

            modelBuilder.Entity("Survey", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Coordinator")
                        .HasColumnType("TEXT");

                    b.Property<string>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalKg")
                        .HasColumnType("TEXT");

                    b.Property<short>("VolunteerCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Survey");
                });
#pragma warning restore 612, 618
        }
    }
}
