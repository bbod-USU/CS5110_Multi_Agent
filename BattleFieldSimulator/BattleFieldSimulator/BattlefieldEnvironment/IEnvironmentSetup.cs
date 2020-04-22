namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public interface IEnvironmentSetup
    {
        Environment Setup(string mapName, string troopFile, string outFile);
    }
}