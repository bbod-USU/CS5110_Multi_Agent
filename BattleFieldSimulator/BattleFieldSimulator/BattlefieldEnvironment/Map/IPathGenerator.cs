using System.Collections.Generic;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public interface IPathGenerator
    {
        List<MapPath> GeneratePaths(Point position, Point destination, IMap map);
    }
}