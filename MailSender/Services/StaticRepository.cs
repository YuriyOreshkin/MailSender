using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MailSender.Models;
using MailSender.Services.Interfaces;

namespace MailSender.Services
{
    public class StaticRepository : IRepository
    {
        private List<REGULATION> filters;

        public StaticRepository()
        {
            filters = new List<REGULATION>()
            {
                new REGULATION
                {
                    ID = -1,
                    NAME="Все письма",
                    FILTER = "",
                    DISABLED = false,
                   
                },

                new REGULATION {
                    ID = 1,
                    NAME="ЕГИССО",
                    FILTER = "{\"prefix\":\"\",\"name\":\"Filter\",\"expression\":{\"logic\":\"and\",\"filters\":[{\"field\":\"Subject\",\"value\":\"ЕГИССО\",\"operator\":\"contains\"}]},\"applyButton\":false,\"fields\":[{\"label\":\"От\",\"name\":\"From\",\"type\":\"string\"},{\"label\":\"Кому\",\"name\":\"To\",\"type\":\"string\"},{\"label\":\"Тема\",\"name\":\"Subject\",\"type\":\"string\"},{\"label\":\"Содержание\",\"name\":\"Text\",\"type\":\"string\"}],\"mainLogic\":\"and\",\"messages\":{\"and\":\"И\",\"or\":\"ИЛИ\",\"apply\":\"Apply\",\"close\":\"Close\",\"addExpression\":\"Add Expression\",\"fields\":\"Fields\",\"operators\":\"Operators\",\"addGroup\":\"Add Group\"},\"operators\":{\"string\":{\"eq\":\"Равно\",\"isempty\":\"Пусто\",\"contains\":\"Содержит\"},\"number\":{\"eq\":\"Is equal to\",\"neq\":\"Is not equal to\",\"gte\":\"Is greater than or equal to\",\"gt\":\"Is greater than\",\"lte\":\"Is less than or equal to\",\"lt\":\"Is less than\",\"isnull\":\"Is null\",\"isnotnull\":\"Is not null\"},\"date\":{\"eq\":\"Is equal to\",\"neq\":\"Is not equal to\",\"gte\":\"Is after or equal to\",\"gt\":\"Is after\",\"lte\":\"Is before or equal to\",\"lt\":\"Is before\",\"isnull\":\"Is null\",\"isnotnull\":\"Is not null\"},\"boolean\":{\"eq\":\"Is equal to\",\"neq\":\"Is not equal to\"}},\"expressionPreview\":true}",
                    DISABLED = true,
                    PARENTID =-1,
                    REGULATIONTARGETS = new List<REGULATIONTARGET>() { new REGULATIONTARGET { TARGETID = -2295 }, new REGULATIONTARGET { TARGETID = -2296 } }
                }
            };
        }

        public void ModifyRegulation(REGULATION regulation,long[] targets)
        {
            if (regulation.ID == 0)
            {
                //Create
                regulation.ID = filters.Count() + 1;
                regulation.PARENTID = -1;
                filters.Add(regulation);
            }
            else {

                var entity = filters.FirstOrDefault(f => f.ID == regulation.ID);
                entity.FILTER = regulation.FILTER;
            }
            
        }



        public IQueryable<REGULATION> GetRegulations()
        {
            return filters.AsQueryable();
        }


        public void DeleteRegulation(long id)
        {
            var entity = filters.FirstOrDefault(f => f.ID == id);
            if (entity != null)
                filters.Remove(entity);
        }

        public REGULATION GetDefaultRegulation()
        {
            return GetRegulations().FirstOrDefault();
        }
    }
}
