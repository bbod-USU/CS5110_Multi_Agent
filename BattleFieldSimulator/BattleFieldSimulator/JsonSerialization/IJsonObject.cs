using System.Collections.Generic;
using BattleFieldSimulator.BattlefieldEnvironment;

namespace BattleFieldSimulator.JsonSerialization
{
    public interface IJsonObject
    {
        IEnumerable<IJsonObject> GetJsonArray(string dataIdentifier);
        Map GetMap(string dataIdentifier);
        List<Troop> GetTroop(string dataIdentifier);

    }
}