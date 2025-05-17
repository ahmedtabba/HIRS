using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoulevardManagement.WebApplication.Models.Admin
{
    public class DiagnosisCategoryVM
    {
        [JsonProperty("DiagnosisName")]
        public string DiagnosisName { get; set; }
        [JsonProperty("DiagnosisArabicName")]
        public string DiagnosisArabicName { get; set; }
        [JsonProperty("SubDiagnosisName")]
        public string SubDiagnosisName { get; set; }
        [JsonProperty("SubDiagnosisArabicName")]
        public string SubDiagnosisArabicName { get; set; }
    }
}