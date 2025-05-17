using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripleA.Utilities.HashidsNet;

namespace BoulevardManagement.DTO
{
    public class NPICUDTO:EntityDTO
    {
        public int PatientId { get; set; }
        public string PatientEncreptedId { get { return HashIdsManager.Encrypt(PatientId); } }
        public int PatientDepartmentId { get; set; }
        public string PatientName { get; set; }
        public string PatientFatherName { get; set; }
        public string PatientMotherName { get; set; }
        public string PatientCode { get; set; }
        public Gender PatientGender { get; set; }
        public BloodType PatientBloodType { get; set; }
        public MaritalStatus PatientMaritalStatus { get; set; }
        public DateTime PatientDateOfBirth { get; set; }
        public string PatientPlaceOfBirth { get; set; }
        public string PatientPhoneNumber { get; set; }
        public NPICUCurrentSteps CurrentStep { get; set; }
        public string CreatedByUserId { get; set; }
        public NPICUType NPICUType { get; set; }
        public CaseStatus Status { get; set; }
        public ICollection<NPICUUserDTO> InvolvedServiceProviders { get; set; }
        public ICollection<NPICUUserDTO> InvolvedConsultants { get; set; }
        public ICollection<NPICUUserDTO> InvolvedUsers { get; set; }

        public List<string> InvolvedServiceProvidersNamesList { get; set; }
        public string InvolvedServiceProvidersNames
        {
            get
            {
                return string.Join(",", InvolvedServiceProvidersNamesList);
            }
        }

        public List<string> InvolvedServiceProvidersIds { get; set; }


        public List<string> InvolvedConsultantsNamesList { get; set; }
        public string InvolvedConsultantsNames
        {
            get
            {
                return string.Join(",", InvolvedConsultantsNamesList);
            }
        }


        public DateTime DateOfCreation { get; set; }

        public List<string> InvolvedConsultantsIds { get; set; }

        public NPICUDTO()
        {
            InvolvedServiceProviders = new List<NPICUUserDTO>();
            InvolvedConsultants = new List<NPICUUserDTO>();
            InvolvedUsers = new List<NPICUUserDTO>();
            InvolvedServiceProvidersIds = new List<string>();
            InvolvedServiceProvidersNamesList = new List<string>();

            InvolvedConsultantsIds = new List<string>();
            InvolvedConsultantsNamesList = new List<string>();
        }
    }
}
