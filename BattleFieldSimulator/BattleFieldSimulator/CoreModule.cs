using BattleFieldSimulator.SimRunner;
using DryIoc;

namespace BattleFieldSimulator
{
    public class CoreModule : IModule
    {
        public void Register(IContainer container)
        {
            container.Register<ISimRunner, SimRunner.SimRunner>();
        }

        public void Resolve(IContainer container)
        {
            
        }
    }
}