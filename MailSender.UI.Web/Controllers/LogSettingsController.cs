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
    public class LogSettingsController : Controller
    {
       
        private ILogger logger;

        public LogSettingsController(IMailServiceConfig _service, IMailsServerService _server,ILogger _logger)
        {
            logger = _logger;
        }
     
     
        public ActionResult Log()
        {

            return PartialView("~/Views/Settings/Log.cshtml");
        }

        public ActionResult ReadLog([DataSourceRequest]DataSourceRequest request, DateTime datebegin, DateTime dateend)
        {
            var log = logger.ReadLog(datebegin, dateend).Select(line =>
            new LogStringViewModel
            {
                date = DateTime.Parse(line.Split('|')[0]).Date,
                time = DateTime.Parse(line.Split('|')[0]).TimeOfDay,
                type = line.Split('|')[1],
                content = line.Split('|')[3]
            }).OrderByDescending(d => d.date).ThenByDescending(t => t.time);

            JsonResult result = Json(log.ToDataSourceResult(request));
            result.MaxJsonLength = 8675309;


            return result;
        }

        
    }
}