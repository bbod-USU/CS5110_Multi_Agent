namespace BattleFieldSimulator.Exceptions
{
    public class InvalidMapException : BattleFieldException
    {
        public InvalidMapException(string message) : base(message)
        {
        }

        public override string Explanation { get; }
    }
}