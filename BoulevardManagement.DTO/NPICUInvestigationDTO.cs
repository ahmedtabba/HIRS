using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class NPICUInvestigationDTO:EntityDTO
    {
        public int NPICUId { get; set; }

        //Enum
        public BloodGases BloodGases { get; set; }
        public int? BloodGasesAttachmentId { get; set; }
        public bool HasBloodGasesAttachment { get; set; }
        public string BloodGasesFilePath { get; set; }
        public string BloodGasesFilename { get; set; }


        public int? CBCWithDifferentialAttachmentId { get; set; }
        public bool HasCBCWithDifferentialAttachment { get; set; }
        public string CBCWithDifferentialFilePath { get; set; }
        public string CBCWithDifferentialFilename { get; set; }


        public int? ElectrolytesAttachmentId { get; set; }
        public bool HasElectrolytesAttachment { get; set; }
        public string ElectrolytesFilePath { get; set; }
        public string ElectrolytesFilename { get; set; }



        public int? BiochemistryAttachmentId { get; set; }
        public bool HasBiochemistryAttachment { get; set; }
        public string BiochemistryFilePath { get; set; }
        public string BiochemistryFilename { get; set; }



        public int? CSFStudyAttachmentId { get; set; }
        public bool HasCSFStudyAttachment { get; set; }
        public string CSFStudyFilePath { get; set; }
        public string CSFStudyFilename { get; set; }

        public int? ChestAndAbdomenXrayAttachmentId { get; set; }
        public bool HasChestAndAbdomenXrayAttachment { get; set; }
        public string ChestAndAbdomenXrayFilePath { get; set; }
        public string ChestAndAbdomenXrayFilename { get; set; }



        public int? OtherInvestigationsAttachmentId { get; set; }
        public bool HasOtherInvestigationsAttachment { get; set; }
        public string OtherInvestigationsFilePath { get; set; }
        public string OtherInvestigationsFilename { get; set; }




        public decimal? PH { get; set; }
        public int? PCO2 { get; set; }
        public int? PO2 { get; set; }
        public decimal? HCO3 { get; set; }
        public decimal? BE { get; set; }
        public decimal? AG { get; set; }



        #region CBC with Differential
        public decimal? RBCs { get; set; }
        public decimal? Hemoglobin { get; set; }
        public decimal? Hematocrit { get; set; }
        public int? MCV { get; set; }
        public decimal? RDW { get; set; }
        public decimal? WBCs { get; set; }
        public int? Neutrophils { get; set; }
        public int? Lymphocytes { get; set; }
        public int? Eosinophils { get; set; }
        public int? Basophils { get; set; }
        public decimal? Reticulocytes { get; set; }
        public int? Platelets { get; set; }
        #endregion

        #region Electrolytes
        public int? Sodium { get; set; }
        public decimal? Potassium { get; set; }
        public int? Chloride { get; set; }
        public decimal? Calcium { get; set; }
        public decimal? Magnesium { get; set; }
        public decimal? Phosphorous { get; set; }
        #endregion

        #region Biochemistry
        public int? SerumGlucose { get; set; }
        public int? Urea { get; set; }
        public decimal? Creatinine { get; set; }
        public int? ALT { get; set; }
        public int? AST { get; set; }
        public decimal? TotalBilirubin { get; set; }
        public decimal? DirectBilirubin { get; set; }
        public decimal? TotalProtein { get; set; }
        public decimal? Albumin { get; set; }
        public int? Ammonia { get; set; }
        public int? AlkalinePhosphatase { get; set; }
        public decimal? Lactate { get; set; }

        #endregion


        #region CSF Study
        public int? CSFGlucose { get; set; }
        public decimal? CSFProtein { get; set; }
        public string CSFLatexAgglutination { get; set; }
        public string CSFGramStain { get; set; }
        public string CSFCulture { get; set; }
        public int? RBSAtLumbarPuncture { get; set; }

        public string ChestAndAbdomenXray { get; set; }
        public string UrineAnalysisAndCulture { get; set; }
        public string BloodCulture { get; set; }
        public string CRP { get; set; }
        public string Echocardiography { get; set; }
        public string Others { get; set; }
        #endregion


        public DateTime DateOfCreate { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }

        public CaseStatus CaseStatus { get; set; }
    }
}
