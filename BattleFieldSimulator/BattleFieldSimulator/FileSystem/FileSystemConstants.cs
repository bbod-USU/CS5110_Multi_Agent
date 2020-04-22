using System;
using static System.IO.Path;
using static System.Environment.SpecialFolder;
using static System.Environment;
namespace BattleFieldSimulator.FileSystem
{
    public static class FileSystemConstants
    {
        public static readonly string ExecutionDirectory =
            GetDirectoryName(Environment.CurrentDirectory);
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