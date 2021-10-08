using MailSender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSender.Services.Interfaces
{
    public interface ISchedulerService
    {
        void Start(SCHEDULERSETTINGS settings);
        void Stop();
        bool isStarted();
    }
}
