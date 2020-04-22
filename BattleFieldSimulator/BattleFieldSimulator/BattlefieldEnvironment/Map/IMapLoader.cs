namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public interface IMapLoader
    {
        IMap LoadMap(string mapName);
    }
}