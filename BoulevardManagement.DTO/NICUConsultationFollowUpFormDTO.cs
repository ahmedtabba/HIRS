using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class NICUConsultationFollowUpFormDTO:EntityDTO
    {
        public int NPICUId { get; set; }
        #region From System
        public string GestationalAge { get; set; }
        public string ChronologicalAge { get; set; }
        public int? BirthWeight { get; set; } 
        #endregion
        public int? CurrentWeight { get; set; }
        public string ProblemList { get; set; }
        public string MajorEventsInTheLast24Hours { get; set; }

        public int? NICUConsultationFollowUpFormAttachmentId { get; set; }
        public bool HasNICUConsultationFollowUpFormAttachment { get; set; }
        public string NICUConsultationFollowUpFormFilePath { get; set; }
        public string NICUConsultationFollowUpFormFilename { get; set; }

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
        public string Procedures { get; set; }
        public string PlanOfCare { get; set; }
        public string CommunicationWithFamily { get; set; }

        #endregion

        public DateTime DateOfCreate { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public CaseStatus CaseStatus { get; set; }
    }
}
