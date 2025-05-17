using TripleA.Utility.DataAnnotationsAttributes;

namespace BoulevardManagement.DTO
{

    public class BoulevardAttachmentDTO : EntityDTO
    {
        public  string FileExtension
        {
            get;
            set;
        }
        public  string FilePath
        {
            get;
            set;
        }

        [CustomRequired]
        public  string Description
        {
            get;
            set;
        }

        public  int ReferenceId
        {
            get;
            set;
        }

        public int ReferenceStatus
        {
            get;
            set;
        }

        public AttachmentReferenceTypes ReferenceType
        {
            get;
            set;
        }
        public string CreatedByName { get; set; }

        public System.DateTime CreationDate { get; set; }
    }
}
