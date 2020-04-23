using System;
using System.IO;
using BattleFieldSimulator.FileSystem;
using BattleFieldSimulator.SimRunner;

namespace BattleFieldSimulator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var bootstrapper = BootStrapper.BootstrapSystem(new CoreModule());
            var simRunner = bootstrapper.Resolve<ISimRunner>();
            var consoleClient = new ConsoleClient.ConsoleClient(simRunner);
            try
            {
                consoleClient.Run(args);
            }
            catch (Exception e)
            {
                const string format = "M_dd_yyyy_hh-mm-ss-tt";
                var logName = $"log_{DateTime.Now.ToString(format)}.txt";
                var fout = new StreamWriter(Path.Combine(FileSystemConstants.LogDirectory, logName));
                fout.WriteLine(e);
                Console.WriteLine(e);
                
            }
        }
    }
}