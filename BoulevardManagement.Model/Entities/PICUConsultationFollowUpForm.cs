using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class PICUConsultationFollowUpForm:Entity
    {
        public int NPICUId { get; set; }
        public NPICU NPICU { get; set; }

        public int? PhysicalExaminationAttachmentId { get; set; }
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
        public int? GeneralAppearance1 { get; set; }
        public int? GeneralAppearance2 { get; set; }
        #endregion

        #region General
        public string CentralNervousSystem { get; set; }
        public string CardiovascularSystem { get; set; }
        public string RespiratorySystem { get; set; }
        public string GastrointestinalTract { get; set; }
        public string GenitourinarySystem { get; set; }
        public string Others { get; set; }

        #endregion
    }
}
