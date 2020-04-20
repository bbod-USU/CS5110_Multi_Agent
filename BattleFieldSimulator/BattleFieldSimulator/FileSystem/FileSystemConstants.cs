using static System.IO.Path;
using static System.Environment.SpecialFolder;
using static System.Environment;
namespace BattleFieldSimulator.FileSystem
{
    public static class FileSystemConstants
    {
        public static readonly string LogDirectory =
            Combine(GetFolderPath(CommonDocuments), "BattlefieldSimulator", "Logs");
    }
}