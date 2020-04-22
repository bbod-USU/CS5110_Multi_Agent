namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public struct Point
    { 
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Point p1, Point p2) => p1.X == p2.X && p1.Y == p2.Y;

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }
    }
}