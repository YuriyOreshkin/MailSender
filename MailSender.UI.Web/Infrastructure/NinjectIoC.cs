using MailSender.Services;
using MailSender.Services.EntityFramework;
using MailSender.Services.Interfaces;
using MailSender.Services.Scheduler;
using Ninject;
using Quartz;
using System.Web;

namespace MailSender.UI.Web.Infrastructure
{
    public static class NinjectIoC
    {
        public static IKernel Initialize()
        {
            IKernel kernel = new StandardKernel();
            AddBindings(kernel);
            return kernel;
        }

        private static IKernel AddBindings(IKernel ninjectKernel)
        {

            //DI Mail Server
            //ninjectKernel.Bind<IMailServiceConfig>().To<StaticMailServiceConfig>().InSingletonScope();
            ninjectKernel.Bind<IMailServiceConfig>().To<XMLMailServiceConfig>().InSingletonScope().WithConstructorArgument("_filename", HttpContext.Current.Server.MapPath("~/App_Data/LotusSettings.xml"));
            ninjectKernel.Bind<IMailsServerService>().To<DominoMailsServerService>().InSingletonScope();
            //DI 
            ninjectKernel.Bind<IMailsService>().To<DominoMailsService>().InSingletonScope();
            //ninjectKernel.Bind<IMailsService>().To<StaticMailsService>().InSingletonScope();
            //DI 
            ninjectKernel.Bind<IWorker>().To<Worker>().InSingletonScope();
            ninjectKernel.Bind<ISchedulerService>().To<QuartzSchedulerService>().InSingletonScope();
            //DI Logger
            ninjectKernel.Bind<ILogger>().To<NLogger>().InSingletonScope().WithConstructorArgument("_logname", HttpContext.Current.Server.MapPath("~/App_Data/log.txt"));

            //DI 
            ninjectKernel.Bind<IJob>().To<SenderWork>().InSingletonScope();
            //DI
            //ninjectKernel.Bind<IRepository>().To<StaticRepository>().InSingletonScope();
            ninjectKernel.Bind<IRepository>().To<EFRepository>().InSingletonScope();
            //DI Targets
            ninjectKernel.Bind<ITargetsProvider>().To<APITargetsProvider>().InSingletonScope().WithConstructorArgument("_url", System.Configuration.ConfigurationManager.AppSettings["url_api_targets"]);

            //DI Sender
            ninjectKernel.Bind<ISenderService>().To<APISender>().InSingletonScope().WithConstructorArgument("_url", System.Configuration.ConfigurationManager.AppSettings["url_api_sender"]);

            //DI Scheduler
            ninjectKernel.Bind<ISchedulerServiceConfig>().To<XMLSchedulerServiceConfig>().InSingletonScope().WithConstructorArgument("_filename", HttpContext.Current.Server.MapPath("~/App_Data/SchedulerSettings.xml"));

            return ninjectKernel;
        }
    }
}