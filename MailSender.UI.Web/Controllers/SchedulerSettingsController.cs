using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MailSender.Models;
using MailSender.Services.Interfaces;
using MailSender.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MailSender.UI.Web.Controllers
{
    public class SchedulerSettingsController : Controller
    {
        private ISchedulerService service;
        private ISchedulerServiceConfig config;

        public SchedulerSettingsController(ISchedulerService _service, ISchedulerServiceConfig _config)
        {
            config = _config;
            service = _service;
        }


        public ActionResult SchedulerSettings()
        {
            SCHEDULERSETTINGS settings = config.ReadSettings();

            var result = new SchedulerSettingsViewModel
            {
                enable = settings.ENABLE,
                intervalinminutes = settings.INTERVALINMINUTES,

                startingdailyat = ConvertTime(settings.STARTINGDAILYAT),
                endingdailyat  = ConvertTime(settings.ENDINGDAILYAT),

                ondaysoftheweek = settings.ONDAYSOFTHEWEEK.HasValue() ? settings.ONDAYSOFTHEWEEK.Split('|'): new string[] { "1","2","3","4","5","6","7" },
                
                isStarted=service.isStarted()

            };

            return PartialView("~/Views/Settings/SchedulerSettings.cshtml", result);
        }


        private TimeSpan ConvertTime(string time_string)
        {
            TimeSpan time = new TimeSpan(0, 0, 0);
            TimeSpan.TryParse(time_string, out time);

            return time;
        }

        public JsonResult SaveSettings(SchedulerSettingsViewModel viewsettings)
        {

            SCHEDULERSETTINGS settings = viewsettings.ToDomainModel(new SCHEDULERSETTINGS());

            if (viewsettings.startingdailyat > viewsettings.endingdailyat)
            {
                return Json(new { message = "errors", errors = "Ошибка: Время начала работы должно быть меньше окончания!"}, JsonRequestBehavior.AllowGet);
            }

            //Save
            try
            {
                config.SaveSettings(settings);

            }
            catch (Exception exception)
            {
                return Json(new { message = "errors", errors = "Ошибка: " + exception.Message }, JsonRequestBehavior.AllowGet);
            }

            ////Reconnect
            try
            {
              

                if (viewsettings.enable)
                {
                   Task.Run(() =>
                    {
                        service.Start(settings);

                    });
                    
                }
                else
                {
                    Task.Run(() =>
                    {
                        service.Stop();

                    });
                }

                Thread.Sleep(1000);
                viewsettings.isStarted = service.isStarted();

            }
            catch (Exception exception)
            {
                return Json(new { message = "errors", errors = "Ошибка: " + exception.Message }, JsonRequestBehavior.AllowGet);
            }



            return Json(new { message = "OK", result = viewsettings }, JsonRequestBehavior.AllowGet);
        }


    }
}