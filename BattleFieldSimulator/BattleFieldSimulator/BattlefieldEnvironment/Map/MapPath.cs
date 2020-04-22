using System.Collections.Generic;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public class MapPath
    {
        public List<Point> Path { get; }
        public int Distance { get; }
        public MapPath(List<Point> points, int distance)
        {
            Path = new List<Point>();
            Distance = distance;
            foreach (var point in points)
            {
                Path.Add(point);
            }
        }
    }
}