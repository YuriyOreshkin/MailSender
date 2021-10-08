using MailSender.Models;
using MailSender.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace MailSender.Services
{
    public class APITargetsProvider : ITargetsProvider
    {
        private string url;
        public APITargetsProvider(string _url)
        {
            this.url = _url;
        }

        /* public IQueryable<Employee> GetEmployees(string filter)
         {

                 using (var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
                 {
                     UriBuilder builder = new UriBuilder(api_path + "USERS/Users_Read");
                     var parameters = new Dictionary<string, string>
                             {
                                { "sort", "FIO-asc" },
                                { "page", "1" },
                                { "pageSize","20" },
                                { "group",""},
                                { "filter", filter}
                             };
                     var content = new FormUrlEncodedContent(parameters);
                     var response = client.PostAsync(builder.Uri,content).Result;
                     var responseString = response.Content.ReadAsStringAsync().Result;
                     dynamic obj = JsonConvert.DeserializeObject(responseString);
                     List<Employee> result = new List<Employee>();
                     foreach (var employee in obj.Data)
                     {
                         result.Add(new Employee { ID = employee.ID, FullName = employee.FIO, Login = employee.Login });
                     }

                         return result.AsQueryable();

                 }
         }



         public IQueryable<Employee> GetEmployeesByDepartment(string code)
         {
             using (var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
             {
                 UriBuilder builder = new UriBuilder(api_path + "api/getUsersByOrg");
                 builder.Query = "ident="+code;
                 var response = client.GetAsync(builder.Uri).Result;
                 response.EnsureSuccessStatusCode();
                 var responseString = response.Content.ReadAsStringAsync().Result;
                 dynamic obj = JsonConvert.DeserializeObject(responseString);

                 List<Employee> result = new List<Employee>();
                 foreach (var employee in obj)
                 {
                     result.Add(new Employee { ID = employee.ID,  FullName =String.Format("{0} {1} {2}",employee.LastName,employee.FirstName, employee.MiddleName)});
                 }

                 return result.AsQueryable();

             }

         }



         public Employee GetEmployee(long id)
         {
             var filter = String.Format("ID~eq~'{0}'", id.ToString());
             return GetEmployees(filter).FirstOrDefault();
         }


         public Employee GetEmployee(string login)
         {

             var filter= String.Format("Login~eq~'{0}'", login);
             return GetEmployees(filter).FirstOrDefault();
         }

        */

        public TARGET GetTargetById(long id)
        {
            var filter = String.Format("ID~eq~'{0}'", id.ToString());
            return GetTargets(filter).FirstOrDefault();
        }


        //private IEnumerable<TARGETTREE> GetChilds(IEnumerable<TARGET> targets, long parentid)
        //{

        //    return targets.Where(r => r.PARENTID == parentid)
        //            .Select(target => new TARGETTREE
        //            {
        //                ID = target.ID,
        //                NAME = target.NAME,
        //                PARENTID = target.PARENTID,
        //                HASCHILDREN = target.HASCHILDREN,
        //                CHILDS = GetChilds(targets, target.ID)

        //            });
        //}

        private IQueryable<TARGET> GetTargets(string filter)
        {
            
            using (var client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true }))
            {
                List<TARGET> result = new List<TARGET>();
                UriBuilder builder = new UriBuilder(url);
                var parameters = new Dictionary<string, string>
                            {
                               { "sort", "code-asc" },
                               //{ "page", "1" },
                               //{ "pageSize","50" },
                               { "group",""},
                               { "filter", filter}
                            };
                var content = new FormUrlEncodedContent(parameters);
                var response = client.PostAsync(builder.Uri, content).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                dynamic obj = JsonConvert.DeserializeObject(responseString);
               
                foreach (var target in obj.Data)
                {
                    result.Add(new TARGET { ID = target.id, NAME = target.name, HASCHILDREN= target.hasChildren, PARENTID= target.parentID });
                }

                
                return result.AsQueryable();

            }
        }

        public IEnumerable<TARGET> GetTargets()
        {
            var targets = GetTargets("").ToList();

            return targets;
            //    .Where(t => t.PARENTID == null).Select(target=> new TARGETTREE
            //{
            //    ID = target.ID,
            //    NAME = target.NAME,
            //    PARENTID = target.PARENTID,
            //    HASCHILDREN = target.HASCHILDREN,
            //    CHILDS = GetChilds(targets,target.ID)

            //});
        }
    }
}
