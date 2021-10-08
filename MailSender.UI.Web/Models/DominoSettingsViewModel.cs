using MailSender.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MailSender.UI.Web.Models
{
    public class DominoSettingsViewModel
    {
        [Required(ErrorMessage ="Необходимо указать имя сервера!")]
        public string server { get; set; }

        [Required(ErrorMessage = "Необходимо указать имя файла базы данных!")]
        public string dbname { get; set; }

        [Required(ErrorMessage = "Необходимо указать пароль!")]
        public string password { get; set; }

        public bool isConnected { get; set; }

        public DOMINOSETTINGS ToDomainModel(DOMINOSETTINGS entity)
        {
            entity.SERVER = server;
            entity.DBNAME = dbname;
            entity.PASSWORD = password;

            return entity;
        }
    }
    
}