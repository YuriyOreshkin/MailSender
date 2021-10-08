using MailSender.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MailSender.Services.Interfaces
{
    public interface IMailServiceConfig
    {
        MAILSSERVICESETTINGS ReadSettings();
        void SaveSettings(MAILSSERVICESETTINGS settings);
     
    }
}
