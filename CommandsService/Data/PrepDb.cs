using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data 
{
    public class PrepDb 
    {
        public static void PrepPopulation(IApplicationBuilder appBuilder)
        {
            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

                var platforms = grpcClient.ReturnAllPlatforms();
                var repo = serviceScope.ServiceProvider.GetService<ICommandRepo>();
                SeedData(repo, platforms);
            }
        }

        private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("--> Seeding new platforms...");
            foreach(Platform platform in platforms)
            {
                if (!repo.ExternalPlatformExist(platform.ExternalId))
                {
                    repo.CreatePlatforms(platform);
                }
                repo.SaveChanges();
            }
        }
    }
}