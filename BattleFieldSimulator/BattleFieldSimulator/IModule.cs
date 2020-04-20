using DryIoc;

namespace BattleFieldSimulator
{
    public interface IModule
    {
        void Register(IContainer container);
        void Resolve(IContainer container);
    }
}