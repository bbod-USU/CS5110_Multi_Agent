using System.Collections.Generic;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public interface IMap
    {
        List<List<int>> Grid { get; }
        int X { get; }
        int Y { get; }
    }
}