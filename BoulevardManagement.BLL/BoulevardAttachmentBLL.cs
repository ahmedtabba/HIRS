using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.Model.Entities;
using BoulevardManagement.DTO;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern;
using AutoMapper;
using Repository.Pattern.Infrastructure;
using AutoMapper.QueryableExtensions;
using System.IO;
using BoulevardManagement.Utilities;
using TripleA.Utilities.Encryption;
using TripleA.Utilities.Helpers;
using System.Web;

namespace BoulevardManagement.BLL
{



    public class BoulevardAttachmentBLL : Service<BoulevardAttachment>, IBoulevardAttachmentBLL
    {
        readonly IRepositoryAsync<BoulevardAttachment> _repository;
        readonly IUnitOfWorkAsync _unitOfWorkAsync;
        readonly IApplicationUserDataContext _applicationContext;
        public BoulevardAttachmentBLL(IRepositoryAsync<BoulevardAttachment> repository, IUnitOfWorkAsync unitOfWorkAsync, IApplicationUserDataContext applicationContex) : base(repository, applicationContex)
        {
            _repository = repository;
            _unitOfWorkAsync = unitOfWorkAsync;
            _applicationContext = applicationContex;
        }
        public IQueryable<BoulevardAttachmentDTO> AttachmentGetByAttachmentTypeAndObjectId(AttachmentReferenceTypes AttachmentType, int objectId)
        {
            try
            {
                var Attachmentslst = Query()
                   .SelectQueryable()
                   .Where(c => (c.ReferenceId == objectId && c.ReferenceType == (int)AttachmentType)).AsQueryable().ProjectTo<BoulevardAttachmentDTO>();

                return Attachmentslst;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public BoulevardAttachmentDTO AttachmentGetById(int AttachmentId)
        {
            try
            {
                return Query()
                   .SelectQueryable()
                   .Where(c => c.Id == AttachmentId).AsQueryable().ProjectTo<BoulevardAttachmentDTO>().FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        public int AddAttachment(BoulevardAttachmentDTO AttachmentObject)
        {
            try
            {

                AttachmentObject.CreatedByName = _applicationContext.GetApplicationUserData().FullName;
                _unitOfWorkAsync.BeginTransaction();
                var ZestAttachment = new BoulevardAttachment();
                Mapper.Map<BoulevardAttachmentDTO, BoulevardAttachment>(AttachmentObject, ZestAttachment);
                #region Copy file to Desination Folder
                if (!File.Exists(AttachmentObject.FilePath))
                    throw new ApplicationException("File doesn't Exist" + "AddAttachment");
                string destenationPath = Path.Combine(Configurations.AttachmentsPhysicalPath, QueryStringEncrypting.Encrypt(AttachmentObject.ReferenceId + "" + AttachmentObject.ReferenceType));
                if (!Directory.Exists(destenationPath))
                    destenationPath = HelperMethods.CreateFolder(destenationPath);
                var file = new FileInfo(AttachmentObject.FilePath);
                string destenationFileName = Guid.NewGuid().ToString() + file.Extension;
                File.Copy(file.FullName, Path.Combine(destenationPath, destenationFileName), true);
                ZestAttachment.FileExtension = file.Extension;
                #endregion
                //Attachment.FilePath = Path.Combine(destenationPath, destenationFileName);
                ZestAttachment.FilePath = Path.Combine(QueryStringEncrypting.Encrypt(AttachmentObject.ReferenceId + "" + AttachmentObject.ReferenceType), destenationFileName);
                ZestAttachment.ObjectState = ObjectState.Added;

                Insert(ZestAttachment);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
                if (ZestAttachment.Id != 0)
                    HelperMethods.DeleteFolder(new FileInfo(AttachmentObject.FilePath).Directory.ToString());
                return ZestAttachment.Id;
            }

            catch (Exception ex)
            {
                throw new ApplicationException("AddAttachment" + ex.InnerException);

            }
        }

        public int Update(BoulevardAttachmentDTO Attachmentobj)
        {
            try
            {

                Attachmentobj.CreatedByName = _applicationContext.GetApplicationUserData().FullName;
                _unitOfWorkAsync.BeginTransaction();
                var Attachment = Find(Attachmentobj.Id);
                var creationDate = Attachment.CreationDate;
                Mapper.Map<BoulevardAttachmentDTO, BoulevardAttachment>(Attachmentobj, Attachment);
                Attachment.CreationDate = creationDate;

                #region Copy file to Desination Folder
                if (!File.Exists(Attachmentobj.FilePath))
                    throw new ApplicationException("File doesn't Exist" + "AddAttachment");
                string destenationPath = Path.Combine(Configurations.AttachmentsPhysicalPath, QueryStringEncrypting.Encrypt(Attachmentobj.ReferenceId + "" + Attachmentobj.ReferenceType));
                if (!Directory.Exists(destenationPath))
                    destenationPath = HelperMethods.CreateFolder(destenationPath);
                var file = new FileInfo(Attachmentobj.FilePath);
                string destenationFileName = Guid.NewGuid().ToString() + file.Extension;
                File.Copy(file.FullName, Path.Combine(destenationPath, destenationFileName), true);
                #endregion
                //Attachment.FilePath = Path.Combine(destenationPath, destenationFileName);
                Attachment.FilePath = Path.Combine(QueryStringEncrypting.Encrypt(Attachmentobj.ReferenceId + "" + Attachmentobj.ReferenceType), destenationFileName);

                Attachment.FileExtension = file.Extension;
                Attachment.ObjectState = ObjectState.Modified;

                InsertOrUpdateGraph(Attachment);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
                if (Attachment.Id != 0)
                    HelperMethods.DeleteFolder(new FileInfo(Attachmentobj.FilePath).Directory.ToString());
                return Attachment.Id;
            }
            catch (Exception ex)
            {
                _unitOfWorkAsync.Rollback();
                throw ex;
            }
        }

        #region Helper Methods
        public void DeleteAttachment(int AttachmentId)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var AttachmentPath = _repository.Find(AttachmentId).FilePath;
                Delete(AttachmentId);
                _unitOfWorkAsync.SaveChanges();
                _unitOfWorkAsync.Commit();
                if (Directory.Exists(new FileInfo(AttachmentPath).Directory.ToString()))
                    HelperMethods.DeleteFolder(new FileInfo(AttachmentPath).Directory.ToString());

            }
            catch
            {
                throw;
            }
        }

        public List<string> DeleteAttachments(AttachmentReferenceTypes attachmentType, int agreementId)
        {
            try
            {
                var attachmentList = AttachmentGetByAttachmentTypeAndObjectId(attachmentType, agreementId).ToList();
                List<string> attachmentPaths = new List<string>();
                foreach (BoulevardAttachmentDTO attachment in attachmentList)
                {
                    attachmentPaths.Add(attachment.FilePath);
                    Delete(attachment.Id);
                }

                return attachmentPaths;
            }
            catch
            {
                throw;
            }
        }

        public string SaveFile(HttpPostedFileBase file, Guid fileGuid)
        {
            try
            {
                if (!Directory.Exists(Configurations.TempAttachmentsPhysicalPath.ToString()))
                    Directory.CreateDirectory(Configurations.TempAttachmentsPhysicalPath.ToString());

                // Some browsers send file names with full path.
                // We are only interested in the file name.
                string FolderLocation = HelperMethods.CreateFolder(Path.Combine(Configurations.TempAttachmentsPhysicalPath, fileGuid.ToString()));
                var fileName = Path.GetFileName(file.FileName);
                var physicalPath = Path.Combine(FolderLocation, fileName);
                // The files are not actually saved in this demo
                file.SaveAs(physicalPath);

                return physicalPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteFiles(List<string> attachmentPaths)
        {
            foreach (string path in attachmentPaths)
            {
                if (Directory.Exists(new FileInfo(path).Directory.ToString()))
                    HelperMethods.DeleteFolder(new FileInfo(path).Directory.ToString());
            }
        }

        public int GetAttachmentsCountOfStatus(AttachmentReferenceTypes attachmentType, int referanceId, int referanceStatus)
        {
            return Query()
                   .SelectQueryable()
                   .Where(c => (c.ReferenceId == referanceId && c.ReferenceType == (int)attachmentType) && c.ReferenceStatus == referanceStatus).Count();

        }


        #endregion



    }
}
