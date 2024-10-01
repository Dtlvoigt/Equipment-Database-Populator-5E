using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EquipmentDatabasePopulator5E
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //set up configuration to read from appsettings.json
            var configurationBuilder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
            //.Build();

            //retrieve connection string
            //IConfigurationRoot configuration = configurationBuilder.Build();
            string connectionString = configurationBuilder.GetConnectionString("DefaultConnection");

            // Set up DbContext options
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

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //begin database operations
                await LoadService(context);
            }
        }

        //public ApplicationDbContext CreateDbContext(string[] args)
        //{
        //    // Set up the configuration to read from appsettings.json
        //    var configurationBuilder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json")
        //        .Build();
        //    //.Build();

        //    // Retrieve the connection string
        //    //IConfigurationRoot configuration = configurationBuilder.Build();
        //    string connectionString = configurationBuilder.GetConnectionString("DefaultConnection");


        //    // Set up DbContext options
        //    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
        //        .UseSqlServer(connectionString);

        //    return new ApplicationDbContext(optionsBuilder.Options);
        //}

        static async Task LoadService(EquipmentContext context)
        {
            var service = new EquipmentService(context);

            await service.LoadEquipmentCategories();
            await service.LoadWeaponProperties();
            await service.LoadEquipment();
            await service.LoadMagicEquipment();
        }
    }
}