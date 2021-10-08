using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Kendo.DynamicLinq;
using MailSender.Models;
using Newtonsoft.Json;

namespace MailSender.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> TextFilter<T>(this IQueryable<T> queryable, string text)
        {
            var qdefault = queryable.Where(t => false);

            if (String.IsNullOrEmpty(text))
                return qdefault;

            try
            {
                dynamic obj = JsonConvert.DeserializeObject(text);
                string filter = Convert.ToString(obj.expression);

                if (String.IsNullOrEmpty(filter))
                    return qdefault;

                obj = JsonConvert.DeserializeObject<Filter>(filter);


                //obj = JsonConvert.DeserializeObject(filter);
                //filter = obj.expression;
                if (((Filter)obj).Field == null && ((Filter)obj).Filters == null)
                    return qdefault;


                DataSourceResult result = queryable.AsQueryable().ToDataSourceResult<T>(0, 0, null, (Filter)obj, null);
                return result.Data.Cast<T>().AsQueryable();
               
            }
            catch
            {

                return qdefault;
            }
        }


        public static IQueryable<MAIL> Parse(this IQueryable<MAIL> queryable, IEnumerable<REGULATION> regulations, REGULATION default_regulation)
        {
             foreach (REGULATION regulation in regulations.Where(r=>r.ID != default_regulation.ID))
                {
                    queryable.TextFilter(regulation.FILTER).ToList().ForEach(mail =>
                    {
                        mail.REGULATIONS.Add(regulation);
                       
                    });
                }

             //Default regulation
            queryable.Where(r => r.REGULATIONS.Count == 0).ToList().ForEach(mail =>
            {
                mail.REGULATIONS.Add(default_regulation);
                mail.ISPARSE = false;
            });

            return queryable;
        }

    }
}
