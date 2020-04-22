using BattleFieldSimulator.BattlefieldEnvironment;
using BattleFieldSimulator.FileSystem;
using BattleFieldSimulator.JsonSerialization;
using BattleFieldSimulator.SimRunner;
using DryIoc;

namespace BattleFieldSimulator
{
    public class CoreModule : IModule
    {
        public void Register(IContainer container)
        {
            container.Register<ISimRunner, SimRunner.SimRunner>(Reuse.Singleton);
            container.Register<IMapLoader, MapLoader>(Reuse.Singleton);
            container.Register<IFileSystem, FileSystem.FileSystem>(Reuse.Singleton);
            container.Register<IEnvironmentFactory, EnvironmentFactory>(Reuse.Singleton);
            container.Register<IEnvironmentSetup, EnvironmentSetup>(Reuse.Singleton);
            container.Register<IJsonSerializer, JsonSerializer>(Reuse.Singleton);
            container.Register<ITroopLoader, TroopLoader>(Reuse.Singleton);
            container.Register<ISimulation, Simulation>(Reuse.Singleton);
//            container.Register<>(Reuse.Singleton);


        }

        public void Resolve(IContainer container)
        {
            
        }
    }
}