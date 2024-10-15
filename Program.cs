using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EquipmentDatabasePopulator5E
{
    class Program
    {
        public static bool buildDatabase = true;

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
                if (buildDatabase)
                {
                    //clear tables if they exist and then recreate them
                    await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS EquipmentWeaponProperties");
                    await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS PackContents");
                    await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS Equipment");
                    await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS Categories");
                    await context.Database.ExecuteSqlRawAsync("DROP TABLE IF EXISTS WeaponProperties");
                    context.Database.EnsureCreated();

                    //begin database operations
                    await LoadService(context);
                }
            }
        }

        static async Task LoadService(EquipmentContext context)
        {
            var service = new EquipmentService(context);

            //load equipment information into database
            await service.LoadEquipmentCategories();
            await service.LoadWeaponProperties();
            await service.LoadEquipment();
            await service.LoadMagicEquipment();

            //create relationship tables and references
            await service.CreateVariantsReferences();
            await service.CreatePackContentRelationships();
            await service.CreateWeaponPropertyRelationships();
        }
    }
}