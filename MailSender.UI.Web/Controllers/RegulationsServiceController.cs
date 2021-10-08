using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MailSender.Extensions;
using MailSender.Models;
using MailSender.Services.Interfaces;
using MailSender.UI.Web.Models;

namespace MailSender.Web.UI.Controllers
{
    public class RegulationsServiceController : Controller
    {
        private IRepository filtersService;
        private IMailsService mailService;

        public RegulationsServiceController(IRepository _filtersService, IMailsService _mailService)
        {
            filtersService = _filtersService;
            mailService = _mailService;
        }



        public JsonResult TreeList([DataSourceRequest] DataSourceRequest request, int? id, DateTime datebegin, DateTime dateend)
        {
            var regulations = filtersService.GetRegulations().Where(e => id.HasValue ? e.PARENTID == id : e.PARENTID == null).ToList().Select(f => ConvertToViewModel(f));
            var result = GetRegulationsWithTotal(regulations.ToList(),datebegin,dateend);

            var tree_result = result.ToTreeDataSourceResult(request,
                e => e.Id,
                e => e.ParentId,
                e => id.HasValue ? e.ParentId == id : e.ParentId == null,
                e => e
            );

            return Json(tree_result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ListRead(string text)
        {

            var regulations = filtersService.GetRegulations().ToList().Where(w=>w.ID != filtersService.GetDefaultRegulation().ID).Select(r => ConvertToViewModel(r));

            if (!string.IsNullOrEmpty(text))
            {
                regulations = regulations.Where(p => p.Name.Contains(text));
            }

            return Json(regulations, JsonRequestBehavior.AllowGet);
        }




        public ActionResult TreeRead(int? id, DateTime datebegin, DateTime dateend)
        {
            
            var regulations = filtersService.GetRegulations().Where(e => id.HasValue ? e.PARENTID == id : e.PARENTID == null).ToList().Select(f =>ConvertToViewModel(f));
            var result = GetRegulationsWithTotal(regulations.ToList(),datebegin,dateend);

            return Json(result,JsonRequestBehavior.AllowGet);
        }


        private List<RegulationViewModel> GetRegulationsWithTotal(IList<RegulationViewModel> regulations, DateTime datebegin, DateTime dateend)
        {
            
            var totals = mailService.GetInbox(datebegin,dateend).ToList().SelectMany(s => s.REGULATIONS, (s, r) => new { total=1, issent = s.ISSENT ? 0 :1 , regulation = r }).Where(w=> regulations.Select(rg=>rg.Id).Contains(w.regulation.ID)).GroupBy(g=>g.regulation).Select(v=> new { Id= v.Key.ID, Total = v.Sum(t=>t.total), Count = v.Sum(t => t.issent) });

                       
            var result = regulations.GroupJoin(totals, regulation => regulation.Id, total => total.Id, (regulation, total) => new { regulation, total }).SelectMany(rt => rt.total.DefaultIfEmpty(), (regulation, total) => new { regulation.regulation, total }).Select(r
                  => new RegulationViewModel
                  {
                      Id = r.regulation.Id,
                      Name = r.regulation.Name,
                      Filter = r.regulation.Filter,
                      Disabled = r.regulation.Disabled,
                      isEdit = r.regulation.isEdit,
                      ParentId = r.regulation.ParentId,
                      sendto = r.regulation.sendto,
                      hasChildren = r.regulation.hasChildren,
                      Total = r.total != null ? r.total.Total : 0,
                      Count = r.total != null ? r.total.Count : 0,
                  });

            return result.ToList();
        }

        private RegulationViewModel ConvertToViewModel(REGULATION regulation)
        {

            return new RegulationViewModel
            {
                Id = regulation.ID,
                Name = regulation.NAME,
                Filter = regulation.FILTER,
                Disabled = regulation.DISABLED,
                isEdit = regulation.ISEDIT,
                ParentId = regulation.PARENTID,
                sendto = regulation.REGULATIONTARGETS.Select(s => s.TARGETID),
                hasChildren = filtersService.GetRegulations().Any(s => s.PARENTID == regulation.ID),
                Total =0,
                Count =0
            };
        } 
        public JsonResult SetTargets(long Id, IEnumerable<long>  targets)
        {
            var entity = filtersService.GetRegulations().FirstOrDefault(r => r.ID == Id);

            if (entity == null)
            {
                
                return Json(new { message = "errors", errors = "Ошибка: Правило не найдено!" });

            }

            try
            {
                filtersService.ModifyRegulation(entity, targets.ToArray());


                return Json(new { message = "OK", result = ConvertToViewModel(entity) });
            }
            catch (Exception exc)
            {

                return Json(new { message = "errors", errors = "Ошибка: " + exc.Message });
            }

        }

        [HttpPost]
        public JsonResult CreateUpdate(RegulationViewModel _regulation)
        {
            var entity = filtersService.GetRegulations().FirstOrDefault(r => r.ID == _regulation.Id);



            if (entity == null)
            {
                if(filtersService.GetRegulations().Any(r=>r.NAME.ToLower() == _regulation.Name.ToLower()))
                    return Json(new { message = "errors", errors = String.Format("Ошибка: Правило с наименованием '{0}' уже существует!", _regulation.Name) });

                entity = new REGULATION();
            }


            _regulation.ToEntity(entity);

            try
            {
                
                filtersService.ModifyRegulation(entity,_regulation.sendto.ToArray());

                _regulation.Id = entity.ID;


                return Json(new { message = "OK", result = _regulation });


            }
            catch (Exception exc)
            {

                return Json(new { message = "errors", errors = "Ошибка: " + exc.Message });
            }
        }


        [HttpPost]
        public JsonResult Delete(long id)
        {

            try
            {
                filtersService.DeleteRegulation(id);

                return Json(new { message = "OK" });
            }
            catch (Exception exc)
            {

                return Json(new { message = "errors", errors = "Ошибка: " + exc.Message });
            }
        }



    }
}
