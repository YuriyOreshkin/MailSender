using System;
using System.Collections.Generic;
using System.Text;

namespace MailSender.Models
{
    public class TARGETTREE: TARGET
    {
        public TARGETTREE()
        {
            CHILDS =  new HashSet<TARGETTREE>();
        }
        public IEnumerable<TARGETTREE> CHILDS { get; set; }
    }
}
