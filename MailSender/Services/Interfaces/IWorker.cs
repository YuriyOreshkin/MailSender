using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Services.Interfaces
{
    public interface IWorker
    {
        Task Do();
    }
}
