﻿// <auto-generated />
using System;
using EarthQuakeNews.Infra.Sql.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EarthQuakeNews.Infra.Sql.Migrations
{
    [DbContext(typeof(EarthQuakeNewsSqlContext))]
    partial class EarthQuakeNewsSqlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EarthQuakeNews.Domain.Entities.Earthquake", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("EarthquakeTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("KmDepth")
                        .HasPrecision(7, 4)
                        .HasColumnType("decimal");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Latitude")
                        .HasPrecision(8, 6)
                        .HasColumnType("decimal");

                    b.Property<decimal>("Longitude")
                        .HasPrecision(9, 6)
                        .HasColumnType("decimal");

                    b.Property<decimal>("Magnitude")
                        .HasPrecision(4, 2)
                        .HasColumnType("decimal");

                    b.Property<string>("Place")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Earthquakes", (string)null);
                });

            modelBuilder.Entity("EarthQuakeNews.Domain.Entities.EarthquakeCount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("EarthquakesCount", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
