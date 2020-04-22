using System.Collections.Generic;
using System.IO;
using BattleFieldSimulator.FileSystem;
using BattleFieldSimulator.JsonSerialization;
using static BattleFieldSimulator.FileSystem.FileSystemConstants;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public class TroopLoader : ITroopLoader
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IFileSystem _fileSystem;

        public TroopLoader(IJsonSerializer jsonSerializer, IFileSystem fileSystem)
        {
            _jsonSerializer = jsonSerializer;
            _fileSystem = fileSystem;
        }
        public List<Troop> LoadAllies(string troopFile)
        {
            var troops = _jsonSerializer.Deserialize(Path.Combine(TroopFileLocation, troopFile)).GetTroop("Allies");
            return troops;
        }

        public List<Troop> LoadAdversaries(string troopFile)
        {
            var troops = _jsonSerializer.Deserialize(Path.Combine(TroopFileLocation, troopFile)).GetTroop("Adversaries");
            return troops;
        }
    }
}