using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailSender.Services.Interfaces
{
    public interface ILogger
    {
        void Info(string message);
        void Error(string message);
        IEnumerable<string> ReadLog(DateTime datebegin, DateTime dateend);
    }
}
