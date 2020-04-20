using System.ComponentModel;
using DryIoc;

namespace ConsoleClient
{
    public class ConsoleClientModule
    {
        public class CoreModule : IModule
        {
            public void Register(IContainer container)
            {
                container.Register<ISimRunner, SimRunner.SimRunner>();
            }

            public void Resolve(IContainer container)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}