
using DryIoc;

namespace BattleFieldSimulator.Utilities
{
    public interface IModule
    {
        void Register(IContainer container);
        void Resolve(IContainer container);
    }
}