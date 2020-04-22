namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public class EnvironmentSetup : IEnvironmentSetup
    {
        private readonly IMapLoader _mapLoader;
        private readonly IEnvironmentFactory _environmentFactory;
        private ITroopLoader _troopLoader;

        public EnvironmentSetup(IMapLoader mapLoader, IEnvironmentFactory environmentFactory, ITroopLoader troopLoader)
        {
            _mapLoader = mapLoader;
            _environmentFactory = environmentFactory;
            _troopLoader = troopLoader;
        }

        public Environment Setup(string mapName, string troopFile, string outFile)
        {
            var map = _mapLoader.LoadMap(mapName);
            var allies = _troopLoader.LoadAllies(troopFile);
            var adversaries = _troopLoader.LoadAdversaries(troopFile);
            var env = _environmentFactory.CreateEnvironment(map, allies, adversaries, outFile);
            return env;
        }
    }
}