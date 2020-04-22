using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Schema;
using BattleFieldSimulator.BattlefieldEnvironment;
using BattleFieldSimulator.FileSystem;

namespace BattleFieldSimulator.SimRunner
{
    public class Simulation : ISimulation
    {
        public Simulation()
        {
            
        }

        public void Run(IEnvironment environment)
        {
            var finished = false;
            while (!finished)
            {
                RunRound(environment);
                finished = CheckIfFinished(environment.Allies, environment.Adversaries);
            }
        }

        private void RunRound(IEnvironment environment)
        {
            var map = environment.Map;
            var allies = environment.Allies;
            var adversaries = environment.Adversaries;
            foreach (var troop in allies)
                LookAround(troop, map, adversaries);

            foreach (var troop in adversaries)
                LookAround(troop, map, allies);
            CheckCallForHelp(allies, adversaries);
            var nextTurn = false;
            var current = 0;
            while (!nextTurn)
            {
                if (current >= allies.Count && current >= adversaries.Count)
                    nextTurn = true;
                if (current < allies.Count)
                    Attack(allies[current], allies[current].IdentifiedEnemy);
                if (current < adversaries.Count)
                    Attack(adversaries[current], adversaries[current].IdentifiedEnemy);
                if (current < allies.Count)
                    if (allies[current].TroopCount == allies[current].Fatalities)
                        allies.Remove(allies[current]);
                if (current < adversaries.Count)
                    if (adversaries[current].TroopCount == adversaries[current].Fatalities)
                        adversaries.Remove(adversaries[current]);
                current++;
            }


        }

        private void CheckCallForHelp(List<Troop> allies, List<Troop> adversaries)
        {
            foreach (var eTroop in from ally in allies from eTroop in ally.IdentifiedEnemy where CalculateAttackValues(eTroop, ally)>CalculateAttackValues(ally, eTroop) select eTroop)
            {
                CallForHelp(eTroop.Location, allies);
            }
            foreach (var eTroop in from adversary in adversaries from eTroop in adversary.IdentifiedEnemy where CalculateAttackValues(eTroop, adversary)>CalculateAttackValues(adversary, eTroop) select eTroop)
            {
                CallForHelp(eTroop.Location, adversaries);
            }
        }

        private void CallForHelp(Point eTroopLocation, List<Troop> teams)
        {
            foreach (var teamMember in teams.Where(teamMember => !teamMember.AssistanceQueue.Contains(eTroopLocation)))
            {
                teamMember.AssistanceQueue.Add(eTroopLocation);
            }
        }

        private void LookAround(Troop troop, IMap map, List<Troop> enemy)
        {
            var enterX = 0;
            var enterY = 0;
            var stopX = 0;
            var stopY = 0;
            //Calculate first point.
            if (troop.Location.X - troop.SightDistance < 0)
                enterX = 0;
            else
                enterX = troop.Location.X - troop.SightDistance;
            if (troop.Location.Y - troop.SightDistance < 0)
                enterY = 0;
            else
                enterY = troop.Location.Y - troop.SightDistance;
            //Calculate last point
            if (troop.Location.X + troop.SightDistance > map.X)
                stopX = map.X;
            else
                stopX = troop.Location.X + troop.SightDistance;
            if (troop.Location.Y + troop.SightDistance > map.Y)
                stopY = map.Y;
            else
                stopY = troop.Location.Y + troop.SightDistance;
            
            var nearbyTroops = new List<Troop>();
            var adjustedTroopSight = CalculateTrueSightDistance(troop);
            for (var x = enterX; x < stopX; x++)
            {
                for (var y = enterY; y < stopY; y++)
                {
                    var currentCell = new Point(x, y);
                    if (Distance(troop.Location, currentCell) < adjustedTroopSight)
                        nearbyTroops.AddRange(enemy.Where(q =>
                            q.Location.X == currentCell.X && q.Location.Y == currentCell.Y).ToList());
                    
                }
            }
            troop.IdentifiedEnemy.AddRange(VerifyTroopSighting(nearbyTroops, troop, map));
        }

        private double CalculateTrueSightDistance(Troop troop)
        {
            if (troop.CurrentSpeed == 0)
                return troop.SightDistance;
            var sightDeprecation = troop.MovementSpeed / troop.CurrentSpeed;
            return troop.SightDistance * sightDeprecation;
        }

        private List<Troop> VerifyTroopSighting(List<Troop> enemyTroops, Troop troop, IMap map)
        {
            var returnList = new List<Troop>();
            foreach (var enemy in enemyTroops)
            {
                var x = Math.Min(troop.Location.X, enemy.Location.X);
                var y = Math.Min(troop.Location.Y, enemy.Location.Y);
                var done = false;
                var canSee = true;
                while (!done)
                {
                    var startX = x;
                    var startY = y;
                    if (map.Grid[x][y] > map.Grid[troop.Location.X][troop.Location.Y] + 2)
                        canSee = false;
                    if (x < Math.Max(troop.Location.X, enemy.Location.X))
                        x++;
                    if (y < Math.Max(troop.Location.Y, enemy.Location.Y))
                        y++;
                    if (startX == x && startY == y)
                        done = true;
                }

                if (canSee)
                    returnList.Add(enemy);
            }

            return returnList;
        }

        private void Attack(Troop troop, List<Troop> enemy)
        {
            if (troop.TroopCount == troop.Fatalities)
                return;
            var r = new Random();
            //Each troop can only engage one enemy
            var troopsInEngagement = 0;
            for(var i = 0; i < enemy.Count; i++)
            {
                var e = enemy[i];
                var attack = CalculateAttackValues(troop, e);
                if (e.TroopCount != e.Fatalities)
                {
                    var startTroopsInEngagement = troopsInEngagement;
                    var startingEnemyCount = e.TroopCount - e.Fatalities;
                    var nextEngage = false;
                    while (!nextEngage) 
                    {
                        troopsInEngagement++;
                        if (NextBool(r, attack * 100))
                            e.Fatalities++;
                        if (troopsInEngagement - startTroopsInEngagement == startingEnemyCount && i < enemy.Count)
                            nextEngage = true;
                    }
                }
            }
        }

        private double CalculateAttackValues(Troop troop, Troop enemy)
        {
            var strengthList = new List<double>();
            if(enemy.TroopCount-enemy.Fatalities != 0)
                strengthList.Add((troop.TroopCount-troop.Fatalities) / (enemy.TroopCount-enemy.Fatalities)*EffectivenessRatios.Size);
            else
                strengthList.Add((troop.TroopCount - troop.Fatalities) / double.MinValue*EffectivenessRatios.Size);
            strengthList.Add(troop.Aggressiveness / enemy.Aggressiveness*EffectivenessRatios.Aggressiveness);
            strengthList.Add(troop.Marksmanship / enemy.Marksmanship*EffectivenessRatios.MarksmanShip);
            if (enemy.CurrentSpeed != 0)
                strengthList.Add(troop.CurrentSpeed / enemy.CurrentSpeed*EffectivenessRatios.MovementSpeed);
            else
                strengthList.Add(troop.CurrentSpeed/double.MinValue*EffectivenessRatios.MovementSpeed);
            strengthList.Add(troop.Defense/enemy.Defense*EffectivenessRatios.Defense);
            return Math.Abs(1-1/strengthList.Sum());
        }
        private static bool NextBool(Random r, double truePercentage = 50) =>
            r.NextDouble() < truePercentage / 100.0;

        private double Distance(Point p1, Point p2) =>
            Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));

        private bool CheckIfFinished(List<Troop> environmentAllies, List<Troop> environmentAdversaries) =>
            environmentAdversaries.Count == 0 || environmentAllies.Count == 0;
        
    }
}