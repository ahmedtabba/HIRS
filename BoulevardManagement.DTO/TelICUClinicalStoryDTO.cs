using BoulevardManagement.DTO.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class TelICUClinicalStoryDTO : EntityDTO
    {
        public int TeleICUId { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public DateTime AcceptanceDate { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public string Complaint { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public string ComplaintDetails { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public string MedicalHistory { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public string PreviousCatheter { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public string BPFirstPart { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public string BPSecondPart { get; set; }
        public string BP { get; set; }
        public decimal? Map { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]

        public string Pulse { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]

        public string Cvs { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]

        public string Cxr { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]

        public string ECG { get; set; }
        public string MitralValve { get; set; }
        public string AorticValve { get; set; }
        public string Others { get; set; }
        public string UltrasoundHeartScan { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public string PharmacologicalPrecedents { get; set; }
        public string EF { get; set; }
        public string Diagnosis { get; set; }
        public bool IsThereAnAllergyToDrugs { get; set; }
        public string DrugsDetails { get; set; }
        public string PreviousSurgicalHistory { get; set; }
        [Required(ErrorMessageResourceType = typeof(CommonResource), ErrorMessageResourceName = "RequiredFeild")]
        public RiskFactorsEnum RiskFactors { get; set; }
        public DiagnosisStatus? DiagnosisStatus { get; set; }
        public int? CxrAttachmentId { get; set; }
        public bool HasCxrAttachment { get; set; }
        public string CxrFilePath { get; set; }
        public string CxrFilename { get; set; }
        public int? ECGAttachmentId { get; set; }
        public bool HasECGAttachment { get; set; }
        public string ECGFilePath { get; set; }
        public string ECGFilename { get; set; }
        public CaseStatus CaseStatus { get; set; }

        public TelICUClinicalStoryDTO()
        {
            AcceptanceDate = DateTime.Now;
        }
    }
}
