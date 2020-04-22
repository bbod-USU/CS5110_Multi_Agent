using System;
using System.Collections.Generic;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public class Node
    {
        public int X { get; }
        public int Y { get; }
        public Node Parent { get; }
        public Point ParentPoint { get; }

        public Node(int x, int y, Node parent, Point parentPoint)
        {
            X = x;
            Y = y;
            Parent = parent;
            ParentPoint = parentPoint;
        }

        public Point ToPoint() => new Point(X, Y);
        public string toString() => $"{X},{Y}";

    }
    public class PathGenerator : IPathGenerator
    {
        private IMap _map { get; set; }

        public PathGenerator()
        {
        }

        public List<MapPath> GeneratePaths(Point position, Point destination, IMap map)
        {
            _map = map;
            return FindPath(position, destination);
        }

        private List<MapPath> FindPath(Point position, Point destination)
        {
            var returnPaths = new List<MapPath>();
            var foundCount = 0;
            var row = new List<int>{-1, 0, 0, 1, -1, 1, 1, -1};
            var column = new List<int>{0, -1, 1, 0, 1, 1, -1, -1};
            var queue = new Queue<Node>();
            var destNode = new Node(position.X, position.Y, null, position);
            queue.Enqueue(destNode);
            var visited = new HashSet<string> {destNode.toString()};
            while (queue.Count!=0)
            {
                var curr = queue.Dequeue();
                var i = curr.X;
                var j = curr.Y;
                if (i == destination.X && j == destination.Y)
                {
                    var x = curr;
                    var returnList = new List<Point>();
                    while (x.Parent!=null)
                    {
                        returnList.Add(x.ParentPoint);
                        x = x.Parent;
                    }
                    returnPaths.Add(new MapPath(returnList, foundCount));
                    foundCount++;
                }
                for (var k = 0; k < 6; k++)
                {
                    var x = i + row[k];
                    var y = j + column[k];
                    if (IsValidMove(curr.ToPoint(), curr.ParentPoint, _map.X, _map.Y))
                    {
                        var next = new Node(x, y, curr, new Point(x,y));
                        var nextKey = next.toString();
                        if (!visited.Contains(nextKey))
                        {
                            queue.Enqueue(next);
                            visited.Add(nextKey);
                        }
                    }
                }
            }

            return returnPaths;
        }

        private bool IsValidMove(Point point, Point toPoint, int mapX, int mapY)
        {
            if (point.X < 0 || point.Y < 0 || toPoint.X < 0 || toPoint.Y < 0 ||
                point.X >= mapX || point.Y >= mapY || toPoint.X >= mapX || toPoint.Y >= mapY)
                return false;
            return Math.Abs(_map.Grid[toPoint.X][toPoint.Y] - _map.Grid[point.X][point.Y]) <= 2;
        }
    }
}