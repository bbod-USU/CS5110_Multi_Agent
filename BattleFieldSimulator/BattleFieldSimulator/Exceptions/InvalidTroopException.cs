namespace BattleFieldSimulator.Exceptions
{
    public class InvalidTroopException : BattleFieldException
    {
        public InvalidTroopException(string message) : base(message)
        {
        }

        public override string Explanation { get; }
    }
}