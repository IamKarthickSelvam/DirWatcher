namespace DirWatcher.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
        }

        private static void SeedData(AppDbContext context)
        {
            if (!context.BgConfiguration.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                context.BgConfiguration.Add(new Models.BgConfig() 
                {
                    Directory = "C:\\Stuff\\Coding\\TestDirWatcher\\DirWatcher\\TestDirectory", 
                    Interval = 10, 
                    MagicString = "awan"
                });
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Seeding Data...");
            }
        }
    }
}
