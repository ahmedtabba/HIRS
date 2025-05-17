using System;
using System.Data.Entity.Migrations;
using System.Linq;
using BoulevardManagement.Model.Common;
using BoulevardManagement.Model.Entities;

namespace BoulevardManagement.Model.Migrations
{
    public sealed class MigrateConfiguration : DbMigrationsConfiguration<BoulevardManagementContext>
    {
        public MigrateConfiguration()

        {

            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

        }

        protected override void Seed(BoulevardManagementContext context)
        {
            base.Seed(context);
            var notifications = context.Notifications.ToList();
            #region UserGroup

            if (!notifications.Exists(r => r.Name == NotificationConsistent.UserGroup.Add))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.UserGroup.Add, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.UserGroup.Edit))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.UserGroup.Edit, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.UserGroup.Delete))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.UserGroup.Delete, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            #endregion


            #region UserManagement

            if (!notifications.Exists(r => r.Name == NotificationConsistent.UserManagement.Add))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.UserManagement.Add, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.UserManagement.Edit))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.UserManagement.Edit, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            //if (!notifications.Exists(r => r.Name == NotificationConsistent.UserManagement.Delete))
            //    context.Notifications.Add(new Notification { Name = NotificationConsistent.UserManagement.Delete, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.UserManagement.ChangePasswordForUser))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.UserManagement.ChangePasswordForUser, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.UserManagement.ChangePermitionGroupForUser))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.UserManagement.ChangePermitionGroupForUser, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            #endregion

            #region NotificationGroup

            if (!notifications.Exists(r => r.Name == NotificationConsistent.NotificationGroup.Add))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NotificationGroup.Add, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.NotificationGroup.Edit))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NotificationGroup.Edit, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.NotificationGroup.Delete))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NotificationGroup.Delete, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            #endregion

            #region Patient

            if (!notifications.Exists(r => r.Name == NotificationConsistent.Patient.Add))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.Patient.Add, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.Patient.Edit))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.Patient.Edit, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.Patient.Delete))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.Patient.Delete, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            #endregion

            #region Zoom

            if (!notifications.Exists(r => r.Name == NotificationConsistent.Zoom.JoinMeeting))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.Zoom.JoinMeeting, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            #endregion

            #region TeleMentalHealth

            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleMentalHealth.Add))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleMentalHealth.Add, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleMentalHealth.Edit))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleMentalHealth.Edit, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleMentalHealth.Close))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleMentalHealth.Close, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleMentalHealth.ReOpen))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleMentalHealth.ReOpen, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });


            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleMentalHealth.Delete))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleMentalHealth.Delete, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleMentalHealth.UpdateCllinicalStory))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleMentalHealth.UpdateCllinicalStory, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleMentalHealth.UpdatePhysicalExaminationReport))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleMentalHealth.UpdatePhysicalExaminationReport, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleMentalHealth.UpdateTherapeuticPlan))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleMentalHealth.UpdateTherapeuticPlan, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleMentalHealth.UpdateWrittenPledge))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleMentalHealth.UpdateWrittenPledge, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleMentalHealth.AddVitalSign))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleMentalHealth.AddVitalSign, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleMentalHealth.EditVitalSign))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleMentalHealth.EditVitalSign, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleMentalHealth.DeleteVitalSign))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleMentalHealth.DeleteVitalSign, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleMentalHealth.ChatOnMentalHealth))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleMentalHealth.ChatOnMentalHealth, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            #endregion


            #region NPICU

            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.Add))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.Add, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.Edit))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.Edit, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });


            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.Close))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.Close, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.ReOpen))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.ReOpen, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });



            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.UpdateNICUConsultationForm))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.UpdateNICUConsultationForm, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
           
            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.UpdateNICUConsultationFollowUpForm))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.UpdateNICUConsultationFollowUpForm, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.AddNICUConsultationFollowUpForm))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.AddNICUConsultationFollowUpForm, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.UpdatePICUConsultationFollowUpForm))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.UpdatePICUConsultationFollowUpForm, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.AddPICUConsultationFollowUpForm))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.AddPICUConsultationFollowUpForm, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.DeleteNICUConsultationFollowUpForm))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.DeleteNICUConsultationFollowUpForm, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.DeletePICUConsultationFollowUpForm))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.DeletePICUConsultationFollowUpForm, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.AddNPICUInvestigation))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.AddNPICUInvestigation, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.UpdateNPICUInvestigation))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.UpdateNPICUInvestigation, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });


            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.AddNPICUConsultantSection))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.AddNPICUConsultantSection, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.UpdateNPICUConsultantSection))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.UpdateNPICUConsultantSection, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });


            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.UpdatePICUConsultationForm))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.UpdatePICUConsultationForm, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
           
            if (!notifications.Exists(r => r.Name == NotificationConsistent.NPICU.ChatOnNPICU))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.NPICU.ChatOnNPICU, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });


            #endregion


            #region ICU
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcu.Add))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcu.Add, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcu.Edit))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcu.Edit, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            
            
            
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcu.Close))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcu.Close, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });



            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcu.ReOpen))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcu.ReOpen, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            
            
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcu.Delete))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcu.Delete, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcu.UpdateCllinicalStory))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcu.UpdateCllinicalStory, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcu.UpdateMedicationDailyScheduleTable))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcu.UpdateMedicationDailyScheduleTable, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcu.UpdateLabUnit))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcu.UpdateLabUnit, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcu.UpdateInternalConsultationForm))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcu.UpdateInternalConsultationForm, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcu.UpdateConsultationForm))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcu.UpdateConsultationForm, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcu.UpdatePatientExitStatusReport))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcu.UpdatePatientExitStatusReport, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcu.ChatOnICU))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcu.ChatOnICU, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcuMedicationScheduler.AddMedicationScheduler))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcuMedicationScheduler.AddMedicationScheduler, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcuMedicationScheduler.EditMedicationScheduler))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcuMedicationScheduler.EditMedicationScheduler, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcuMedicationScheduler.DeleteMedicationScheduler))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcuMedicationScheduler.DeleteMedicationScheduler, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });


            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcuDiabetesControl.AddDiabetesControl))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcuDiabetesControl.AddDiabetesControl, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcuDiabetesControl.EditDiabetesControl))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcuDiabetesControl.EditDiabetesControl, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcuDiabetesControl.DeleteDiabetesControl))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcuDiabetesControl.DeleteDiabetesControl, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcuVitalSign.AddVitalSign))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcuVitalSign.AddVitalSign, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcuVitalSign.EditVitalSign))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcuVitalSign.EditVitalSign, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcuVitalSign.DeleteVitalSign))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcuVitalSign.DeleteVitalSign, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });

            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcuPump.AddPump))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcuPump.AddPump, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcuPump.EditPump))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcuPump.EditPump, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            if (!notifications.Exists(r => r.Name == NotificationConsistent.TeleIcuPump.DeletePump))
                context.Notifications.Add(new Notification { Name = NotificationConsistent.TeleIcuPump.DeletePump, ObjectState = Repository.Pattern.Infrastructure.ObjectState.Added, CreatedBy = "0", CreationDate = DateTime.Now });
            



            #endregion
            context.SaveChanges();

        }

    }

}
