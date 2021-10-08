using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using MailSender.Models;
using MailSender.Services.Interfaces;
using MailSender.UI.Web.Models;

namespace MailSender.UI.Web.Controllers
{
    public class RegulationController : Controller
    {
        

        public ActionResult Tree()
        {
            return View();
        }


        public ActionResult TreeList()
        {
            return View();
        }


        public ActionResult SetTargets(RegulationViewModel regulation)
        {


           return PartialView("~/Views/Regulation/TargetsForm.cshtml", regulation);
            
        }

        public ActionResult WindowDetails(RegulationViewModel regulation)
        {
        
            return PartialView(regulation);
        }

       
    }
}