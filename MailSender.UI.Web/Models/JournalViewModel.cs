using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MailSender.UI.Web.Models
{
    public class JournalViewModel
    {
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }
        [Display(Name = "Получатель")]
        public TargetViewModel Target { get; set; }
    }
}