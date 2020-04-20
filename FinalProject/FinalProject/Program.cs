using ConsoleClient;
using FinalProject.Core;
using FinalProject.Utilities.BootStrap;

namespace FinalProject
{
  internal class Program
  {
    public static void Main(string[] args)
    {
      var bootstrapper = Bootstrapper.BootstrapSystem
      (
        new ConsoleClientModule(),
        new CoreModule()
      );

      var consoleClient = new ConsoleClient.ConsoleClient(bootstrapper.Resolve<ISimRunner>());
      consoleClient.Run(args);
    }
  }
}