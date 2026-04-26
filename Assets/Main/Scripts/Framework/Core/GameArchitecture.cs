using QFramework;

namespace CodeFighter.Framework.Core
{
    public class GameArchitecture : Architecture<GameArchitecture>
    {
        protected override void Init()
        {
            RegisterUtilities();
            RegisterModels();
            RegisterSystems();
        }

        protected virtual void RegisterUtilities()
        {
        }

        protected virtual void RegisterModels()
        {
        }

        protected virtual void RegisterSystems()
        {
        }
    }
}
