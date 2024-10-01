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
                //create database from migration files
                //context.Database.EnsureDeleted();
                context.Database.Migrate();

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