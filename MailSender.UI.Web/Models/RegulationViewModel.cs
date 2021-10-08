using MailSender.Models;
using System.Collections.Generic;
using System.Linq;

namespace MailSender.UI.Web.Models
{
    public class RegulationViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Filter { get; set; }
        public bool Disabled { get; set; }
        public short? isEdit { get; set; }
        public long? ParentId { get; set; }
        public bool hasChildren { get; set; }
        public int Total { get; set; }
        public int Count { get; set; }
      
        public IEnumerable<long> sendto { get; set; }


        public REGULATION ToEntity(REGULATION regulation)
        {
           
            regulation.NAME = this.Name;
            regulation.FILTER = this.Filter;
            regulation.DISABLED = this.Disabled;
          
            return regulation;
        }

    }
}
