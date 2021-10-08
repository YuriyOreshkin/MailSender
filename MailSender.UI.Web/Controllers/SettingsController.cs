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
    public class SettingsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}