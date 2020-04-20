using DryIoc;
using FinalProject.Core;
using FinalProject.Utilities.BootStrap;

namespace FinalProject
{
    public class CoreModule : Module
    {
        public void Register(IContainer container)
        {
            container.Register<ISimRunner, SimRunner>(Reuse.Singleton);
        }

        public void Resolve(IContainer container)
        {
            container.Resolve<ISimRunner>();
        }
    }
    
}