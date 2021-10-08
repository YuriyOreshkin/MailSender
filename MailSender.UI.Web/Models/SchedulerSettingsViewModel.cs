using MailSender.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MailSender.UI.Web.Models
{
    public class SchedulerSettingsViewModel
    {
        public bool enable { get; set; }
        [Required(ErrorMessage ="Установите интервал запуска!")]
        [Range(0, 1440,ErrorMessage = "Интервал должен находится в диапазоне от {1} до {2}.")]
        public int intervalinminutes { get; set; }

        [Required(ErrorMessage = "Установите время ежедневного запуска работы!")]
        public TimeSpan startingdailyat { get; set; }
        [Required(ErrorMessage = "Установите время ежедневного окончания работы!")]
        public TimeSpan endingdailyat { get; set; }
      
        [Required(ErrorMessage = "Выберите дни недели запуска работы!")]
        public string[] ondaysoftheweek { get; set; }

        public bool isStarted { get; set; }

        public SCHEDULERSETTINGS ToDomainModel(SCHEDULERSETTINGS entity)
        {
            entity.ENABLE = enable;
            entity.INTERVALINMINUTES = intervalinminutes;
            entity.STARTINGDAILYAT = startingdailyat.ToString();
            entity.ENDINGDAILYAT = endingdailyat.ToString();
            entity.ONDAYSOFTHEWEEK = String.Join("|", ondaysoftheweek);

            return entity;
        }
    }
    
}