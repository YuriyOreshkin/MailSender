using MailSender.Models;
using MailSender.Services.Interfaces;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Services.Scheduler
{
    public class QuartzSchedulerService : ISchedulerService
    {
        private IScheduler scheduler;
        private ISchedulerServiceConfig config;
        private IJob worker;
        private ILogger logger;

        private ITrigger trigger;
        private IJobDetail jobDetail;
        private bool isstarted;


        public QuartzSchedulerService(IJob _worker,ISchedulerServiceConfig _config,ILogger _logger)
        {
            config = _config;
            worker = _worker;
            logger = _logger;

            StartScheduler(config.ReadSettings());
        }


      

        private async void StartScheduler(SCHEDULERSETTINGS settings)
        {
            scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = new JobFactory(worker);

            await scheduler.Start();

            jobDetail = JobBuilder.Create<SenderWork>().WithIdentity("mailsender").Build();


            //Start Job
            if (settings.ENABLE)
            {
                Start(settings);
            }
            else {

                Stop();
            }
        }

        private void CreateTrigger(SCHEDULERSETTINGS settings)
        {

            var time = new TimeSpan(0, 0, 0);


            if (!String.IsNullOrEmpty(settings.STARTINGDAILYAT)) {

                TimeSpan.TryParse(settings.STARTINGDAILYAT, out time);
            }

            var startAt = TimeOfDay.HourAndMinuteOfDay(time.Hours, time.Minutes);

            time = new TimeSpan(23, 59, 59);

            if (!String.IsNullOrEmpty(settings.ENDINGDAILYAT))
            {
                TimeSpan.TryParse(settings.ENDINGDAILYAT, out time);
               
            }

            var endAt = TimeOfDay.HourAndMinuteOfDay(time.Hours, time.Minutes);

            trigger = TriggerBuilder.Create()
                   .WithIdentity("mailsender")
                   .StartAt(new DateTimeOffset())
                   .StartNow()
                   //.WithSimpleSchedule(x => x.WithIntervalInMinutes(settings.INTERVALINMINUTES).RepeatForever())
                   .WithDailyTimeIntervalSchedule(x=>x.OnDaysOfTheWeek(DaysOfWeeks(settings.ONDAYSOFTHEWEEK).ToArray()).WithIntervalInMinutes(settings.INTERVALINMINUTES).StartingDailyAt(startAt).EndingDailyAt(endAt))
                   .Build();
            
        }

        private List<DayOfWeek> DaysOfWeeks(string setting)
        {
            List<DayOfWeek> dayOfWeeks = new List<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday };
            if (!String.IsNullOrEmpty(setting))
            {
                dayOfWeeks = dayOfWeeks.Where(x => setting.Split('|').Contains((dayOfWeeks.IndexOf(x) + 1).ToString())).ToList();
            }

            return dayOfWeeks;
        }

        public async void Start(SCHEDULERSETTINGS settings)
        {
            
            Stop();
            CreateTrigger(settings);
            await scheduler.ScheduleJob(jobDetail, trigger);
            isstarted = true;
            logger.Info("Scheduler was started!");
        }

        public async void Stop()
        {
            await scheduler.UnscheduleJob(new TriggerKey("mailsender"));
            isstarted = false;
            logger.Info("Scheduler was stopped!");
        }

        public bool isStarted()
        {
            return isstarted;
        }
    }
}
