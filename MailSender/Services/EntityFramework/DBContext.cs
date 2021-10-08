using MailSender.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Services.EntityFramework
{
    public class DBContext : DbContext
    {
        public DBContext(): base("MAILSENDER")
        {

        }
        public DbSet<REGULATION> REGULATIONS { get; set; }
        public DbSet<REGULATIONTARGET> REGULATIONTARGETS { get; set; }
    }
}
