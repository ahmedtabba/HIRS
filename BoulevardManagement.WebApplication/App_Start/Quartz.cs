using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BoulevardManagement.WebApplication.App_Start
{
    public static class Quartz
    {
        public static void Start()
        {
            var factoryUnity = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ISchedulerFactory)) as ISchedulerFactory;

            IScheduler scheduler = factoryUnity.GetScheduler().Result;
            scheduler.Start().GetAwaiter();


            //IJobDetail job = JobBuilder.Create<CustomerCountJob>().Build();

            //ITrigger trigger = TriggerBuilder.Create()
            //    .StartNow()
            //    .WithSimpleSchedule(x => x
            //        .WithIntervalInSeconds(20)
            //        .RepeatForever())
            //.Build();

            //scheduler.ScheduleJob(job, trigger).GetAwaiter();

            //scheduler.Clear().GetAwaiter();
        }
    }
}