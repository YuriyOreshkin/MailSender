using MailSender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSender.Services.Interfaces
{
    public interface IRepository
    {
        IQueryable<REGULATION> GetRegulations();
        void ModifyRegulation(REGULATION regulation,long[] targets);
        void DeleteRegulation(long id);
        REGULATION GetDefaultRegulation();

        //IQueryable<MAIL> GetMails();
        //void AddMail(MAIL mail);


    }
}
