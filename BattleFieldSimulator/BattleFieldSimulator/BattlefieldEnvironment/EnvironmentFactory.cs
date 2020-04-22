using System.Collections.Generic;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public class EnvironmentFactory : IEnvironmentFactory
    {
        

        public Environment CreateEnvironment(IMap map, List<Troop> allies, List<Troop> adversaries, string outFile)
        {
            return new Environment(map, allies, adversaries, outFile);
        }
    }
}