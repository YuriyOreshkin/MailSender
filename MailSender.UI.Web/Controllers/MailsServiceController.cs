using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MailSender.Services.Interfaces;
using System.Web.Mvc;
using MailSender.UI.Web.Models;
using MailSender.Models;
using System.IO;
using System.Web;
using MailSender.Extensions;

namespace MailSender.UI.Web.Controllers
{
    public class MailsServiceController : Controller
    {
        private IMailsService mailsService;
        private ITargetsProvider targetsProvider;
       // private IRepository repository;

        public MailsServiceController(IMailsService _mailsService, ITargetsProvider _targetsProvider, IRepository _repository)
        {
            mailsService = _mailsService;
            targetsProvider = _targetsProvider;
            //repository = _repository;
        }


        private MailViewModel ConvertToViewModel(MAIL mail)
        {
            return new MailViewModel
            {
                From = mail.FROM,
                To = mail.SENDTO,
                MessageID = mail.MESSAGEID.Substring(1, mail.MESSAGEID.Length - 2),
                Date = mail.DATE,
                Subject = mail.SUBJECT,
                Body = mail.BODY,
                isSent = mail.ISSENT,
                isParse = mail.ISPARSE 
            };
        }

        public ActionResult GridRead([DataSourceRequest]DataSourceRequest request, DateTime datebegin, DateTime dateend)
        {
           
            var result = mailsService.GetInbox(datebegin,dateend).Select(m => ConvertToViewModel(m)).OrderByDescending(o => o.Date); //.Parse(mailsService.GetRegulations(),mailsService.GetDefaultRegulation()).Select(m =>ConvertToViewModel(m)).OrderByDescending(o => o.Date);

            return Json(result.ToDataSourceResult(request));
        }

        //Read Joutnal
        public ActionResult ReadJournal([DataSourceRequest]DataSourceRequest request, string MessageID)
        {
            var journal = mailsService.GetMailByID(MessageID).JOURNAL;
            var targets = targetsProvider.GetTargets().ToList();

            if (journal != null)
            {
                var result = journal.OrderByDescending(o => o.SENTDATE).Select(j => new JournalViewModel
                {
                    Date = j.SENTDATE,
                    //Target = new TargetViewModel { id = j.TARGETID, Name = targetsProvider.GetTargetById(j.TARGETID) != null ? targetsProvider.GetTargetById(j.TARGETID).NAME : String.Empty }
                    Target = new TargetViewModel { id = j.TARGETID, Name = targets.Any(t=>t.ID ==j.TARGETID) ? targets.First(t => t.ID == j.TARGETID).NAME : String.Empty }

                }).ToDataSourceResult(request);

                return Json(result);
            }

            return Json(Enumerable.Empty<JournalViewModel>());
        }

        ////Delete mail
        //[ValidateInput(false)]
        //public ActionResult Delete([DataSourceRequest]DataSourceRequest request, MailViewModel mailview)
        //{
        //    if (mailview != null)
        //    {
        //        mailsService.Delete(mailview.MessageID);
        //    }

        //    return Json(new[] { mailview }.ToDataSourceResult(request, ModelState));

        //}

        public ActionResult Delete([Bind(Prefix = "messages")]IEnumerable<string> messages)
        {

            foreach (string messageid in messages)
            {
                try
                {

                    mailsService.Delete(messageid);


                }
                catch (Exception exc)
                {
                    return Json(new { message = "errors", errors = "Ошибка: " + exc.Message });
                }
            }
                

            return Json(new { message = "OK" });

        }


        public ActionResult SendOut([Bind(Prefix = "messages")]IEnumerable<string> messages,IEnumerable<long> targets)
        {
            List<MailViewModel> mails = new List<MailViewModel>();

            foreach (string messageid in messages)
            {
                var mail = mailsService.GetMailByID(messageid);
                if (mail != null)
                {
                    try
                    {
                        //mail = new[] { mail }.AsQueryable<MAIL>().Parse(mailsService.GetRegulations(), mailsService.GetDefaultRegulation()).FirstOrDefault();
                        mail.JOURNAL = targets.Select(t => new MAILSENT { TARGETID = t, SENTDATE = DateTime.Now }).ToList();

                        mailsService.SendOut(mail);
                        mails.Add(ConvertToViewModel(mail));

                    }
                    catch (Exception exc)
                    {
                        return Json(new { message = "errors", result = mails.Select(s => s.id), errors = "Ошибка: " + exc.Message });
                    }
                }
            }

            return Json(new { message = "OK", result = mails.Select(s => s.id) });

        }

        
        public ActionResult MarkSent([Bind(Prefix = "messages")]IEnumerable<string> messages, bool mark)
        {
            List<MailViewModel> mails = new List<MailViewModel>();

            foreach (string messageid in messages)
            {
                var mail = mailsService.GetMailByID(messageid);
                if (mail != null)
                {
                    if (mail.ISSENT != mark)
                    {
                        try
                        {
                            mail.ISSENT = mark;
                            mailsService.MarkSent(mail);
                            mails.Add(ConvertToViewModel(mail));

                        }
                        catch (Exception exc)
                        {
                            return Json(new { message = "errors", result = mails.Select(s => s.id), errors = "Ошибка: " + exc.Message });
                        }
                    }
                }
            }

            return Json(new { message = "OK", result = mails.Select(s => s.id) });

        } 

    }
}