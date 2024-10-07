using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EquipmentDatabasePopulator5E
{
    class Program
    {
        public static bool rebuildDb = true;

        static async Task Main(string[] args)
        {
            //set up configuration to read from appsettings.json
            var configurationBuilder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            //retrieve connection string
            string connectionString = configurationBuilder.GetConnectionString("DefaultConnection");

            //set up DbContext options
            var optionsBuilder = new DbContextOptionsBuilder<EquipmentContext>()
                .UseSqlServer(connectionString);

            using (var context = new EquipmentContext(optionsBuilder.Options))
            {
                //remove tables from database
                //await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS EquipmentVariants");
                //await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS EquipmentWeaponProperties");
                //await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS PackContents");
                //await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS Equipment");
                //await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS Categories");
                //await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS WeaponProperties");

                //create database from migration files
                //await context.Database.MigrateAsync();

                if (rebuildDb)
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }

                //begin database operations
                await LoadService(context);
            }
        }

        static async Task LoadService(EquipmentContext context)
        {
            var service = new EquipmentService(context);

            if (rebuildDb)
            {
                //load equipment information into database
                await service.LoadEquipmentCategories();
                await service.LoadWeaponProperties();
                await service.LoadEquipment();
                await service.LoadMagicEquipment();


                //create relationship tables and references
                await service.CreateVariantsReferences();
                //await service.CreatePackContentRelationships();
                //await service.CreateWeaponPropertyRelationships();
            }

            var equipment = await context.Equipment.Include(e => e.Variants).ToListAsync();
            //var variants = await context.EquipmentVariants.Include(e => e.Equipment).ThenInclude(e => e.Variants).ThenInclude(e => e.Variant).ToListAsync();
        }
    }
}