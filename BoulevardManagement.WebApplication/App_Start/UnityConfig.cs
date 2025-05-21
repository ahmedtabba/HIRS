using BoulevardManagement.BLL;
using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.Model.Entities;
using BoulevardManagement.WebApplication.Controllers;
using BoulevardManagement.WebApplication.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.Practices.Unity;
using Repository.Pattern;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;
using BoulevardManagement.Model.Migrations;
using Quartz.Spi;
using Quartz;
using BoulevardManagement.WebApplication.Helper.Quartz;
using BoulevardManagement.WebApplication.Helper;

namespace BoulevardManagement.WebApplication.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here

            container.RegisterSingleton<IJobFactory>(new InjectionFactory(c => new UnityJobFactory(container)));
            container.RegisterType<ISchedulerFactory, UnitySchedulerFactory>();

            container
                    .RegisterType<IApplicationUserDataContext, ApplicationContext>()
                   .RegisterType<IDataContextAsync, FlavourManagementDBContext>(new PerRequestOrTransientLifeTimeManager())
                    .RegisterType<IUnitOfWorkAsync, UnitOfWork>(new PerRequestOrTransientLifeTimeManager())
                    .RegisterType(typeof(IRepositoryAsync<>), typeof(Repository<>))
                    .RegisterType<IBoulevardAttachmentBLL, BoulevardAttachmentBLL>()
                    .RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>()
                    .RegisterType<AccountController>(new InjectionConstructor())
                    .RegisterType<IErrorLogBLL, ErrorLogBLL>()
                    .RegisterType<IProjectBLL, ProjectBLL>()
                    .RegisterType<IDepartmentTestBLL, DepartmentTestBLL>()
                    .RegisterType<IOperationLogBLL, OperationLogBLL>()
                    .RegisterType<IPatientBLL, PatientBLL>()
                     .RegisterType<INotificationBLL, NotificationBLL>()
                    .RegisterType<IUserNotificationBLL, UserNotificationBLL>()
                    .RegisterType<INotificationGroupBLL, NotificationGroupBLL>()
                    .RegisterType<IUserNotificationGroupsBLL, UserNotificationGroupsBLL>()
                    .RegisterType<IDepartmentBLL, DepartmentBLL>()
                    .RegisterType<ILocationBLL, LocationBLL>()
                    .RegisterType<IUserQuickActionBLL, UserQuickActionBLL>()
                    .RegisterType<ITeleMentalHealthBLL, TeleMentalHealthBLL>()
                    .RegisterType<ITelMHDiagnosisCategoryBLL, TelMHDiagnosisCategoryBLL>()
                    .RegisterType<ITelMHDiagnosisSubCategoryBLL, TelMHDiagnosisSubCategoryBLL>()
                    .RegisterType<IMedicationBLL, MedicationBLL>()
                    .RegisterType<IStickyNoteBLL, StickyNoteBLL>()
                    .RegisterType<ITeleICUBLL, TeleICUBLL>()
                    .RegisterType<INPICUBLL, NPICUBLL>()
                    .RegisterType<ITelMHDiagnosisBLL, TelMHDiagnosisBLL>()
                    .RegisterType<ITelMHMostLikelyDiagnosisBLL, TelMHMostLikelyDiagnosisBLL>()
                    .RegisterType<ICaseClosureBLL, CaseClosureBLL>()

                    .RegisterType<ManageController>(new InjectionConstructor());
            //.RegisterType<UsersAdminController>(new InjectionConstructor());

            Database.SetInitializer<BoulevardManagementContext>(new MigrateDatabaseToLatestVersion<BoulevardManagementContext, MigrateConfiguration>());



            var dbMigrator = new DbMigrator(new MigrateConfiguration());
            dbMigrator.Update();


        }
    }
}
