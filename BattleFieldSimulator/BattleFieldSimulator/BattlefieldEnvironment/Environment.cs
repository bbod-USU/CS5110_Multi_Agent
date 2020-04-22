using System.Collections.Generic;
using System.IO;
using BattleFieldSimulator.FileSystem;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public class Environment : IEnvironment
    {
        public IMap Map { get; }
        public List<Troop> Allies { get; }
        public List<Troop> Adversaries { get; }
        public StreamWriter OutFile { get; }

        public Environment(IMap map, List<Troop> allies, List<Troop> adversaries, string outFile)
        {
            Map = map;
            Allies = allies;
            Adversaries = adversaries;
            OutFile = new StreamWriter(Path.Combine(FileSystemConstants.ExecutionDirectory, outFile));
        }

    }
}