using System.Collections.Generic;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public interface ITroopLoader
    {
        List<Troop> LoadAllies(string troopFile);
        List<Troop> LoadAdversaries(string troopFile);

    }
}