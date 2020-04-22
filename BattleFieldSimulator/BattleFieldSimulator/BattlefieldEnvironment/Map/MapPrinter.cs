using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public class MapPrinter : IMapPrinter
    {
        public void PrintMap(IMap map, List<Troop> allies, List<Troop> adversaries, StreamWriter environmentOutFile)
        {
            for (var i = 0; i < map.X; i++)
            {
                for (var j = 0; j < map.Y; j++)
                {
                    var p1 = new Point(i, j);

                    if (adversaries.Any(x => x.Location == p1 && allies.Any(y=>y.Location == p1)))
                    {
                        Console.Write("A/E");
                        environmentOutFile.Write("A/E");
                    }
                    else if (allies.Any(x => x.Location == p1))
                    {
                        Console.Write("A ");
                        environmentOutFile.Write("A ");
                    }
                    else if (adversaries.Any(x => x.Location == p1))
                    {
                        Console.Write("E ");
                        environmentOutFile.Write("E ");
                    }
                    else
                    {
                        Console.Write(". ");
                        environmentOutFile.Write(". ");
                    }


                }

                Console.WriteLine();
                environmentOutFile.WriteLine();
            }
            Console.WriteLine();
            environmentOutFile.WriteLine();
            for(int i =0; i < map.X*2; i++)
            {
                Console.Write("-");
                environmentOutFile.Write("-");
            }
            Console.WriteLine();
            environmentOutFile.WriteLine();
            environmentOutFile.Flush();
        }
    }
}