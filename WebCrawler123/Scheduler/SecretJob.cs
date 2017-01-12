using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace WebCrawler.Scheduler
{
    public class SecretJob
    {
        public void Scheduler()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            // get a scheduler, start the schedular before triggers or anything else
            IScheduler sched = schedFact.GetScheduler();
            sched.Start();

            // create job
            IJobDetail job = JobBuilder.Create<SimpleJob>()
                    .WithIdentity("job1", "group1")
                    .Build();

            // create trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(6, 00))
                .Build();

            // Schedule the job using the job and trigger 
            sched.ScheduleJob(job, trigger);
        }
    }

    public class SimpleJob : IJob
    {
        void IJob.Execute(IJobExecutionContext context)
        {
            //throw new NotImplementedException();
            Console.WriteLine("Hello, JOb executed");
        }
    }
}
