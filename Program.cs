using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EquipmentDatabasePopulator5E
{
    class Program
    {
        static void Main(string[] args)
        {
            //Set up the configuration to read from appsettings.json
            var configurationBuilder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
            //.Build();

            // Retrieve the connection string
            //IConfigurationRoot configuration = configurationBuilder.Build();
            string connectionString = configurationBuilder.GetConnectionString("DefaultConnection");


            // Set up DbContext options
            var optionsBuilder = new DbContextOptionsBuilder<EquipmentContext>()
                .UseSqlServer(connectionString);
            //Program p = new Program();

            using (var context = new EquipmentContext(optionsBuilder.Options))
            {
                context.Database.Migrate();
                // Your database operations here
                //LoadService(context);
                //var service = new EquipmentService();
                //await service.LoadEquipment();

                //var service = new EquipmentService();
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

        void LoadService(EquipmentContext context)
        {

        }
    }
}