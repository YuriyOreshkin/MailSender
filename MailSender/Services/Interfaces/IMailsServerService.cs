using MailSender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSender.Services.Interfaces
{
    public interface IMailsServerService
    {
        object GetServer();
        void Connect(MAILSSERVICESETTINGS settings);
        bool isConnected();
    }
}
