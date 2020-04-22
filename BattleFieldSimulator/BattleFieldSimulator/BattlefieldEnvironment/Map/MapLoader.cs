using System.IO;
using BattleFieldSimulator.JsonSerialization;
using static BattleFieldSimulator.FileSystem.FileSystemConstants;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public class MapLoader : IMapLoader
    {
        private readonly IJsonSerializer _jsonSerializer;

        public MapLoader(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }
        
        public IMap LoadMap(string mapName)
        {
           return _jsonSerializer.Deserialize(Path.Combine(MapLocation, mapName)).GetMap("Map");
        }
    }
}