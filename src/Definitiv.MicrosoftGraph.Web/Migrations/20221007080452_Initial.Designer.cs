﻿// <auto-generated />
using System;
using Definitiv.MicrosoftGraph.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Definitiv.MicrosoftGraph.Web.Migrations
{
    [DbContext(typeof(LeaveApplicationDbContext))]
    [Migration("20221007080452_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.9");

            modelBuilder.Entity("Definitiv.MicrosoftGraph.Web.Data.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EmailAddress")
                        .IsUnique();

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c7ca12d8-7aa9-49ab-9511-a62cdc391632"),
                            EmailAddress = "tony.stark@HackathonOct22Definitiv.onmicrosoft.com"
                        },
                        new
                        {
                            Id = new Guid("97ac4c3d-4fe9-4417-9b2d-979257906716"),
                            EmailAddress = "peter.parker@HackathonOct22Definitiv.onmicrosoft.com"
                        });
                });

            modelBuilder.Entity("Definitiv.MicrosoftGraph.Web.Data.LeaveApplication", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("From")
                        .HasColumnType("TEXT");

                    b.Property<int>("LeaveType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OutlookEventId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("To")
                        .HasColumnType("TEXT");

                    b.HasKey("Id", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("LeaveApplications");
                });

            modelBuilder.Entity("Definitiv.MicrosoftGraph.Web.Data.LeaveApplication", b =>
                {
                    b.HasOne("Definitiv.MicrosoftGraph.Web.Data.Employee", null)
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}