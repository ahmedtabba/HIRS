using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.Model.Entities
{
    public class TelMHClinicalStory:Entity
    {
        public int TeleMentalHealthId { get; set; }
        public TeleMentalHealth TeleMentalHealth { get; set; }

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
        public string CurrentComplaint { get; set; }
        public string CurrentComplaintVoiceAttachPath { get; set; }
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
        public string CaseSummary { get; set; }
        public string CaseSummaryVoiceAttachPath { get; set; }

        public ICollection<TelMHDiagnosis> Diagnosis { get; set; }
        public ICollection<TelMHMostLikelyDiagnosis> MostLikelyDiagnosis { get; set; }


        #endregion
        public TelMHClinicalStory()
        {
            Diagnosis = new List<TelMHDiagnosis>();
            MostLikelyDiagnosis = new List<TelMHMostLikelyDiagnosis>();

        }
    }
}
