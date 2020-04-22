using System;
using System.Collections.Generic;
using System.Linq;
using BattleFieldSimulator.Exceptions;
using BattleFieldSimulator.JsonSerialization;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public class Map : IMap
    {
        public List<List<int>> Grid { get; }
        public int X { get; }
        public int Y { get; }
        public Map(int x, int y, List<List<int>> grid)
        {
            X = x;
            Y = y;
            Grid = grid;
            ValidateMap();
        }

        private void ValidateMap()
        {
            if(Grid.Count != X)
                throw new InvalidMapException($"X Dimension of map is incorrect expecting: {X} but was: {Grid.Count}");
            if(Grid[0].Count != Y)
                throw new InvalidMapException($"Y Dimension of map is incorrect expecting: {Y} but was: {Grid[0].Count}");
            foreach (var row in Grid)
            {
                foreach (var x in row)
                {
                    if(x > 5 || x < -5)
                        throw new InvalidMapException($"Map values must be between (-5,5) but value {x} was found.");
                }
            }
        }
    }
}