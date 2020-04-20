using BattleFieldSimulator.FileSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BattleFieldSimulator.JsonSerialization
{
    public class JsonSerializer : IJsonSerializer
    {
        private readonly IFileSystem _fileSystem;
        public JsonSerializer(IFileSystem fileSystem) => _fileSystem = fileSystem;

        public T Deserialize<T>(string serialized) => JsonConvert.DeserializeObject<T>(serialized);

        public IJsonObject Deserialize(string filepath) =>
            new JsonObjectWrapper(JObject.Parse(_fileSystem.ReadAllText(filepath)));

        public string Serialize(object obj) => JsonConvert.SerializeObject(obj);
    }
}