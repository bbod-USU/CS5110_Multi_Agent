using BattleFieldSimulator.BattlefieldEnvironment;

namespace BattleFieldSimulator.SimRunner
{
    public interface ISimulation
    {
        void Run(IEnvironment environment);
    }
}