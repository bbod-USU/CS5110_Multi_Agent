using System;
using DryIoc;

namespace BattleFieldSimulator
{
    public class BootStrapper
    {
        private static readonly string NoBootstrapperMessage = $"Called {nameof(BootStrapper)} before it existed";

        private static BootStrapper _bootStrapper;
        private static IContainer _container;

        public static BootStrapper Instance =>
            _bootStrapper ?? throw new InvalidOperationException(NoBootstrapperMessage);
        
        private BootStrapper(params IModule[] modules)
        {
            _container = new Container();
            foreach (var module in modules)
            {
                module.Register(_container);   
            }
            foreach (var module in modules)
            {
                module.Resolve(_container);
            }
        }

        public static BootStrapper BootstrapSystem(params IModule[] modules) =>
            _bootStrapper = new BootStrapper(modules);

        public T Resolve<T>() => _container.Resolve<T>();
    }
}