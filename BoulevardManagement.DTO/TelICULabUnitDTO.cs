using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleA.Utilities.HashidsNet;

namespace BoulevardManagement.DTO
{
    public class TelICULabUnitDTO : EntityDTO
    {
        public int TeleICUId { get; set; }
        public string EncrptedTeleICUId { get { return HashIdsManager.Encrypt(TeleICUId); } set { } }
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
        public BloodType? CBCBloodGroup { get; set; }
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
        public PositiveNegativeEnum? SerologyHBSAG { get; set; }
        public PositiveNegativeEnum? SerologyHCV { get; set; }
        public PositiveNegativeEnum? SerologyHIV { get; set; }
        public decimal? UrinePH { get; set; }
        public SignTypeEnum? UrineGlucose { get; set; }
        public AppearanceEnum? UrineAppearance { get; set; }
        public decimal? UrineSG { get; set; }
        public SignTypeEnum? UrintHemoglobin { get; set; }
        public ColorEnum? UrineColor { get; set; }
        public SignTypeEnum? UrineProtein { get; set; }
        public SignTypeEnum? UrineBilllrubin { get; set; }
        public SignTypeEnum? UrineKitones { get; set; }
        public int? UrineWBC { get; set; }
        public SignTypeEnum? UrineCalciumOxalate { get; set; }
        public int? UrineRBC { get; set; }
        public SignTypeEnum? UrineAmorphousUrate { get; set; }
        public int? UrineECells { get; set; }
        public SignTypeEnum? UrineUACrystals { get; set; }
        public SignTypeEnum? UrineMucus { get; set; }
        public SignTypeEnum? UrineCandida { get; set; }
        public SignTypeEnum? UrineBacteria { get; set; }
        public SignTypeEnum? UrineGCylinders { get; set; }
        public DateTime DateOfCreate { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }

        #region  BloddGases
        public BloodGases? BloodGases { get; set; }
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
        public bool HasBloodGasesAttachment { get; set; }
        public string BloodGasesFilePath { get; set; }
        public string BloodGasesFilename { get; set; }

        public int? ChemistryAttachmentId { get; set; }
        public bool HasChemistryAttachment { get; set; }
        public string ChemistryFilePath { get; set; }
        public string ChemistryFilename { get; set; }

        public int? CBCAttachmentId { get; set; }
        public bool HasCBCAttachment { get; set; }
        public string CBCFilePath { get; set; }
        public string CBCFilename { get; set; }

        public int? ElectrolytesAttachmentId { get; set; }
        public bool HasElectrolytesAttachment { get; set; }
        public string ElectrolytesFilePath { get; set; }
        public string ElectrolytesFilename { get; set; }

        public int? CoagulationTestAttachmentId { get; set; }
        public bool HasCoagulationTestAttachment { get; set; }
        public string CoagulationTestFilePath { get; set; }
        public string CoagulationTestFilename { get; set; }

        public int? SerologyAttachmentId { get; set; }
        public bool HasSerologyAttachment { get; set; }
        public string SerologyFilePath { get; set; }
        public string SerologyFilename { get; set; }


        public int? UrineTestAttachmentId { get; set; }
        public bool HasUrineTestAttachment { get; set; }
        public string UrineTestFilePath { get; set; }
        public string UrineTestFilename { get; set; }

        public int? LabUnitGeneralAttachmentId { get; set; }
        public bool HasLabUnitGeneralAttachment { get; set; }
        public string LabUnitGeneralFilePath { get; set; }
        public string LabUnitGeneralFilename { get; set; }

        public CaseStatus CaseStatus { get; set; }
    }
}
