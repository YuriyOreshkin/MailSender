using MailSender.Models;
using MailSender.Services.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Services
{
    public class Worker : IWorker
    {
        private ILogger logger;
        private IMailsService service;

        public Worker(ILogger _logger, IMailsService _service)
        {
            logger = _logger;
            service = _service;
        } 


        public Task Do()
        {
            return Task.Run(() => {

                StringBuilder logmessage = new StringBuilder("Worker.Do.Start");
                logmessage.Append("\r\n");

                var forSent = service.GetInbox(DateTime.MinValue,DateTime.MaxValue).Where(m => !m.ISSENT).OrderBy(o=>o.DATE).ToList();//.Where(m => !m.ISSENT).Parse(service.GetRegulations().Where(r => !r.DISABLED), service.GetDefaultRegulation()).ToList();
                foreach (MAIL mail in forSent)
                {
                    foreach (REGULATION regulation in mail.REGULATIONS)
                    {
                        if (!regulation.DISABLED)
                        {

                            regulation.REGULATIONTARGETS.ToList().ForEach(target =>
                                 {
                                     if (!mail.JOURNAL.Select(j => j.TARGETID).Contains(target.TARGETID))
                                         mail.JOURNAL.Add(new MAILSENT { TARGETID = target.TARGETID, SENTDATE = DateTime.Now });
                                 });
                        }
                        else
                        {
                            //Sent to default if Disable
                            service.GetDefaultRegulation().REGULATIONTARGETS.ToList().ForEach(target =>
                            {
                                if (!mail.JOURNAL.Select(j => j.TARGETID).Contains(target.TARGETID))
                                    mail.JOURNAL.Add(new MAILSENT { TARGETID = target.TARGETID, SENTDATE = DateTime.Now });
                            });

                        }

                        try
                        {
                            service.SendOut(mail);
                           

                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex.Message);
                        }
                    }
                }

                logmessage.AppendFormat(">>{0}: {1}/{2}", "Отправлено".PadLeft(30), forSent.Where(m => m.ISSENT).Count(),forSent.Count());
                
                foreach (REGULATION regulation in  service.GetRegulations())
                {
                    if (forSent.Any(m => m.REGULATIONS.Contains(regulation)))
                    {
                        logmessage.Append("\r\n");
                        logmessage.AppendFormat(">>{0}: {1}/{2}", regulation.NAME.PadLeft(30), forSent.Where(m => m.ISSENT).Where(m => m.REGULATIONS.Contains(regulation)).Count(), forSent.Where(m => m.REGULATIONS.Contains(regulation)).Count());
                    }
                }
                
                logmessage.Append("\r\n");
                logmessage.Append("Worker.Do.Stop");

                logger.Info(logmessage.ToString());
            });
        }
    }
}
