//using ArchPM.Core.Enums;
//using ArchPM.Data;
//using ArchPM.Web.Core;
//using ArchPM.Web.Core.Managers;
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Web;
//using Unity;
//using Unity.Lifetime;

//namespace AutoBitBot.Infrastructure
//{
//    public class Kernel : IObjectContainer
//    {

//        public static readonly Kernel Instance = new Kernel();
//        Boolean initialized = false;
//        IUnityContainer container;

//        private Kernel()
//        {
//            String env = ConfigurationManager.AppSettings.Get("Kernel.Environment");
//            if (String.IsNullOrEmpty(env))
//                this.Environment = ApplicationEnvironments.Test;
//            else
//                this.Environment = EnumManager<ApplicationEnvironments>.Parse(env);
//        }

//        public ApplicationEnvironments Environment { get; set; }

//        public void Initialize()
//        {
//            if (initialized)
//                return;

//            this.container = new UnityContainer();

//            //register modules
//            RegisterModuleProviders();
//            initialized = true;
//        }


//        /// <summary>
//        /// Register Providers from Assemblies
//        /// </summary>
//        /// <returns></returns>
//        void RegisterModuleProviders()
//        {
//            //switch (this.Environment)
//            //{
//            //    case ApplicationEnvironments.Development:
//            //        //contexts
//            //        this.container.RegisterType<IDbContext, SisliAdoContext>(DbContextDatabases.MIS.ToString(), new InjectionConstructor("ConnectionString_dev"));
//            //        this.container.RegisterType<IDbContext, SisliAdoContext>(DbContextDatabases.ChangeLog.ToString(), new InjectionConstructor("ChangeLogConnectionString_dev"));
//            //        this.container.RegisterType<IDbContext, SisliAdoContext>(DbContextDatabases.YonsisDb.ToString(), new InjectionConstructor("YonsisDbConnectionString"));
//            //        this.container.RegisterType<IDbContext, SisliAdoContext>(DbContextDatabases.GIS.ToString(), new InjectionConstructor("GISDbConnectionString"));
//            //        break;

//            //    case ApplicationEnvironments.Test:
//            //    default:
//            //        //contexts
//            //        this.container.RegisterType<IDbContext, SisliAdoContext>(DbContextDatabases.MIS.ToString(), new InjectionConstructor("ConnectionString_test"));
//            //        this.container.RegisterType<IDbContext, SisliAdoContext>(DbContextDatabases.ChangeLog.ToString(), new InjectionConstructor("ChangeLogConnectionString_test"));
//            //        this.container.RegisterType<IDbContext, SisliAdoContext>(DbContextDatabases.YonsisDb.ToString(), new InjectionConstructor("YonsisDbConnectionString"));
//            //        this.container.RegisterType<IDbContext, SisliAdoContext>(DbContextDatabases.GIS.ToString(), new InjectionConstructor("GISDbConnectionString"));
//            //        break;

//            //    case ApplicationEnvironments.Production:
//            //        //contexts
//            //        this.container.RegisterType<IDbContext, SisliAdoContext>(DbContextDatabases.MIS.ToString(), new InjectionConstructor("ConnectionString"));
//            //        this.container.RegisterType<IDbContext, SisliAdoContext>(DbContextDatabases.ChangeLog.ToString(), new InjectionConstructor("ChangeLogConnectionString"));
//            //        this.container.RegisterType<IDbContext, SisliAdoContext>(DbContextDatabases.YonsisDb.ToString(), new InjectionConstructor("YonsisDbConnectionString"));
//            //        this.container.RegisterType<IDbContext, SisliAdoContext>(DbContextDatabases.GIS.ToString(), new InjectionConstructor("GISDbConnectionString"));
//            //        break;
//            //}

//            //session
//            this.container.RegisterType<ISessionProvider, SessionProvider>();

//            ////Mernis
//            //this.container.RegisterType<IMernisServiceAdaptor, MernisServiceAdaptor>(); //degistirmeyi unutma
//            //this.container.RegisterType<IMernisServiceBusiness, MernisServiceBusiness>();

//            ////YonSis
//            //this.container.RegisterType<IYonsisDbServiceAdaptor, YonsisDbServiceAdaptor>();
//            //this.container.RegisterType<IYonsisDbServiceBusiness, YonsisDbServiceBusiness>();

//            ////GIS
//            //this.container.RegisterType<IGISDbServiceAdaptor, GISDbServiceAdaptor>();
//            //this.container.RegisterType<IGISDbServiceBusiness, GISDbServiceBusiness>();

//            ////active directory
//            //this.container.RegisterType<IActiveDirectoryAdaptor, SisliActiveDirectoryAdaptor>();
//            //this.container.RegisterType<IActiveDirectoryBusiness, ActiveDirectoryBusiness>();

//            ////common
//            //this.container.RegisterType<IDashboardBusiness, DashboardBusiness>();
//            //this.container.RegisterType<IChangeLogManager, HttpChangeLogManager>();
//            //this.container.RegisterType<IChangeLogService, ChangeLogServiceV2>();
//            //this.container.RegisterInstance<IObjectContainer>(Instance);

//            ////business
//            ////this.container.RegisterType(typeof(IBusiness<>), typeof(CrudBusiness<>));
//            //this.container.RegisterType<IExportBusiness, ExportBusiness>();
//            //this.container.RegisterType<ISosyalBasvuruBusiness, BasvuruBusiness>();
//            //this.container.RegisterType<IAskerBasvuruBusiness, BasvuruBusiness>();
//            //this.container.RegisterType<IBasvuruBusiness, BasvuruBusiness>();
//            //this.container.RegisterType<IIhbarBasvuruBusiness, SosyalIhbarBusiness>();
//            //this.container.RegisterType<ICommonDataBusiness, CommonDataBusiness>();
//            //this.container.RegisterType<IKararBusiness, KararBusiness>();
//            //this.container.RegisterType<IKurulBusiness, KurulBusiness>();

//            ////notificationn Services
//            //this.container.RegisterType<INotificationService, SmtpMailNotificationService>("smtp");
//            //this.container.RegisterType<INotificationService, MsmqNotificationService>("msmq");
//        }

//        void unregister<T>()
//        {

//            //if (this.container != null && this.container.IsRegistered<T>())
//            //{
//            //    foreach (var registration in this.container.Registrations
//            //    .Where(p => p.RegisteredType == typeof(T)
//            //        && p.LifetimeManagerType == typeof(ContainerControlledLifetimeManager)))
//            //    {
//            //        registration.LifetimeManager.RemoveValue();
//            //    }
//            //}
//        }

//        public void RegisterTypeAgain<T, U>() where U : T
//        {
//            unregister<T>();

//            this.container.RegisterType<T, U>();
//        }

//        public T Resolve<T>(string name = "")
//        {
//            //try
//            //{
//                var rep = this.container.Resolve<T>(name);
//                return rep;
//            //}
//            //catch (Exception ex)
//            //{
//            //    //EventLogFactory.Instance.Log(ex, "Bootstrapper");

//            //    //throw new FatalException(String.Format("''{0}'' not Resolved! ExtensionName:{1}", typeof(T).Name, name), ex);
//            //}
//        }

//        public IEnumerable<T> ResolveAll<T>()
//        {
//            var rep = this.container.ResolveAll<T>();
//            return rep;
//        }

//        //public void ErrorFreeChangeLog(Exception ex) //tugbay: burada olmamali. sonra yerini degistirelim
//        //{
//        //    try //fistan
//        //    {
//        //        IChangeLogService changeLogService = Bootstrapper.Instance.Resolve<IChangeLogService>();
//        //        IDbContext dbContext = Bootstrapper.Instance.Resolve<IDbContext>(DbContextDatabases.ChangeLog.ToString());
//        //        ISessionProvider sessionProvider = Bootstrapper.Instance.Resolve<ISessionProvider>();
//        //        //hata source olarak UI gonderiyoruz. tekrar exception throw etmeyecek sekilde log aliniyor
//        //        changeLogService.ChangeLog(ex, dbContext, sessionProvider, ChangeLogTypes.Exception, ChangeLogSources.ChangeLogItself);
//        //    }
//        //    catch (Exception iex) { }
//        //}

//        //public void ChangeLog<T>(T entity, ChangeLogTypes logType, ChangeLogSources logSource)
//        //{
//        //    IChangeLogService bus = Bootstrapper.Instance.Resolve<IChangeLogService>();
//        //    IDbContext dbContext = Bootstrapper.Instance.Resolve<IDbContext>(DbContextDatabases.ChangeLog.ToString());
//        //    ISessionProvider sessionProvider = Bootstrapper.Instance.Resolve<ISessionProvider>();

//        //    bus.ChangeLog(sessionProvider.AuthUser, dbContext, sessionProvider, logType, logSource);
//        //}



//    }
//}