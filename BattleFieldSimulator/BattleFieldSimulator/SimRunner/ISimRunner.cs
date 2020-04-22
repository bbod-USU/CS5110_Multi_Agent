namespace BattleFieldSimulator.SimRunner
{
    public interface ISimRunner
    {
        void RunSimulation(string mapName, string troopFile, string outFile);
    }
}