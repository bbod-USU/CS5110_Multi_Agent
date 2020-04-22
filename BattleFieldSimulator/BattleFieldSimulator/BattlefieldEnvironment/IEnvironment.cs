
using System.Collections.Generic;
using System.IO;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public interface IEnvironment
    {
        IMap Map { get; }
        List<Troop> Allies { get; }
        List<Troop> Adversaries { get; }
        StreamWriter OutFile { get; }

    }
}