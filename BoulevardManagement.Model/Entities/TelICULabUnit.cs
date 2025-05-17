using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelICULabUnit : Entity
    {
        public int TeleICUId { get; set; }
        public TeleICU TeleICU { get; set; }
        public int? ChemistryGlucose { get; set; }
        public int? ChemistryUrea { get; set; }
        public decimal? ChemistryCreatinine { get; set; }
        public int? ChemistryTotalProteins { get; set; }
        public decimal? ChemistryAlbumin { get; set; }
        public decimal? ChemistryGlobulins { get; set; }
        public int? ChemistrySGPT { get; set; }
        public decimal? ChemistryBilirTotal { get; set; }
        public decimal? ChemistryBilirDirct { get; set; }
        public decimal? ChemistryBilirIndirect { get; set; }
        public int? ChemistryCKMB { get; set; }
        public int? ChemistryCRP { get; set; }
        public int? ChemistryALP { get; set; }
        public int? ChemistryAmylase { get; set; }
        public int? ChemistryTROPONINT { get; set; }
        public int? CBCWBC { get; set; }
        public decimal? CBCHB { get; set; }
        public int? CBCHT { get; set; }
        public int? CBCL { get; set; }
        public int CBCM { get; set; }
        public int CBCN { get; set; }
         
        public int? CBCPLT { get; set; }
        public int? CBCBloodGroup { get; set; }
        public decimal? CoagulationPtPatient { get; set; }
        public int? CoagulationPtActivity { get; set; }
        public decimal? CoagulationPtINR { get; set; }
        public decimal? CoagulationPtControl { get; set; }
        public decimal? CoagulationBleedingTime { get; set; }
        public decimal? CoagulationClottingTime { get; set; }
        public int? SerologyO { get; set; }
        public int? SerologyH { get; set; }
        public int? SerologyM { get; set; }
        public int? SerologyA { get; set; }
        public int? SerologyHBSAG { get; set; }
        public int? SerologyHCV { get; set; }
        public int? SerologyHIV { get; set; }
        public decimal? UrinePH { get; set; }
        public int? UrineGlucose { get; set; }
        public int? UrineAppearance { get; set; }
        public decimal? UrineSG { get; set; }
        public int? UrintHemoglobin { get; set; }
        public int? UrineColor { get; set; }
        public int? UrineProtein { get; set; }
        public int? UrineBilllrubin { get; set; }
        public int? UrineKitones { get; set; }
        public int? UrineWBC { get; set; }
        public int? UrineCalciumOxalate { get; set; }
        public int? UrineRBC { get; set; }
        public int? UrineAmorphousUrate{ get; set; }
        public int? UrineECells { get; set; }
        public int? UrineUACrystals { get; set; }
        public int? UrineMucus { get; set; }
        public int? UrineCandida { get; set; }
        public int? UrineBacteria { get; set; }
        public int? UrineGCylinders { get; set; }

        #region  BloddGases
        public int? BloodGases { get; set; }
        public int? PH { get; set; }
        public int? PCO2 { get; set; }
        public int? PO2 { get; set; }
        public int? HCO3 { get; set; }
        public int? BE { get; set; }
        public int? AG { get; set; }
        #endregion

        #region Electrolytes
        public int? Sodium { get; set; }
        public int? Potassium { get; set; }
        public int? Chloride { get; set; }
        public int? Calcium { get; set; }
        public int? Magnesium { get; set; }
        public int? Phosphorous { get; set; }
        #endregion
        public int? BloodGasesAttachmentId { get; set; }
        public int? ChemistryAttachmentId { get; set; }
        public int? CBCAttachmentId { get; set; }
        public int? ElectrolytesAttachmentId { get; set; }

        public int? CoagulationTestAttachmentId { get; set; }
        public int? SerologyAttachmentId { get; set; }

        public int? UrineTestAttachmentId { get; set; }
        public int? LabUnitGeneralAttachmentId { get; set; }


    }
}
