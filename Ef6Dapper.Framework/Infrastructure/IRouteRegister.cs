using System.Web.Routing;

namespace OnePage.Framework.Infrastructure
{
    /// <summary>
    /// Route register.
    /// </summary>
    public interface IRouteRegister
    {
        /// <summary>
        /// Register routes.
        /// </summary>
        /// <param name="routes">Routes.</param>
        void RegisterRoutes(RouteCollection routes);
    }
}
