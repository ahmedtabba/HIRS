using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BoulevardManagement.DTO;

namespace BoulevardManagement.WebApplication.Models
{
    public class AttachmentTemplateGridModel
    {

        public string GridId { set; get; }
        public int ReferenceId { set; get; }
        public int ReferenceStatus { set; get; }
        public AttachmentReferenceTypes ReferenceType { set; get; }
    }
}