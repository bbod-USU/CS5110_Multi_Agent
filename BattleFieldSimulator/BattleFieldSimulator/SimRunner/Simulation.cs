using System;
using System.Collections.Generic;
using System.Linq;
using BattleFieldSimulator.BattlefieldEnvironment;

namespace BattleFieldSimulator.SimRunner
{
    public class Simulation : ISimulation
    {
        private IPathGenerator _pathGenerator;
        private IMapPrinter _mapPritner;

        public Simulation(IPathGenerator pathGenerator, IMapPrinter mapPrinter)
        {
            _pathGenerator = pathGenerator;
            _mapPritner = mapPrinter;
        }

        public void Run(IEnvironment environment)
        {
            var finished = false;
            while (!finished)
            {
                PrintRoundInfo(environment);
                RunRound(environment);
                finished = CheckIfFinished(environment.Allies, environment.Adversaries);
            }
            if(environment.Adversaries.Count != 0)
            {
                Console.WriteLine($"The Enemy Won!!\n\n");
                environment.OutFile.WriteLine($"The Enemy Won!!\n\n");
            }
            if(environment.Allies.Count != 0)
            {
                Console.WriteLine($"We Were Victorious!!\n\n");
                environment.OutFile.WriteLine($"We Were Victorious!!\n\n");
            }
        }

        private void PrintRoundInfo(IEnvironment environment)
        {
            Console.WriteLine($"Allies: {environment.Allies.Count} " +
                              $"\nAdversaries: {environment.Adversaries.Count}");
            environment.OutFile.WriteLine($"Allies: {environment.Allies.Count} " +
                                          $"\nAdversaries: {environment.Adversaries.Count}");
            foreach(var ally in environment.Allies)
            {
                Console.WriteLine(
                    $"Ally {environment.Allies.IndexOf(ally)} Casualties: {ally.Fatalities} Initial Troop Count: {ally.TroopCount}");
                environment.OutFile.WriteLine($"Ally {environment.Allies.IndexOf(ally)} Casualties: {ally.Fatalities} Initial Troop Count: {ally.TroopCount}");
                    
            }
            foreach(var adversary in environment.Adversaries)
            {
                Console.WriteLine(
                    $"Adversary {environment.Adversaries.IndexOf(adversary)} Casualties: {adversary.Fatalities} Initial Troop Count: {adversary.TroopCount}");
                environment.OutFile.WriteLine($"Adversary {environment.Adversaries.IndexOf(adversary)} Casualties: {adversary.Fatalities} Initial Troop Count: {adversary.TroopCount}");
            }

            _mapPritner.PrintMap(environment.Map, environment.Allies, environment.Adversaries, environment.OutFile);
        }

        private void RunRound(IEnvironment environment)
        {
            var map = environment.Map;
            var allys = environment.Allies;
            var adversaries = environment.Adversaries;
            foreach (var troop in allys)
                LookAround(troop, map, adversaries);

            foreach (var troop in adversaries)
                LookAround(troop, map, allys);
            CheckCallForHelp(allys, adversaries);
            var nextTurn = false;
            var current = 0;
            foreach (var ally in allys.Where(ally => ally.IdentifiedEnemy.Count == 0))
            {
                if (ally.AssistanceQueue.Count == 0)
                {
                    Move(ally, map, ally.Objective);
                }
                else
                {
                    var smallest = double.MaxValue;
                    var destPoint = new Point(0,0);
                    foreach (var request in ally.AssistanceQueue)
                    {
                        var dist = Distance(request, ally.Location);
                        if (dist < smallest)
                        {
                            smallest = dist;
                            destPoint = request;
                        }
                    }
                    Move(ally, map, destPoint);
                }
            }

            foreach (var adversary in adversaries.Where(adversary => adversary.IdentifiedEnemy.Count == 0))
            {
                if (adversary.AssistanceQueue.Count == 0 )
                {
                    Move(adversary, map, adversary.Objective);
                }
                else
                {
                    var smallest = double.MaxValue;
                    var destPoint = new Point(0,0);
                    foreach (var request in adversary.AssistanceQueue)
                    {
                        var dist = Distance(request, adversary.Location);
                        if (dist < smallest)
                        {
                            smallest = dist;
                            destPoint = request;
                        }
                    }
                    Move(adversary, map, destPoint);
                }
            }
            while (!nextTurn)
            {
                if (current >= allys.Count && current >= adversaries.Count)
                    nextTurn = true;
                if (current < allys.Count)
                    Attack(allys[current], allys[current].IdentifiedEnemy);
                if (current < adversaries.Count)
                    Attack(adversaries[current], adversaries[current].IdentifiedEnemy);
                
                //Clean up the Dead.
                if (current < allys.Count)
                    if (allys[current].TroopCount == allys[current].Fatalities)
                    {
                        allys.Remove(allys[current]);
                        foreach (var adversary in adversaries)
                        {
                            adversary.IdentifiedEnemy.RemoveAll(n => n.TroopCount == n.Fatalities);
                        }
                    }
                if (current < adversaries.Count)
                    if (adversaries[current].TroopCount == adversaries[current].Fatalities)
                    {
                        adversaries.Remove(adversaries[current]);
                        foreach (var ally in allys)
                        {
                            ally.IdentifiedEnemy.RemoveAll(n => n.Fatalities == n.TroopCount);
                        }
                    }
                current++;
            }


        }

        private void Move(Troop troop, IMap map, Point destPoint)
        {
           var x = _pathGenerator.GeneratePaths(troop.Location, destPoint, map);
           CalculateMove(x, map, troop);
        }

        private void CalculateMove(List<MapPath> mapPaths, IMap map, Troop troop)
        {
            mapPaths.OrderBy(p => p.Distance);
            var path = mapPaths[0].Path;
            path.Reverse();
            var movement = GetMovementSpeed(troop);
            var index = 0;
            while (movement > 0)
            {
                var current = path[index];
                var next = path[index + 1];
                if (Math.Abs(map.Grid[next.X][next.Y] - map.Grid[current.X][current.Y]) > 1)
                    movement -= 2;
                else
                    movement--;
                index++;
            }

            troop.Location = path[index];
        }

        private double GetMovementSpeed(Troop troop)
        {
            var r = new Random();
            double speed;
            if (troop.Mission == Mission.Attack)
            {
                if (troop.AssistanceQueue.Count > 0)
                    speed = troop.MovementSpeed;
                else
                {
                    if (NextBool(r, 65))
                        speed = troop.MovementSpeed;
                    else if (NextBool(r, 55))
                        speed = troop.MovementSpeed * .75;
                    else if (NextBool(r, 45))
                        speed = troop.MovementSpeed * .5;
                    else
                        speed = troop.MovementSpeed * .25;
                }
            }
            else
            {
                if (troop.AssistanceQueue.Count > 0)
                    speed = troop.MovementSpeed * .75;
                else
                {
                    if (NextBool(r, 45))
                        speed = troop.MovementSpeed;
                    else if (NextBool(r, 35))
                        speed = troop.MovementSpeed * .75;
                    else if (NextBool(r, 25))
                        speed = troop.MovementSpeed * .5;
                    else
                        speed = troop.MovementSpeed * .15;
                }
            }
            speed = (int) speed;
            if (speed < 1)
                speed = 1;
            return speed;
        }

        private void CheckCallForHelp(List<Troop> allys, List<Troop> adversaries)
        {
            foreach (var eTroop in from ally in allys from eTroop in ally.IdentifiedEnemy where CalculateAttackValues(eTroop, ally)>CalculateAttackValues(ally, eTroop) select eTroop)
            {
                CallForHelp(eTroop.Location, allys);
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
                            q.Location == currentCell && !troop.IdentifiedEnemy.Contains(q)).ToList());
                    
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