using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class PICUConsultationForm:Entity
    {
        public int NPICUId { get; set; }
        public NPICU NPICU { get; set; }

        public string MainComplaint { get; set; }
        public string HistoryOfPresentIllness { get; set; }

        public int? PICUConsultationFormAttachmentId { get; set; }

        #region Past Medical History
        public string PregnancyAndDeliveryHistory { get; set; }
        public string MedicalAndSurgicalHistory { get; set; }
        public string Allergy { get; set; }
        public string Immunization { get; set; }
        public string PsychomotorDevelopment { get; set; }
        public string NutritionalHistory { get; set; }
        public string FamilyMedicalAndPsychosocialHistory { get; set; }
        #endregion


        #region Physical Examination
        public int? Weight { get; set; }
        public int? Length { get; set; }
        public decimal? HeadCircumference { get; set; }
        public int? PhysicalExaminationAttachmentId { get; set; }


        #endregion



        #region Vital Signs
        public int? Temp { get; set; }
        public int? HR { get; set; }
        public int? RR { get; set; }
        public int? BPFirstPart { get; set; }
        public int? BPSecondPart { get; set; }
        public decimal? Map { get; set; }
        public int? SpO2 { get; set; }
        public int? RBS { get; set; }

        #endregion


        #region General Appearance
        //Enum
        public int? GeneralAppearance1 { get; set; }
        public int? GeneralAppearance2 { get; set; }
        #endregion

        public string CentralNervousSystem { get; set; }
        public string CardiovascularSystem { get; set; }
        public string RespiratorySystem { get; set; }
        public string GastrointestinalTract { get; set; }
        public string GenitourinarySystem { get; set; }
        public string HaematologicalSystem { get; set; }
        public string Others { get; set; }
        public string PrimaryDiagnosisOrDifferentialDiagnosis { get; set; }
        public string Procedures { get; set; }
        public string PlanOfCare { get; set; }
        public string CommunicationWithFamily { get; set; }

    }
}
