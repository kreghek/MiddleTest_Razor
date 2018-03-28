﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using Test;

namespace Test.Migrations
{
    [DbContext(typeof(TestDbContext))]
    [Migration("20180315061101_Add_Name_To_Graph")]
    partial class Add_Name_To_Graph
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Test.BLL.Graph", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Graphs");
                });

            modelBuilder.Entity("Test.BLL.Node", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("GraphId");

                    b.Property<string>("Name");

                    b.Property<int?>("NodeId");

                    b.HasKey("Id");

                    b.HasIndex("GraphId");

                    b.HasIndex("NodeId");

                    b.ToTable("Node");
                });

            modelBuilder.Entity("Test.BLL.Node", b =>
                {
                    b.HasOne("Test.BLL.Graph")
                        .WithMany("Nodes")
                        .HasForeignKey("GraphId");

                    b.HasOne("Test.BLL.Node")
                        .WithMany("Nodes")
                        .HasForeignKey("NodeId");
                });
#pragma warning restore 612, 618
        }
    }
}
