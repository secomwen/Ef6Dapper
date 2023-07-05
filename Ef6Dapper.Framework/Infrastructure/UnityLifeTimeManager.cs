using System.Web;
using Unity.AspNet.Mvc;
using Unity.Lifetime;

namespace OnePage.Framework.Infrastructure
{
    public class UnityLifeTimeManager : LifetimeManager
    {
        //https://stackoverflow.com/questions/33192431/perrequestlifetimemanager-can-only-be-used-in-the-context-of-an-http-request

        private readonly PerRequestLifetimeManager perRequestLifetimeManager = new PerRequestLifetimeManager();
        private readonly PerThreadLifetimeManager perThreadLifetimeManager = new PerThreadLifetimeManager();

        private LifetimeManager GetAppropriateLifetimeManager()
        {
            if (HttpContext.Current == null)
                return perThreadLifetimeManager;

            return perRequestLifetimeManager;
        }

        public override object GetValue(ILifetimeContainer container = null)
        {
            return GetAppropriateLifetimeManager().GetValue();
        }

        public override void SetValue(object newValue, ILifetimeContainer container = null)
        {
            GetAppropriateLifetimeManager().SetValue(newValue);
        }

        public override void RemoveValue(ILifetimeContainer container = null)
        {
            GetAppropriateLifetimeManager().RemoveValue();
        }

        protected override LifetimeManager OnCreateLifetimeManager()
        {
            return GetAppropriateLifetimeManager();
        }
    }
}
