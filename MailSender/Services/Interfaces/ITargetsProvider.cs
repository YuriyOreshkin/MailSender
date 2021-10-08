using MailSender.Models;
using System.Collections.Generic;

namespace MailSender.Services.Interfaces
{
    public interface ITargetsProvider
    {
        //IEnumerable<TARGETTREE> GetTargets();
        IEnumerable<TARGET> GetTargets();
        TARGET GetTargetById(long id);
    }
}
