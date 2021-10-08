using MailSender.UI.Web.Models;
using System.Web.Mvc;

namespace MailSender.UI.Web.Controllers
{
    public class MailsController : Controller
    {
     
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Index()
        {
           
            return View();
        }


        public ActionResult SendingForm()
        {

            return PartialView("~/Views/Mails/SendingForm.cshtml");
        }



        public ActionResult Journal(MailViewModel model)
        {
            
            return PartialView("Journal", model);
        }
    }
}