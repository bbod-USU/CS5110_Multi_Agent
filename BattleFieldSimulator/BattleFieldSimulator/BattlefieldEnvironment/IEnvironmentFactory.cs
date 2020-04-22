using System.Collections.Generic;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public interface IEnvironmentFactory
    {
        Environment CreateEnvironment(IMap map, List<Troop> allies, List<Troop> adversaries, string outFile);
    }
}