﻿// <auto-generated />
using System;
using EquipmentDatabasePopulator5E;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EquipmentDatabasePopulator5E.Migrations
{
    [DbContext(typeof(EquipmentContext))]
    partial class EquipmentContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EquipmentDatabasePopulator5E.Models.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArmorCategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ArmorClass")
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<string>("DamageDice")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DamageType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("DexBonus")
                        .HasColumnType("bit");

                    b.Property<string>("GearCategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasVariant")
                        .HasColumnType("bit");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsVariant")
                        .HasColumnType("bit");

                    b.Property<bool?>("MagicItem")
                        .HasColumnType("bit");

                    b.Property<int?>("MaxDexBonus")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("ParentEquipmentId")
                        .HasColumnType("int");

                    b.Property<string>("RangeCategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RangeLong")
                        .HasColumnType("int");

                    b.Property<int?>("RangeNormal")
                        .HasColumnType("int");

                    b.Property<string>("Rarity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpecialAttribute")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Speed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("StealthDisadvantage")
                        .HasColumnType("bit");

                    b.Property<int?>("StrengthMinimum")
                        .HasColumnType("int");

                    b.Property<int?>("ThrowRangeLong")
                        .HasColumnType("int");

                    b.Property<int?>("ThrowRangeNormal")
                        .HasColumnType("int");

                    b.Property<string>("ToolCategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TwoHandedDamageDice")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TwoHandedDamageType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("URL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VehicleCategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WeaponCategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WeaponRange")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.HasIndex("ParentEquipmentId");

                    b.ToTable("Equipment");
                });

            modelBuilder.Entity("EquipmentDatabasePopulator5E.Models.EquipmentCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("EquipmentDatabasePopulator5E.Models.EquipmentWeaponProperty", b =>
                {
                    b.Property<int>("EquipmentId")
                        .HasColumnType("int");

                    b.Property<int>("WeaponPropertyId")
                        .HasColumnType("int");

                    b.HasKey("EquipmentId", "WeaponPropertyId");

                    b.HasIndex("WeaponPropertyId");

                    b.ToTable("EquipmentWeaponProperties");
                });

            modelBuilder.Entity("EquipmentDatabasePopulator5E.Models.PackContent", b =>
                {
                    b.Property<int>("PackId")
                        .HasColumnType("int");

                    b.Property<int>("ContentId")
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.HasKey("PackId", "ContentId");

                    b.HasIndex("ContentId");

                    b.ToTable("PackContents");
                });

            modelBuilder.Entity("EquipmentDatabasePopulator5E.Models.WeaponProperty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("WeaponProperties");
                });

            modelBuilder.Entity("EquipmentDatabasePopulator5E.Models.Equipment", b =>
                {
                    b.HasOne("EquipmentDatabasePopulator5E.Models.Equipment", "ParentEquipment")
                        .WithMany("Variants")
                        .HasForeignKey("ParentEquipmentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ParentEquipment");
                });

            modelBuilder.Entity("EquipmentDatabasePopulator5E.Models.EquipmentWeaponProperty", b =>
                {
                    b.HasOne("EquipmentDatabasePopulator5E.Models.Equipment", "Equipment")
                        .WithMany()
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EquipmentDatabasePopulator5E.Models.WeaponProperty", "WeaponProperty")
                        .WithMany()
                        .HasForeignKey("WeaponPropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("WeaponProperty");
                });

            modelBuilder.Entity("EquipmentDatabasePopulator5E.Models.PackContent", b =>
                {
                    b.HasOne("EquipmentDatabasePopulator5E.Models.Equipment", "ContentEquipment")
                        .WithMany()
                        .HasForeignKey("ContentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EquipmentDatabasePopulator5E.Models.Equipment", "PackEquipment")
                        .WithMany("PackContents")
                        .HasForeignKey("PackId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ContentEquipment");

                    b.Navigation("PackEquipment");
                });

            modelBuilder.Entity("EquipmentDatabasePopulator5E.Models.Equipment", b =>
                {
                    b.Navigation("PackContents");

                    b.Navigation("Variants");
                });
#pragma warning restore 612, 618
        }
    }
}
