using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelICUClinicalStory : Entity
    {
        public int TeleICUId { get; set; }
        public TeleICU TeleICU { get; set; }
        public DateTime AcceptanceDate{ get; set; }
        public string Complaint { get; set; }
        public string ComplaintDetails{ get; set; }
        public string MedicalHistory{ get; set; }
        public string PreviousCatheter { get; set; }
        public string BP { get; set; }
        public decimal? Map { get; set; }
        public string Pulse { get; set; }
        public string Cvs { get; set; }
        public string Cxr { get; set; }
        public int? CxrAttachmentId { get; set; }
        public string ECG { get; set; }
        public int? ECGAttachmentId { get; set; }
        public string MitralValve { get; set; }
        public string AorticValve { get; set; }
        public string Others { get; set; }
        public string UltrasoundHeartScan{ get; set; }
        public string PharmacologicalPrecedents{ get; set; }
        public string EF { get; set; }
        public string Diagnosis { get; set; }
        public bool IsThereAnAllergyToDrugs { get; set; }
        public string DrugsDetails { get; set; }
        public string PreviousSurgicalHistory { get; set; }
        public int RiskFactors { get; set; }
        public int? DiagnosisStatus { get; set; }

    }
}
