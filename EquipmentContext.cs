using EquipmentDatabasePopulator5E.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentDatabasePopulator5E
{
    public class EquipmentContext : DbContext
    {
        public EquipmentContext(DbContextOptions<EquipmentContext> options) : base(options)
        {
        }

        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipmentCategory> Categories { get; set; }
        public DbSet<WeaponProperty> WeaponProperties { get; set; }
        public DbSet<EquipmentWeaponProperty> EquipmentWeaponProperties { get; set; }
        public DbSet<PackContent> PackContents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //name values are unique so that duplicate records don't get inserted
            modelBuilder.Entity<Equipment>()
                .HasIndex(e => e.Name)
                .IsUnique();

            modelBuilder.Entity<EquipmentCategory>()
                .HasIndex(e => e.Name)
                .IsUnique();

            modelBuilder.Entity<WeaponProperty>()
                .HasIndex(e => e.Name)
                .IsUnique();

            //define the foreign key for magic item variants
            modelBuilder.Entity<Equipment>()
                .HasMany(e => e.Variants)
                .WithOne(e => e.ParentEquipment)
                .HasForeignKey(e => e.ParentEquipmentId)
                .OnDelete(DeleteBehavior.Restrict);

            //define the foreign keys for EquipmentWeaponProperties
            modelBuilder.Entity<Equipment>()
                .HasMany(e => e.WeaponProperties)
                .WithMany()
                .UsingEntity<EquipmentWeaponProperty>(
                    ewp => ewp.HasOne(ewp => ewp.WeaponProperty)
                    .WithMany()
                    .HasForeignKey(ewp => ewp.WeaponPropertyId),
                    ewp => ewp.HasOne(ewp => ewp.Equipment)
                    .WithMany()
                    .HasForeignKey(ewp => ewp.EquipmentId));

            //define the composite key for EquipmentWeaponProperties
            modelBuilder.Entity<EquipmentWeaponProperty>()
                .HasKey(ewp => new { ewp.EquipmentId, ewp.WeaponPropertyId });

            //define the composite key and foreign keys for PackContents
            modelBuilder.Entity<PackContent>()
                .HasKey(e => new { e.PackId, e.ContentId });
            modelBuilder.Entity<PackContent>()
                .HasOne(pc => pc.PackEquipment)
                .WithMany(e => e.PackContents)
                .HasForeignKey(pc => pc.PackId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PackContent>()
                .HasOne(pc => pc.ContentEquipment)
                .WithMany()
                .HasForeignKey(pc => pc.ContentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    //this dbContextFactory gets used when creating migrations manually
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<EquipmentContext>
    {
        public EquipmentContext CreateDbContext(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            string connectionString = configurationBuilder.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<EquipmentContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new EquipmentContext(optionsBuilder.Options);
        }
    }
}
