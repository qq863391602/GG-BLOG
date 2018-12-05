﻿// <auto-generated />
using System;
using CoreWebApp.Data.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreWebApp.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CoreWebApp.Model.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100);

                    b.Property<bool>("Enabled");

                    b.Property<string>("LastLoginIP")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("LastLoginTime");

                    b.Property<int>("LoginCount");

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("RegisterIP")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Role");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("sys_User");
                });
#pragma warning restore 612, 618
        }
    }
}
