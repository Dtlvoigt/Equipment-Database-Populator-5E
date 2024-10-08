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
        //public DbSet<EquipmentVariant> EquipmentVariants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //make name values unique so that duplicate records don't get inserted
            modelBuilder.Entity<Equipment>()
                .HasIndex(e => e.Name)
                .IsUnique();
            modelBuilder.Entity<Equipment>()
                .HasMany(e => e.Variants)
                .WithOne(e => e.ParentEquipment)
                .HasForeignKey(e => e.ParentEquipmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EquipmentCategory>()
                .HasIndex(e => e.Name)
                .IsUnique();
            
            modelBuilder.Entity<WeaponProperty>()
                .HasIndex(e => e.Name)
                .IsUnique();

            //define the composite key and foreign keys for EquipmentVariants
            //modelBuilder.Entity<EquipmentVariant>()
            //    .HasKey(e => new { e.ParentEquipmentId, e.VariantId });
            //modelBuilder.Entity<EquipmentVariant>()
            //    .HasOne(ev => ev.ParentEquipment)
            //    .WithMany(e => e.Variants)
            //    .HasForeignKey(ev => ev.ParentEquipmentId)
            //    .OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<EquipmentVariant>()
            //    .HasOne(ev => ev.Variant)
            //    .WithMany()
            //    .HasForeignKey(ev => ev.VariantId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //define the composite key and foreign keys for EquipmentWeaponProperties
            modelBuilder.Entity<EquipmentWeaponProperty>()
                .HasKey(e => new { e.EquipmentId, e.WeaponPropertyId });
            modelBuilder.Entity<EquipmentWeaponProperty>()
                .HasOne(ewp => ewp.Equipment)
                .WithMany(e => e.WeaponProperties)
                .HasForeignKey(ewp => ewp.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<EquipmentWeaponProperty>()
                .HasOne(ewp => ewp.WeaponProperty)
                .WithMany()
                .HasForeignKey(ewp => ewp.WeaponPropertyId)
                .OnDelete(DeleteBehavior.Restrict);

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
