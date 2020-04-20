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
            consoleClient.Run(args);
        }
    }
}