using System.Collections.Generic;
using System.Linq;
using BattleFieldSimulator.JsonSerialization;

namespace BattleFieldSimulator.BattlefieldEnviornment
{
    public class Map
    {
        public List<List<int>> Grid { get; }
        public int X { get; }
        public int Y { get; }
        public Map(int x, int y, IEnumerable<IJsonObject> grid)
        {
            X = x;
            Y = y;
            Grid = ConvertGrid(grid);
        }

        private List<List<int>> ConvertGrid(IEnumerable<IJsonObject> grid)
        {
            var r_grid = new List<List<int>>();
            var index = 0;
            var jsonObjects = grid.ToList();
            for (var i = 0; i < X; i++)
            {
                for (var j = 0; j < Y; j++)
                {
                    r_grid[i][j] = int.Parse(jsonObjects[index].ToString());
                    index++;
                }
            }

            return r_grid;
        }
    }
}