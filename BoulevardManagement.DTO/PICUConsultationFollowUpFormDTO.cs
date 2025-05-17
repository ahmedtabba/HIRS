using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class PICUConsultationFollowUpFormDTO:EntityDTO
    {
        public int NPICUId { get; set; }


        public string ProblemList { get; set; }
        public string MajorEventsInTheLast24Hours { get; set; }

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

        public int? Weight { get; set; }


        #region General Appearance
        //Enum
        public GeneralAppearance1? GeneralAppearance1 { get; set; }
        public GeneralAppearance2? GeneralAppearance2 { get; set; }
        #endregion

        #region General
        public string CentralNervousSystem { get; set; }
        public string CardiovascularSystem { get; set; }
        public string RespiratorySystem { get; set; }
        public string GastrointestinalTract { get; set; }
        public string GenitourinarySystem { get; set; }
        public string Others { get; set; }

        #endregion

        public int? PhysicalExaminationAttachmentId { get; set; }
        public bool HasPhysicalExaminationAttachment { get; set; }
        public string PhysicalExaminationFilePath { get; set; }
        public string PhysicalExaminationFilename { get; set; }

        public DateTime DateOfCreate { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }

        public CaseStatus CaseStatus { get; set; }
    }
}
