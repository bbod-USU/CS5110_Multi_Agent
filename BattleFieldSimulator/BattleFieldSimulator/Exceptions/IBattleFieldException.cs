using System;

namespace BattleFieldSimulator.Exceptions
{
    public abstract class BattleFieldException : Exception
    {
        public abstract string Explanation { get; }

        protected BattleFieldException(string message) : base((string.IsNullOrWhiteSpace(message)
            ? throw new ArgumentException("You must provide a message that is not null or whitespace.", nameof(message))
            : message)){}
        
    }
}