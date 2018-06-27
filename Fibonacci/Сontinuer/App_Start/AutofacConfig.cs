using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using BLL.Interfaces;
using BLL.Services;

namespace Сontinuer
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            var config = GlobalConfiguration.Configuration;

            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterApiControllers(assembly);

            builder.RegisterType<CalculationService>().As<ICalculationService>();
            builder.RegisterType<EasyNetQService>().As<IMessageBusService>();
            builder.RegisterType<ContinuerTransportService>().As<ITransportService>();
            builder.RegisterType<ThreadingService>().As<IThreadingService>();

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}