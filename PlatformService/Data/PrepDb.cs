using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb 
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if(isProd)
            {
                Console.WriteLine("---> Attempt to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"---> Could not run migrations: {e.Message}...");
                }
            }

            if (!context.Platforms.Any())
            {
                Console.WriteLine("---> Seeding Data...");

                context.Platforms.AddRange(
                    new Platform() 
                    {
                        Name = "Dot Net",
                        Publisher = "Microsoft",
                        Cost = "Free"
                    },
                    new Platform() 
                    {
                        Name = "Dot Net2",
                        Publisher = "Microsoft2",
                        Cost = "Free"
                    },
                    new Platform() 
                    {
                        Name = "Dot Net3",
                        Publisher = "Microsoft3",
                        Cost = "Free"
                    }
                );

                context.SaveChanges();
            }
            else 
            {
                Console.WriteLine("---> Data Ready Already");
            }
        }
    }
}