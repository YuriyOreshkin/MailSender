using Kendo.Mvc.UI;
using MailSender.Models;
using MailSender.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MailSender.UI.Web.Controllers
{
    public class TargetsController : Controller
    {
        private IEnumerable<TARGET> targets;

        public TargetsController(ITargetsProvider _targets)
        {
            targets = _targets.GetTargets();
        }


        public ActionResult Targets(IEnumerable<string> sendto)
        {

            ViewBag.Targets = targets.Where(p => p.PARENTID == null).Select(target =>
                          new DropDownTreeItemModel
                          {
                              Id = target.ID.ToString(),
                              Text = target.NAME,
                              Value = target.ID.ToString(),
                              Items = target.HASCHILDREN ? GetChilds(targets, target.ID).ToList() : null  //GetTargets(target.CHILDS).ToList() : null

                          });

            return PartialView("Targets", sendto);

        }

        private IEnumerable<DropDownTreeItemModel> GetChilds(IEnumerable<TARGET> _targets, long parentid)
        {

            return _targets.Where(r => r.PARENTID == parentid)
                    .Select(target => new DropDownTreeItemModel
                    {
                        Id = target.ID.ToString(),
                        Text = target.NAME,
                        Value = target.ID.ToString(),
                        Items = target.HASCHILDREN ? GetChilds(_targets, target.ID).ToList() : null


                    });
        }


    }
}