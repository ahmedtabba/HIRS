using Quartz;
using Quartz.Core;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoulevardManagement.WebApplication.Helper.Quartz
{
    public class UnitySchedulerFactory : StdSchedulerFactory
    {
        private readonly IJobFactory _jobFactory;

        public UnitySchedulerFactory(IJobFactory jobFactory)
        {
            _jobFactory = jobFactory;
        }

        protected override IScheduler Instantiate(QuartzSchedulerResources rsrcs, QuartzScheduler qs)
        {
            qs.JobFactory = _jobFactory;

            return base.Instantiate(rsrcs, qs);
        }
    }
}