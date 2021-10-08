using MailSender.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MailSender.Services.Interfaces
{
    public interface ISchedulerServiceConfig
    {
        SCHEDULERSETTINGS ReadSettings();
        void SaveSettings(SCHEDULERSETTINGS settings);
     
    }
}
