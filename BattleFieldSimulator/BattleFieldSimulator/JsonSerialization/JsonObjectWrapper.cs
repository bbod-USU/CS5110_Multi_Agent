using System;
using System.Collections.Generic;
using System.Linq;
using BattleFieldSimulator.BattlefieldEnvironment;
using BattleFieldSimulator.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BattleFieldSimulator.JsonSerialization
{
    public class JsonObjectWrapper : IJsonObject
    {
        private readonly JObject _jObject;

        public JsonObjectWrapper(JObject jObject)
        {
            _jObject = jObject;
        }
        
        public IEnumerable<IJsonObject> GetJsonArray(string dataIdentifier)
        {
            return GetData<JToken,IEnumerable<IJsonObject>> (dataIdentifier, ConvertJsonArray);

            IEnumerable<JsonObjectWrapper> ConvertJsonArray(JToken jArray)
            {
                foreach (var token in jArray)
                {
                    foreach (var item in token)
                    {
                        yield return new JsonObjectWrapper(item as JObject);

                    }
                }
            }
        }
            

        public Map GetMap(string dataIdentifier) => GetData<JToken, Map>(dataIdentifier, jArray =>
        {
            if (!jArray.Any())
                return (Map) null;
            var x = jArray["X"].Value<int>();
            var y = jArray["Y"].Value<int>();
            var grid = JsonConvert.DeserializeObject<List<List<int>>>(jArray["Grid"].ToString());
            return new Map(x, y, grid);
        });

        public List<Troop> GetTroop(string dataIdentifier) => GetData<JToken, List<Troop>>(dataIdentifier, jArray =>
        {
            var r_List = new List<Troop>();
            if (!jArray.Any())
                return null;
            foreach (var token in jArray.Children())
            {
                var troop = token["Troop"];
                var movementSpeed = troop["MovementSpeed"].Value<double>();
                var sightDistance = troop["SightDistance"].Value<int>();
                var engagementDistance = troop["EngagementDistance"].Value<int>();
                var troopCount = troop["TroopCount"].Value<int>();
                var marksmanship = troop["Marksmanship"].Value<double>();
                var weaponDamage = troop["WeaponDamage"].Value<double>();
                var aggressiveness = troop["Aggressiveness"].Value<double>();
                var defense = troop["Defense"].Value<double>();
                var mission = troop["Mission"].Value<string>();
                var entryPointX = troop["EntryPointX"].Value<int>();
                var entryPointY = troop["EntryPointY"].Value<int>();
                var point = new Point(entryPointX, entryPointY);
                var objectiveX = troop["ObjectiveX"].Value<int>();
                var objectiveY = troop["ObjectiveY"].Value<int>();
                var objective = new Point(objectiveX, objectiveY);
                var r_troop = new Troop(movementSpeed, sightDistance, engagementDistance, weaponDamage, marksmanship,
                    troopCount, aggressiveness, defense, mission, point, objective);
                r_List.Add(r_troop);
            }
            return r_List;
        });

        private T2 GetData<T1, T2>(string dataIdentifier, Func<T1, T2> convertFunc)
        {
            if (_jObject.ContainsKey(dataIdentifier))
                return convertFunc.Invoke(_jObject[dataIdentifier].Value<T1>());
            throw new ItemNotFoundException($"Data Identifier \"{dataIdentifier}\" was not found in the file.");
        }
    }
}