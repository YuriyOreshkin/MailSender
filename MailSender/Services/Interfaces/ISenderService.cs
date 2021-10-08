using System;
using System.Collections.Generic;
using System.Text;

namespace MailSender.Services.Interfaces
{
    public interface ISenderService
    {
        void SendMessage(IEnumerable<long> targets, string message);
    }
}
