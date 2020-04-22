using System.Collections.Generic;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public class EnvironmentFactory : IEnvironmentFactory
    {
        

        public Environment CreateEnvironment(IMap map, List<Troop> allies, List<Troop> adversaries)
        {
            return new Environment(map, allies, adversaries);
        }
    }
}