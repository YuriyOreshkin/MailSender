using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MailSender.UI.Web.Models
{
    public class MailViewModel
    {

        public string id {
            get {

                if (MessageID.IndexOf('@') > -1)
                {
                    return MessageID.Substring(1, MessageID.IndexOf('@') - 1).Replace(".", "").Replace("-", "");
                }
                else {

                    return Guid.NewGuid().ToString() ;
                }
            }
        }

        public string MessageID { get; set; }

        [Display(Name = "От")]
        public string From { get; set; }

        [Display(Name = "Кому")]
        public string To { get; set; }

        [Display(Name = "Тема")]
        public string Subject { get; set; }

        [Display(Name = "Текст")]
        [AllowHtml]
        public string Body { get; set; }

        [Display(Name = "Дата")]
        public DateTime? Date { get; set; }

        
        public bool isSent { get; set; }

        public bool isParse { get; set; }


    }
}
