namespace BattleFieldSimulator.Exceptions
{
    public class ItemNotFoundException : BattleFieldException
    {
        public ItemNotFoundException(string message) : base(message)
        {
            Explanation = message;
        }

        public override string Explanation { get; }
    }
}