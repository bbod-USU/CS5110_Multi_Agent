using BattleFieldSimulator.BattlefieldEnvironment;
using Environment = BattleFieldSimulator.BattlefieldEnvironment.Environment;

namespace BattleFieldSimulator.SimRunner
{
    public class SimRunner : ISimRunner
    {
        private readonly IEnvironmentSetup _environmentSetup;
        private readonly ISimulation _simulation;

        public SimRunner(IEnvironmentSetup environmentSetup, ISimulation simulation)
        {
            _environmentSetup = environmentSetup;
            _simulation = simulation;
        }
        
        public void RunSimulation(string mapName, string troopFile)
        {
            var environment = Setup(mapName, troopFile);
            _simulation.Run(environment);
        }

        private Environment Setup(string mapName, string troopFile)
        {
            return _environmentSetup.Setup(mapName, troopFile);
        }
    }
}