using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Unity;

namespace BoulevardManagement.WebApplication.Helper.Quartz
{
    public class UnityJobFactory : IJobFactory
    {
        private readonly IUnityContainer _container;

        public UnityJobFactory(IUnityContainer container)
        {
            _container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {

            using (var scope = GlobalConfiguration.Configuration.DependencyResolver.BeginScope())
            {
                var job = scope.GetService(bundle.JobDetail.JobType) as IJob;
                return job;
            }
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}