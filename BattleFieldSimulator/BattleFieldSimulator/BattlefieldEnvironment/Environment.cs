using System.Collections.Generic;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public class Environment : IEnvironment
    {
        public IMap Map { get; }
        public List<Troop> Allies { get; }
        public List<Troop> Adversaries { get; }
        public Environment(IMap map, List<Troop> allies, List<Troop> adversaries)
        {
            Map = map;
            Allies = allies;
            Adversaries = adversaries;
        }
        
    }
}