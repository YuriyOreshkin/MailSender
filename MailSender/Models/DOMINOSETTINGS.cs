using System;
using System.Collections.Generic;
using System.Text;

namespace MailSender.Models
{
    public class DOMINOSETTINGS : MAILSSERVICESETTINGS
    {
        public string PASSWORD { get; set; }
        public string SERVER { get; set; }
        public string DBNAME { get; set; }

    }
}
