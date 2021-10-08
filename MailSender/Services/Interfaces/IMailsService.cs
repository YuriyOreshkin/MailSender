using MailSender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MailSender.Services.Interfaces
{
    public interface IMailsService
    {
        IQueryable<MAIL> GetInbox(DateTime datebegin, DateTime dateend);
        MAIL GetMailByID(string messageID);
        void MarkSent(MAIL mail);
        void SendOut(MAIL mail);
        void Delete(string messageID);
        IQueryable<REGULATION> GetRegulations();
        REGULATION GetDefaultRegulation();
    }
}
