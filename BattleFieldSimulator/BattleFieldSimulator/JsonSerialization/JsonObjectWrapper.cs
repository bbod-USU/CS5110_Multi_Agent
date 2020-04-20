using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleFieldSimulator.BattlefieldEnviornment;
using BattleFieldSimulator.Exceptions;
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
            var grid = GetJsonArray("map");
            return new Map(x, y, grid);
        });

        private T2 GetData<T1, T2>(string dataIdentifier, Func<T1, T2> convertFunc)
        {
            if (_jObject.ContainsKey(dataIdentifier))
                return convertFunc.Invoke(_jObject[dataIdentifier].Value<T1>());
            throw new ItemNotFoundException($"Data Identifier \"{dataIdentifier}\" was not found in the file.");
        }
    }
}