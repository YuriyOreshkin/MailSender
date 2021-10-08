using MailSender.Models;
using MailSender.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Services.EntityFramework
{
    public class EFRepository : IRepository, IDisposable
    {
        private DBContext db = new DBContext();
        private bool disposed = false;

        public void DeleteRegulation(long id)
        {
            REGULATION entity = db.REGULATIONS.FirstOrDefault(r => r.ID == id);
            if (entity != null)
            {
                db.REGULATIONS.Remove(entity);
                db.SaveChanges();

            }
        }

        public IQueryable<REGULATION> GetRegulations()
        {
            return db.REGULATIONS;
        }

        public void ModifyRegulation(REGULATION regulation,long[] targets)
        {
            if (regulation.ID == 0)
            {
                //Create

                regulation.REGULATIONTARGETS =targets.Select(t => new REGULATIONTARGET
                {
                    TARGETID = t

                }).ToList();


                db.REGULATIONS.Add(regulation);
                db.SaveChanges();
            }
            else
            {
                //Update
                //var targets = regulation.REGULATIONTARGETS.ToList();
                var entity = db.REGULATIONS.FirstOrDefault(r => r.ID == regulation.ID);
                if (entity != null)
                {
                    //Delete  targets

                    var delete = entity.REGULATIONTARGETS.Where(r => !targets.Contains(r.TARGETID)).ToList();
                    entity.REGULATIONTARGETS.Where(r=>!targets.Contains(r.TARGETID)).ToList().ForEach(target =>
                     {
                         db.REGULATIONTARGETS.Remove(target);
                     });


                    //Add  targets

                    var add = targets.Except(entity.REGULATIONTARGETS.Select(r => r.TARGETID)).ToArray();
                    Array.ForEach(targets.Except(entity.REGULATIONTARGETS.Select(r=>r.TARGETID)).ToArray(), target =>
                            {
                                db.REGULATIONTARGETS.Add(new REGULATIONTARGET { REGULATIONID = regulation.ID, TARGETID = target });
                            });


                    db.Entry(regulation).State = EntityState.Modified;
                    db.SaveChanges();

                }
            }
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public REGULATION GetDefaultRegulation()
        {
            return GetRegulations().FirstOrDefault(r=>r.ISEDIT==2);
        }
    }
}
