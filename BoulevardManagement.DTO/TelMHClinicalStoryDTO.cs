using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BoulevardManagement.DTO
{
    public class TelMHClinicalStoryDTO:EntityDTO
    {
        public int TeleMentalHealthId { get; set; }

        #region Personal Information
        public string EducationalLevel { get; set; }
        public string CurrentWork { get; set; }
        public string PreviousWork { get; set; }
        public string CurrentPlaceResidence { get; set; }
        public string PreviousPlaceResidence { get; set; }
        public string NumberOfBrothers { get; set; }
        public string PlaceOfInterview { get; set; }
        public string SourceOfReferral { get; set; }
        public string CauseOfReferral { get; set; }
        public string Remarks { get; set; }
        public string RemarksVoiceAttachPath { get; set; }
        public bool IsRemarksVoiceDeleted{ get; set; }
        public string CurrentComplaint { get; set; }
        public string CurrentComplaintVoiceAttachPath { get; set; }
        public bool IsCurrentComplaintVoiceDeleted { get; set; }

        public string ComplaintDetails { get; set; }
        public string PsychologicalNeurologicalAndPersonalAddictivePrecedents { get; set; }
        public string GeneralMedicalPrecedents { get; set; }
        public string FamilyPrecedents { get; set; }
        public string SocialStory { get; set; }
        public string CharacteristicsOfThePreDisorder { get; set; }
        public bool AccompanyingPersonExisted { get; set; }
        #endregion

        #region Psychological examination
        public string AppearanceBehaviorAndCooperationWithTheExaminer { get; set; }
        public string PsychomotorActivity { get; set; }
        public string Speech { get; set; }
        public string MoodAndAffection { get; set; }
        public string Thinking { get; set; }
        public string PerceptionAndConsciousness { get; set; }
        public string SenseAndCognition_ConsciousnessAndOrientation { get; set; }
        public string SenseAndCognition_Memory { get; set; }
        public string SenseAndCognition_AttentionAndConcentration { get; set; }
        public string SenseAndCognition_Abstraction { get; set; }
        public string SenseAndCognition_Judgment { get; set; }
        public string SenseAndCognition_Insight { get; set; }
        public string RiskAssessment { get; set; }
        public string RiskAssessmentVoiceAttachPath { get; set; }
        public bool IsRiskAssessmentVoiceDeleted { get; set; }

        public string CaseSummary { get; set; }
        public string CaseSummaryVoiceAttachPath { get; set; }
        public bool IsCaseSummaryVoiceDeleted { get; set; }

        public List<TelMHDiagnosisDTO> Diagnosis { get; set; }

        public ICollection<TelMHMostLikelyDiagnosisDTO> MostLikelyDiagnosis { get; set; }
        public List<string> MostLikelyDiagnosisNamesList { get; set; }
        public List<int> MostLikelyDiagnosisIds { get; set; }
        public string MostLikelyDiagnosisNames
        {
            get
            {
                return string.Join(",", MostLikelyDiagnosisNamesList);
            }
        }
        #endregion

        public CaseStatus CaseStatus { get; set; }
        public TelMHClinicalStoryDTO()
        {
            Diagnosis = new List<TelMHDiagnosisDTO>();
            MostLikelyDiagnosis = new List<TelMHMostLikelyDiagnosisDTO>();
            MostLikelyDiagnosisNamesList = new List<string>();
            MostLikelyDiagnosisIds = new List<int>();

        }
    }
}
