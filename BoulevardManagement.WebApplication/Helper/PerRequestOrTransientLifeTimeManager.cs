using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity.AspNet.Mvc;
using Unity.Lifetime;

namespace BoulevardManagement.WebApplication.Helper
{
    public class PerRequestOrTransientLifeTimeManager : LifetimeManager, IInstanceLifetimeManager, IFactoryLifetimeManager, ITypeLifetimeManager
    {
        private readonly PerRequestLifetimeManager _perRequestLifetimeManager = new PerRequestLifetimeManager();
        private readonly TransientLifetimeManager _transientLifetimeManager = new TransientLifetimeManager();

        private LifetimeManager GetAppropriateLifetimeManager()
        {
            if (HttpContext.Current == null)
            {
                return _transientLifetimeManager;
            }

            return _perRequestLifetimeManager;
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
            return this;
        }
    }
}