using System.Collections.Generic;
using BattleFieldSimulator.Exceptions;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public class Troop
    {
        public int SightDistance { get; }
        public int EngagementDistance { get; }
        public int TroopCount { get; }
        public Mission Mission { get; }
        public Point Location { get; set; }
        public double MovementSpeed { get; }
        public double WeaponDamage { get; }
        public double Marksmanship { get; }
        public double Aggressiveness { get; }
        public double Defense { get; }
        public List<Point> AssistanceQueue { get; }
        public Point Objective { get; }
        public double CurrentSpeed { get; set; }
        public List<Troop> IdentifiedEnemy { get; } 
        public int Fatalities { get; set; }
        public Point Destination { get; set; }



        public Troop
        (
            double movementSpeed, 
            int sightDistance,
            int engagementDistance,
            double weaponDamage,
            double marksmanship,
            int troopCount,
            double aggressiveness,
            double defense,
            string mission,
            Point point,
            Point objective
            
        )
        {
            SightDistance = sightDistance;
            EngagementDistance = engagementDistance;
            TroopCount = troopCount;
            MovementSpeed = movementSpeed;
            WeaponDamage = weaponDamage;
            Marksmanship = marksmanship;
            Aggressiveness = aggressiveness;
            Defense = defense;
            var missionM = mission == "Attack" ? Mission.Attack : Mission.Defend;
            Mission = missionM;
            Location = point;
            CurrentSpeed = 0;
            AssistanceQueue = new List<Point>();
            Objective = objective;
            IdentifiedEnemy = new List<Troop>();
            Fatalities = 0;
            Destination = objective;
            ValidateTroop();
        }


        private void ValidateTroop()
        {
            if(SightDistance > 10 || SightDistance < 0)
                throw new InvalidTroopException($"SightDistance must be between (1,10) but was: {SightDistance}");
            if(EngagementDistance > 10 || EngagementDistance < 0)
                throw new InvalidTroopException($"EngagementDistance must be between (1,10) but was: {EngagementDistance}");
            if(MovementSpeed > 10 || MovementSpeed < 0)
                throw new InvalidTroopException($"MovementSpeed must be between (0,10) but was: {MovementSpeed}");
            if(WeaponDamage > 10 || WeaponDamage < 0)
                throw new InvalidTroopException($"WeaponDamage must be between (1,10) but was: {WeaponDamage}");
            if(Marksmanship > 10 || Marksmanship < 0)
                throw new InvalidTroopException($"Marksmanship must be between (1,10) but was: {Marksmanship}");
            if(Aggressiveness > 10 || Aggressiveness < 1)
                throw new InvalidTroopException($"Aggressiveness must be between (1,10) but was: {Aggressiveness}");
            if(Defense > 10 || Defense < 0)
                throw new InvalidTroopException($"Defense must be between (1,10) but was: {Defense}");
            if(TroopCount<=0)
                throw  new InvalidTroopException($"TroopCount must be greater than 0 but was: {TroopCount}");

        }
    }
}