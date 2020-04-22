using System.Collections.Generic;
using System.IO;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public interface IMapPrinter
    {
        void PrintMap(IMap map, List<Troop> allies, List<Troop> adversaries, StreamWriter environmentOutFile);
    }
}