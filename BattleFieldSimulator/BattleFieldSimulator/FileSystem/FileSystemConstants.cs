using static System.IO.Path;
using static System.Environment.SpecialFolder;
using static System.Environment;
using static System.Reflection.Assembly;
namespace BattleFieldSimulator.FileSystem
{
    public static class FileSystemConstants
    {
        public static readonly string ExecutionDirectory =
            GetDirectoryName(GetExecutingAssembly().Location);
        public static readonly string BattleFieldSimulatorDirectory =
            Combine(GetFolderPath(LocalApplicationData), "BattleFieldSimulator");
        public static readonly string LogDirectory =
            Combine(GetFolderPath(CommonDocuments), "BattlefieldSimulator", "Logs");
        public static readonly string TroopFileLocation = 
            Combine(ExecutionDirectory, "troops");
        public static readonly string MapLocation =
            Combine(ExecutionDirectory, "maps");
    }
}