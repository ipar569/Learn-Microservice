using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();

        // Platform
        IEnumerable<Platform> GetAllPlatforms();

        void CreatePlatforms(Platform plat);

        bool PlatformExists(int platformId);

        bool ExternalPlatformExist(int externalPlatformId);

        // Command
        IEnumerable<Command> GetCommandsForPlatform(int platformId);

        Command GetCommand(int platformId, int commandId);

        void CreateCommand(int platformId, Command command);
    }
}