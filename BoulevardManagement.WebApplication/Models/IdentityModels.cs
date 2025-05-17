using BoulevardManagement.DTO;
using BoulevardManagement.Model.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BoulevardManagement.WebApplication.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Groups = new List<ApplicationUserGroup>();
            NotificationGroups = new List<UserNotificationGroup>();
        }

        public string FullName { get; set; }

        public bool HasPhoto { get; set; }

        public virtual ICollection<ApplicationUserGroup> Groups { get; set; }
        public ICollection<UserNotificationGroup> NotificationGroups { get; set; }

        public JobRole JobRole { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? CreationDate { get; set; }
        public Gender Gender { get; set; }
        public string Descreption { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("FullName", FullName));
            userIdentity.AddClaim(new Claim("JobRole", JobRole.ToString()));
            userIdentity.AddClaim(new Claim("Email", Email));
            userIdentity.AddClaim(new Claim("Id", Id));
            userIdentity.AddClaim(new Claim("HasPhoto", HasPhoto.ToString()));
            userIdentity.AddClaim(new Claim("DepartmentId", DepartmentId.ToString()));
            userIdentity.AddClaim(new Claim("CreationDate", CreationDate.HasValue? CreationDate.ToString():""));
            userIdentity.AddClaim(new Claim("Gender", Gender.ToString()));
            userIdentity.AddClaim(new Claim("Descreption", (this.Descreption!=null?this.Descreption:"")));
            return userIdentity;
        }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }




    public class UserNotificationGroup
    {
        public UserNotificationGroup()
        {
            Id = Guid.NewGuid().ToString();
        }
        public String Id { get; set; }

        public int NotificationGroup { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }



    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {

        }

        /// <summary>
        /// That description field used as friendly name 
        /// </summary>
        public string Description { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
    public sealed class IdentityMigrateConfiguration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public IdentityMigrateConfiguration()
        {

            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }


        protected override void Seed(ApplicationDbContext context)
        {
            var roles = context.Roles.ToListAsync().Result;


            #region Patient
            if (!roles.Exists(r => r.Name == RoleConsistent.Patient.Browse))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.Patient.Browse });

            if (!roles.Exists(r => r.Name == RoleConsistent.Patient.Add))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.Patient.Add });

            if (!roles.Exists(r => r.Name == RoleConsistent.Patient.Edit))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.Patient.Edit });

            if (!roles.Exists(r => r.Name == RoleConsistent.Patient.Delete))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.Patient.Delete });

            #endregion

            #region Notification Group
            if (!roles.Exists(r => r.Name == RoleConsistent.NotificationGroup.Add))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NotificationGroup.Add });

           
            if (!roles.Exists(r => r.Name == RoleConsistent.NotificationGroup.Edit))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NotificationGroup.Edit });

            if (!roles.Exists(r => r.Name == RoleConsistent.NotificationGroup.Delete))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NotificationGroup.Delete });

            if (!roles.Exists(r => r.Name == RoleConsistent.NotificationGroup.Browse))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NotificationGroup.Browse });
            #endregion

            #region UserGroup
            if (!roles.Exists(r => r.Name == RoleConsistent.UserGroup.Add))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.UserGroup.Add });

            if (!roles.Exists(r => r.Name == RoleConsistent.UserGroup.Edit))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.UserGroup.Edit });

            if (!roles.Exists(r => r.Name == RoleConsistent.UserGroup.Delete))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.UserGroup.Delete });

            if (!roles.Exists(r => r.Name == RoleConsistent.UserGroup.Browse))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.UserGroup.Browse });

            //if (!roles.Exists(r => r.Name == RoleConsistent.UserGroup.Manage))
            //    context.Roles.Add(new ApplicationRole { Name = RoleConsistent.UserGroup.Manage });
            #endregion

            #region UserManagement
            if (!roles.Exists(r => r.Name == RoleConsistent.UserManagement.Add))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.UserManagement.Add });

            if (!roles.Exists(r => r.Name == RoleConsistent.UserManagement.Edit))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.UserManagement.Edit });

            if (!roles.Exists(r => r.Name == RoleConsistent.UserManagement.Browse))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.UserManagement.Browse });

            //if (!roles.Exists(r => r.Name == RoleConsistent.UserManagement.UserLog))
            //    context.Roles.Add(new ApplicationRole { Name = RoleConsistent.UserManagement.UserLog });
            #endregion


            #region Location
            if (!roles.Exists(r => r.Name == RoleConsistent.Location.Browse))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.Location.Browse });

            if (!roles.Exists(r => r.Name == RoleConsistent.Location.Add))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.Location.Add });

            if (!roles.Exists(r => r.Name == RoleConsistent.Location.Edit))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.Location.Edit });

            if (!roles.Exists(r => r.Name == RoleConsistent.Location.Delete))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.Location.Delete });

            #endregion


            #region TeleMentalHealth
            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.Browse))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.Browse });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.Add))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.Add });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.Edit))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.Edit });
            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.EditSettings))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.EditSettings });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.Close))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.Close });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.ReOpen))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.ReOpen });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.Delete))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.Delete });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.EditClinicalStory))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.EditClinicalStory });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.EditMonitoringTheVitalSigns))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.EditMonitoringTheVitalSigns });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.EditPhysicalExaminationReport))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.EditPhysicalExaminationReport });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.EditTherapeuticPlan))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.EditTherapeuticPlan });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.EditWrittenPledge))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.EditWrittenPledge });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.ViewClinicalStory))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.ViewClinicalStory });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.ViewMonitoringTheVitalSigns))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.ViewMonitoringTheVitalSigns });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.ViewPhysicalExaminationReport))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.ViewPhysicalExaminationReport });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.ViewTherapeuticPlan))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.ViewTherapeuticPlan });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.ViewWrittenPledge))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.ViewWrittenPledge });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleMentalHealth.ViewChat))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleMentalHealth.ViewChat });

            #endregion

            #region StickyNote
            if (!roles.Exists(r => r.Name == RoleConsistent.StickyNote.Record))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.StickyNote.Record });
            if (!roles.Exists(r => r.Name == RoleConsistent.StickyNote.Delete))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.StickyNote.Delete });
            if (!roles.Exists(r => r.Name == RoleConsistent.StickyNote.DownloadRecord))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.StickyNote.DownloadRecord });

            #endregion

         


            #region NPICU
            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.Browse))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.Browse });

            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.Add))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.Add });

            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.Edit))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.Edit });
            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.EditSettings))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.EditSettings });

            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.Close))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.Close });

            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.ReOpen))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.ReOpen });

            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.Delete))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.Delete });



            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.ViewNICUConsultationForm))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.ViewNICUConsultationForm });

            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.EditNICUConsultationForm))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.EditNICUConsultationForm });

            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.ViewPICUConsultationForm))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.ViewPICUConsultationForm });

            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.EditPICUConsultationForm))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.EditPICUConsultationForm });

            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.ViewNICUConsultationFollowUpForm))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.ViewNICUConsultationFollowUpForm });

            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.EditNICUConsultationFollowUpForm))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.EditNICUConsultationFollowUpForm });

            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.ViewPICUConsultationFollowUpForm))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.ViewPICUConsultationFollowUpForm });

            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.EditPICUConsultationFollowUpForm))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.EditPICUConsultationFollowUpForm });


            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.EditNPICUInvestigation))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.EditNPICUInvestigation });

            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.ViewNPICUInvestigation))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.ViewNPICUInvestigation });

            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.EditNPICUConsultantSection))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.EditNPICUConsultantSection });

            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.ViewNPICUConsultantSection))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.ViewNPICUConsultantSection });

            if (!roles.Exists(r => r.Name == RoleConsistent.NPICU.ViewChat))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.NPICU.ViewChat });

            #endregion

            #region TeleICU
            if (!roles.Exists(r => r.Name == RoleConsistent.TeleICU.Browse))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleICU.Browse });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleICU.Add))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleICU.Add });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleICU.Edit))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleICU.Edit });
            if (!roles.Exists(r => r.Name == RoleConsistent.TeleICU.EditSettings))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleICU.EditSettings });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleICU.Close))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleICU.Close });

            if (!roles.Exists(r => r.Name == RoleConsistent.TeleICU.ReOpen))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleICU.ReOpen });


            if (!roles.Exists(r => r.Name == RoleConsistent.TeleICU.Delete))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleICU.Delete });

            if (!roles.Exists(r => r.Name == RoleConsistent.TelIcuClinicalStory.EditClinicalStory))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TelIcuClinicalStory.EditClinicalStory });
            if (!roles.Exists(r => r.Name == RoleConsistent.TelIcuMedicationDailyScheduleTable.EditMedicationDailyScheduleTable))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TelIcuMedicationDailyScheduleTable.EditMedicationDailyScheduleTable });
            if (!roles.Exists(r => r.Name == RoleConsistent.TelIcuLabUnit.EditLabUnit))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TelIcuLabUnit.EditLabUnit });
            if (!roles.Exists(r => r.Name == RoleConsistent.TelIcuInternalConsultationForm.EditInternalConsultationForm))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TelIcuInternalConsultationForm.EditInternalConsultationForm });
            if (!roles.Exists(r => r.Name == RoleConsistent.TelIcuConsultationForm.EditConsultationForm))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TelIcuConsultationForm.EditConsultationForm });
            if (!roles.Exists(r => r.Name == RoleConsistent.TelIcuPatientExitStatusReport.EditPatientExitStatusReport))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TelIcuPatientExitStatusReport.EditPatientExitStatusReport });
            

            if (!roles.Exists(r => r.Name == RoleConsistent.TelIcuClinicalStory.ViewClinicalStory))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TelIcuClinicalStory.ViewClinicalStory });
            if (!roles.Exists(r => r.Name == RoleConsistent.TelIcuMedicationDailyScheduleTable.ViewMedicationDailyScheduleTable))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TelIcuMedicationDailyScheduleTable.ViewMedicationDailyScheduleTable });
            if (!roles.Exists(r => r.Name == RoleConsistent.TelIcuLabUnit.ViewLabUnit))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TelIcuLabUnit.ViewLabUnit });
            if (!roles.Exists(r => r.Name == RoleConsistent.TelIcuInternalConsultationForm.ViewInternalConsultationForm))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TelIcuInternalConsultationForm.ViewInternalConsultationForm });
            if (!roles.Exists(r => r.Name == RoleConsistent.TelIcuConsultationForm.ViewConsultationForm))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TelIcuConsultationForm.ViewConsultationForm });
            if (!roles.Exists(r => r.Name == RoleConsistent.TelIcuPatientExitStatusReport.ViewPatientExitStatusReport))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TelIcuPatientExitStatusReport.ViewPatientExitStatusReport });
            if (!roles.Exists(r => r.Name == RoleConsistent.TeleICU.ViewChat))
                context.Roles.Add(new ApplicationRole { Name = RoleConsistent.TeleICU.ViewChat });



            


            #endregion
        }
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual IDbSet<UserNotificationGroup> UserNotificationGroups { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, IdentityMigrateConfiguration>("DefaultConnection"));
            this.Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // Add the ApplicationGroups property:
        public virtual IDbSet<ApplicationGroup> ApplicationGroups { get; set; }

        // Override OnModelsCreating:
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationGroup>()
                .HasMany<ApplicationUserGroup>((ApplicationGroup g) => g.ApplicationUsers)
                .WithRequired().HasForeignKey<string>((ApplicationUserGroup ag) => ag.ApplicationGroupId);

            modelBuilder.Entity<ApplicationUserGroup>()
                .HasKey((ApplicationUserGroup r) =>
                    new
                    {
                        ApplicationUserId = r.ApplicationUserId,
                        ApplicationGroupId = r.ApplicationGroupId
                    }).ToTable("ApplicationUserGroups");

            modelBuilder.Entity<ApplicationGroup>()
                .HasMany<ApplicationGroupRole>((ApplicationGroup g) => g.ApplicationRoles)
                .WithRequired().HasForeignKey<string>((ApplicationGroupRole ap) => ap.ApplicationGroupId);

            modelBuilder.Entity<ApplicationGroupRole>().HasKey((ApplicationGroupRole gr) =>
                new
                {
                    ApplicationRoleId = gr.ApplicationRoleId,
                    ApplicationGroupId = gr.ApplicationGroupId
                }).ToTable("ApplicationGroupRoles");

        }
    }

    public class ApplicationGroup
    {
        public ApplicationGroup()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ApplicationRoles = new List<ApplicationGroupRole>();
            this.ApplicationUsers = new List<ApplicationUserGroup>();
        }

        public ApplicationGroup(string name)
            : this()
        {
            this.Name = name;
        }
         
        public ApplicationGroup(string name, string description)
            : this(name)
        {
            this.Description = description;
        }

        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ApplicationGroupRole> ApplicationRoles { get; set; }
        public virtual ICollection<ApplicationUserGroup> ApplicationUsers { get; set; }
    }

    public class ApplicationUserGroup
    {
        public string ApplicationUserId { get; set; }
        public string ApplicationGroupId { get; set; }
    }

    public class ApplicationGroupRole
    {
        public string ApplicationGroupId { get; set; }
        public string ApplicationRoleId { get; set; }
    }

    #region Seed Data of Users & Roles

    public static class IdentityDataSeeder
    {
        public static async void Create()
        {


            var dbcontext = new ApplicationDbContext();
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(dbcontext));
            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(dbcontext));
            var _groupManager = new ApplicationGroupManager(dbcontext,userManager,roleManager);




        ApplicationUser managerUser = new ApplicationUser
            {
                Email = BoulevardManagement.Utilities.Configurations.SuperUserEmail,
                UserName = BoulevardManagement.Utilities.Configurations.SuperUserEmail,
                FullName = BoulevardManagement.Utilities.Configurations.SuperUserFullName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PasswordHash = new Microsoft.AspNet.Identity.PasswordHasher().HashPassword("Boulevard"),
                JobRole=JobRole.IT
            };

            var roles = dbcontext.Roles.ToListAsync().Result;

            

            if (userManager.FindByEmail(managerUser.Email) == null)
            {
                await userManager.CreateAsync(managerUser, "Admin=1234");

                var managerUsercreated = userManager.FindByEmail(managerUser.Email);
                //userManager.AddToRole(managerUsercreated.Id, ZestRoles.Manager.ToString());

            }

            var manager = userManager.FindByEmail(managerUser.Email);
            if (!_groupManager.Groups.Any())
            {
                var group = await InitSuperAdminGroup(roles);
                _groupManager.SetUserGroups(manager.Id, (new List<string> { group.Id }).ToArray());
            }

            var context = new BoulevardManagementContext(new App_Start.ApplicationContext(new Repository.Pattern.ApplicationUserData() { Email = manager.Email, FullName = manager.FullName, UserId = manager.Id, UserName = manager.UserName }));
            if (!context.Departments.Where(x => x.Name == "Mental Health").Any())
            {
                context.Departments.Add(new Department
                {
                    Code = "01",
                    Name = "Mental Health",
                    ArabicName = "الصحة النفسية",
                    HaveLocation = false,
                    ObjectState = ObjectState.Added
                });
            }
            if (!context.Departments.Where(x => x.Name == "NPICU").Any())
            {
                context.Departments.Add(new Department
                {
                    Code = "02",
                    Name = "NPICU",
                    ArabicName = "الأطفال",
                    HaveLocation = false,
                    ObjectState = ObjectState.Added
                });
            }

           
            if (!context.Departments.Where(x => x.Name == "ICU").Any())
            {
                context.Departments.Add(new Department
                {
                    Code = "03",
                    Name = "ICU",
                    ArabicName = "وحدة العناية المركزة",
                    HaveLocation = true,
                    ObjectState = ObjectState.Added
                });
            }
            context.SaveChanges();
            async Task<ApplicationGroup> InitSuperAdminGroup(List<IdentityRole> rolesList)
            {
                ApplicationGroup superAdminPG = _groupManager.Groups.Where(x => x.Name == "Super Admin PG").FirstOrDefault();
                if (superAdminPG == null)
                {
                    superAdminPG = new ApplicationGroup
                    {
                        Name = "Super Admin PG"
                    };
                    var createGroup = await _groupManager.CreateGroupAsync(superAdminPG);

                    if (createGroup.Succeeded)
                    {
                        rolesList = dbcontext.Roles.ToListAsync().Result;
                        _groupManager.SetGroupRoles(superAdminPG.Id, rolesList.Select(s => s.Name).ToArray());
                    }
                }
                return superAdminPG;
            }
        }
    }
    #endregion
}