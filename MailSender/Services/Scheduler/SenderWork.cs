using MailSender.Services.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Services.Scheduler
{
    public class SenderWork : IJob
    {
        private IWorker woker;

        public SenderWork(IWorker _woker)
        {
            woker = _woker;
        } 

        public async Task Execute(IJobExecutionContext context)
        {
             await woker.Do();
           
        }
    }
}
