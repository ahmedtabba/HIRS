using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BoulevardManagement.DTO;
using BoulevardManagement.Model.Entities;

namespace BoulevardManagement.BLL.Interfaces
{
    public interface IBoulevardAttachmentBLL : IService<BoulevardAttachment>
    {
        IQueryable<BoulevardAttachmentDTO> AttachmentGetByAttachmentTypeAndObjectId(AttachmentReferenceTypes AttachmentType, int objectId);

        BoulevardAttachmentDTO AttachmentGetById(int AttachmentId);

        int AddAttachment(BoulevardAttachmentDTO AttachmentObject);

        string SaveFile(HttpPostedFileBase file, Guid fileGuid);

        void DeleteFiles(List<string> paths);

        void DeleteAttachment(int AttachmentId);

        int Update(BoulevardAttachmentDTO Attachmentobj);

        List<string> DeleteAttachments(AttachmentReferenceTypes attachmentType, int referanceId);


        int GetAttachmentsCountOfStatus(AttachmentReferenceTypes attachmentType, int referanceId,int referanceStatus);
    }
}
