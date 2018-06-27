using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using BLL.Interfaces;
using BLL.Services;

namespace Starter
{
    public class AutofacConfig
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            var assembly = Assembly.GetExecutingAssembly();
            builder.RegisterControllers(assembly);

            builder.RegisterType<CalculationService>().As<ICalculationService>();
            builder.RegisterType<CalculationApiService>().As<IApiService>();
            builder.RegisterType<EasyNetQService>().As<IMessageBusService>();
            builder.RegisterType<StarterTransportService>().As<ITransportService>();
            builder.RegisterType<ThreadingService>().As<IThreadingService>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}