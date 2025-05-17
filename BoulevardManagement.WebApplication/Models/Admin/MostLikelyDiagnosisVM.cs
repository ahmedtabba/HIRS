using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BoulevardManagement.WebApplication.Models.Admin
{
    public class MostLikelyDiagnosisVM
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        
    }
}