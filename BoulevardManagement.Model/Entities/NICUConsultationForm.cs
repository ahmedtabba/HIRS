using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class NICUConsultationForm:Entity
    {
        public int NPICUId { get; set; }
        public NPICU NPICU { get; set; }

        public string GestationalAge { get; set; }
        public string ChronologicalAge { get; set; }
        public string AgeAtAdmission { get; set; }
        public int? BirthWeight { get; set; }
        public int BornPlace { get; set; }
        public string ReasonOfAdmission { get; set; }
        public string CurrentIllnessHistory { get; set; }

        #region Pregnancy and Delivery history

        public int? MotherAge { get; set; }
        public int? Gravida { get; set; }
        public int? Parity { get; set; }
        public int? Abortion { get; set; }
        public string CurrentPregnancyRelatedMedicalHistory { get; set; }
        //Enum
        public int LabourAndDelivery { get; set; }
        public string LabourAndDeliveryRemarks { get; set; }
        //Multi Choise Enum 
        public int Resuscitation { get; set; }
        public string ResuscitationRemarks { get; set; }
        public int? ApgarScoreFirstMin { get; set; }
        public int? ApgarScoreFifthMin { get; set; }
        public int? ApgarScoreTenthMin { get; set; }
        public string ChronicDiseasesAndPreviousObstetricalHistory { get; set; }
        public string FamilyHistory { get; set; }
        public string PsychosocialHistory { get; set; }
        public int? PhysicalExaminationAttachmentId { get; set; }
        public int? NICUConsultationFormAttachmentId { get; set; }


        #endregion


        #region Physical Examination
        public int? Weight { get; set; }
        public int? Lenghth { get; set; }
        public decimal? HeadCircumference { get; set; }

        #endregion



        #region Vital Signs
        public decimal? Temp { get; set; }
        public int? HR { get; set; }
        public int? RR { get; set; }
        public int? BPFirstPart { get; set; }
        public int? BPSecondPart { get; set; }
        public decimal? Map { get; set; }
        public int? SpO2 { get; set; }
        public int? RBS { get; set; }

        #endregion


        #region General
        public string Color { get; set; }
        public string Posture { get; set; }
        public string Skin { get; set; }
        public string Remarks { get; set; }
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

        #endregion
    }
}
