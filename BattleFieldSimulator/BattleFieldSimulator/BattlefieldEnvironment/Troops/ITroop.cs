using System.Collections.Generic;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public class ITroop
    {
        int SightDistance { get;}
        int EngagementDistance { get;}
        int TroopCount { get; }
        double MovementSpeed { get; }
        double WeaponDamage { get; }
        double Marksmanship { get; }
        double Aggressiveness { get; }
        double Defense { get; }
        Queue<Point> AssistanceQueue { get; }
        Point Objective { get; }
        double CurrentSpeed { get; set; }
        List<Troop> IdentifiedEnemy { get; } 
    }
}