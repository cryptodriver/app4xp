using Application.Infrastructure.ApiPool;
using Application.Infrastructure.EventBus;
using Application.Infrastructure.Logger;
using Application.Infrastructure.SQLiter;
using Application.Infrastructure.ThreadPool;
using Autofac;
using System;
using System.Linq;
using System.Reflection;

namespace Application.Core
{
    public class AutofacLocator : IAutofacLocator
    {
        private IContainer _container;

        private Config _config;

        public AutofacLocator(Config config)
        {
            _config = config;
        }

        public TInterface Get<TInterface>()
        {
            return _container.Resolve<TInterface>();
        }

        public TInterface Get<TInterface>(string name)
        {
            return _container.ResolveNamed<TInterface>(name);
        }

        public void Register()
        {
            var Container = new ContainerBuilder();

            // Register infrastruce one by one
            Container.Register(config => new ApiClientPool(new ApiClientConfig { Name = _config.CurrentAccount, Password = _config.CurrentPassword })).As<IApiClientPool>().SingleInstance();
            Container.Register(config => new EventBus(new EventBusConfig { handler = _config.EventHandler })).As<IEventBus>().SingleInstance();
            Container.Register(config => new ThreadPool()).As<IThreadPool>().SingleInstance();
            Container.Register(config => new LoggerManager(new LoggerConfig { Level = (LogLevel)_config.DefaultLogLevel, FilePath = _config.CurrentPath + _config.LogLPath })).As<ILoggerManager>().SingleInstance();
            Container.Register(config => new SQLiter(new SQLiteConfig { DBPath = _config.CurrentPath, DBName = _config.DefaultLDBName })).As<ISQLiter>().SingleInstance();

            // Reigseter by Assembly
            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly asm in asms)
            {
                //Container.RegisterAssemblyTypes(asm).Where(t => t.Name.EndsWith("Command"));
                Type[] types = asm.GetTypes();

                foreach (var type in types)
                {
                    var attr = (AutofacAttribute)type.GetCustomAttribute(typeof(AutofacAttribute), false);
                    if (attr != null && attr.Allow)
                    {
                        var interfaceDefault = type.GetInterfaces().FirstOrDefault();
                        if (interfaceDefault != null)
                        {
                            Container.RegisterType(type).Named(type.Name, interfaceDefault).As(interfaceDefault);
                        }
                    }
                }
            }

            _container = Container.Build();
        }
    }
}
