﻿// <auto-generated />
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
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Named")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Sites");
                });
#pragma warning restore 612, 618
        }
    }
}
