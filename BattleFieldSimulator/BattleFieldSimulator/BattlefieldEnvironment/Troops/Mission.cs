using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Mission
    {
        [EnumMember(Value = "Attack")]
        Attack = 1,
        [EnumMember(Value = "Defend")]
        Defend = 0
    }
}