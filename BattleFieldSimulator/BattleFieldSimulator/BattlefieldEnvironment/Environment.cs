using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using BattleFieldSimulator.FileSystem;

namespace BattleFieldSimulator.BattlefieldEnvironment
{
    public class Environment : IEnvironment
    {
        public IMap Map { get; }
        public List<Troop> Allies { get; }
        public List<Troop> Adversaries { get; }
        public StreamWriter OutFile { get; }

        public Environment(IMap map, List<Troop> allies, List<Troop> adversaries)
        {
            Map = map;
            Allies = allies;
            Adversaries = adversaries;
            const string format = "M_dd_yyyy_hh-mm-ss-tt";
            var logName = $"log_{DateTime.Now.ToString(format)}.txt";
            OutFile = new StreamWriter(Path.Combine(FileSystemConstants.LogDirectory, logName));
        }

    }
}