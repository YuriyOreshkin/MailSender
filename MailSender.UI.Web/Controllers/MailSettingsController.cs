using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MailSender.Models;
using MailSender.Services.Interfaces;
using MailSender.UI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MailSender.UI.Web.Controllers
{
    public class MailSettingsController : Controller
    {
        private IMailsServerService server;
        private IMailServiceConfig service;

        public MailSettingsController(IMailServiceConfig _service, IMailsServerService _server)
        {
            server = _server;
            service = _service;
        }
     

   
        public ActionResult MailSettings()
        {
            DOMINOSETTINGS settings = (DOMINOSETTINGS)service.ReadSettings();

            var result = new DominoSettingsViewModel
            {
                server = settings.SERVER,
                dbname = settings.DBNAME,
                password = settings.PASSWORD,
                isConnected = server.isConnected()
            };

            return PartialView("~/Views/Settings/MailSettings.cshtml", result);
        }


        public JsonResult SaveSettings(DominoSettingsViewModel viewsettings)
        {
            DOMINOSETTINGS settings = viewsettings.ToDomainModel(new DOMINOSETTINGS());
            
            //Save
            try
            {
                service.SaveSettings(settings);

            }
            catch (Exception exception)
            {
                return Json(new { message = "errors", errors = "Ошибка: " + exception.Message }, JsonRequestBehavior.AllowGet);
            }

            ////Reconnect
            try
            {
           
                  server.Connect(settings);
                  Thread.Sleep(1000);              
            
                  viewsettings.isConnected = server.isConnected();

            }
            catch (Exception exception)
            {
                return Json(new { message = "errors", errors = "Ошибка: " + exception.Message }, JsonRequestBehavior.AllowGet);
            }



            return Json(new { message = "OK", result = viewsettings }, JsonRequestBehavior.AllowGet);
        }

       
    }
}