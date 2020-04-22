
using System.Collections.Generic;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public interface IEnvironment
    {
        IMap Map { get; }
        List<Troop> Allies { get; }
        List<Troop> Adversaries { get; }
    }
}