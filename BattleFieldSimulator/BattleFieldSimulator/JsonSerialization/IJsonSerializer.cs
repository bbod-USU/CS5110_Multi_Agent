namespace BattleFieldSimulator.JsonSerialization
{
    public interface IJsonSerializer
    {
        T Deserialize<T>(string serialized);
        IJsonObject Deserialize(string filepath);
        string Serialize(object obj);
    }
}