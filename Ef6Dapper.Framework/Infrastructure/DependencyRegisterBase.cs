using Unity;

namespace OnePage.Framework.Infrastructure
{
    /// <summary>
    /// Unity register for whole solution.
    /// </summary>
    public abstract class DependencyRegisterBase
    {
        /// <summary>
        /// Get unity container.
        /// </summary>
        /// <returns>Unity container.</returns>
        public static IUnityContainer GetConfiguredContainer()
        {
            return UnityConfig.GetConfiguredContainer();
        }

        /// <summary>
        /// Register dependencies.
        /// </summary>
        /// <param name="container">Unity container.</param>
        public abstract void RegisterTypes(IUnityContainer container);
    }
}
