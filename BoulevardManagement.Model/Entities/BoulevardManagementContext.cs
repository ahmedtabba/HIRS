using Repository.Pattern;
using Repository.Pattern.Ef6;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using BoulevardManagement.Model.Migrations;

namespace BoulevardManagement.Model.Entities
{
    public partial class BoulevardManagementContext : DataContext
    {
        public BoulevardManagementContext()
            : base("Name=BoulevardManagementContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BoulevardManagementContext, MigrateConfiguration>("BoulevardManagementContext"));
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;

        }
        public BoulevardManagementContext(IApplicationUserDataContext context) : base("Name=BoulevardManagementContext", context)
        {
            //SetInitializer();
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            //this.SetCommandTimeOut(500);
        }

        public void SetCommandTimeOut(int Timeout)
        {
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = Timeout;
        }


        public DbSet<BoulevardAttachment> BoulevardAttachments { get; set; }

        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<OperationLog> OperationLogs { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<DepartmentTest> DepartmentTests { get; set; }
        public DbSet<EmployeeTest> EmployeeTests { get; set; }
        public DbSet<NotificationGroup> NotificationGroups { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<UserNotificationGroups> UserNotificationGroups { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<UserQuickAction> UserQuickActions { get; set; }
        public DbSet<TeleMentalHealth> TelementalHealths { get; set; }
        public DbSet<TelMHClinicalStory> TelMHClinicalStories { get; set; }
        public DbSet<TelMHWrittenPledge> TelMHWrittenPledges { get; set; }
        public DbSet<TelMHPhysicalExaminationReport> TelMHPhysicalExaminationReports { get; set; }
        public DbSet<TelMHTherapeuticPlan> TelMHTherapeuticPlans { get; set; }
        public DbSet<TelMHVitalSign> TelMHVitalSigns { get; set; }
        public DbSet<TelMHDiagnosisCategory> TelMHDiagnosisCategories { get; set; }
        public DbSet<TelMHDiagnosisSubCategory> TelMHDiagnosisSubCategories { get; set; }
        public DbSet<TelMHDiagnosis> TelMHDiagnoses { get; set; }
        public DbSet<Medication> MostLikelyDiagnoses { get; set; }
        public DbSet<TelMHMostLikelyDiagnosis> TelMHMostLikelyDiagnoses { get; set; }
        public DbSet<StickyNote> StickyNotes { get; set; }
        public DbSet<TeleICU> TeleICUs { get; set; }
        public DbSet<TelICUClinicalStory> TelICUClinicalStories { get; set; }
        public DbSet<TelICUMedicationScheduler> TelICUMedicationSchedulers { get; set; }
        public DbSet<TelICUDiabetesControl> TelICUDiabetesControls { get; set; }
        public DbSet<NPICU> NPICUs { get; set; }
        public DbSet<NPICUUser> NPICUUsers { get; set; }
        public DbSet<NICUConsultationForm> NICUConsultationForms { get; set; }
        public DbSet<NICUConsultationFollowUpForm> NICUConsultationFollowUpForms { get; set; }
        public DbSet<NPICUInvestigation> NPICUInvestigations { get; set; }
        public DbSet<TelICUVitalSign> TelICUVitalSigns { get; set; }
        public DbSet<TelICUPump> TelICUPumps { get; set; }
        public DbSet<TelICUMedicationDailySchedule> TelICUMedicationDailySchedules { get; set; }
        public DbSet<TelICULabUnit> TelICULabUnits { get; set; }
        public DbSet<TelICUInternalConsultationForm> TelICUInternalConsultationForms { get; set; }
        public DbSet<TelICUExit> TelICUExits { get; set; }
        public DbSet<NPICUConsultantSection> NPICUConsultantSections { get; set; }
        public DbSet<PICUConsultationForm> PICUConsultationForms { get; set; }
        public DbSet<PICUConsultationFollowUpForm> PICUConsultationFollowUpForms { get; set; }
        public DbSet<TelICUConsultationForm> TelICUConsultationForms { get; set; }
        public DbSet<Medication_MHTherapeuticPlan> Medication_MHTherapeuticPlans { get; set; }
        public DbSet<CaseClosure> CaseClosures { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);



            #region Notification
            modelBuilder.Entity<NotificationGroup>()
             .HasMany(m => m.GroupNotifications)
             .WithMany(c => c.Groups)
             .Map(m =>//Map to another table
             {
                 m.MapLeftKey("NotificationGroupId");
                 m.MapRightKey("NotificationId");
                 m.ToTable("GroupNotifications");
             });
            #endregion


        }


    }
}