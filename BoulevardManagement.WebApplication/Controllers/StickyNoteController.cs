using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.DTO;
using BoulevardManagement.Utilities;
using BoulevardManagement.WebApplication.Models;
using BoulevardManagement.WebApplication.Models.StickyNote;
using BoulevardManagement.WebApplication.Utilities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity.Owin;
using Repository.Pattern;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripleA.Utilities.HashidsNet;

namespace BoulevardManagement.WebApplication.Controllers
{
    public class StickyNoteController : BaseController
    {
        IStickyNoteBLL _stickyNoteBLL;
        ITeleICUBLL _teleIcuBLL;
        ITeleMentalHealthBLL _teleMentalHealthBLL;
        INPICUBLL _npicuBLL;

        IApplicationUserDataContext _appContext;

        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }

        }

        public StickyNoteController(IStickyNoteBLL stickyNoteBLL,
            ITeleICUBLL teleIcuBLL,
            ITeleMentalHealthBLL teleMentalHealthBLL,
            INPICUBLL npicuBLL,
            IApplicationUserDataContext appContext,
        IErrorLogBLL errorLogBLL) : base(errorLogBLL)
        {
            _stickyNoteBLL = stickyNoteBLL;
            _appContext = appContext;
            _teleIcuBLL = teleIcuBLL;
            _teleMentalHealthBLL = teleMentalHealthBLL;
            _npicuBLL = npicuBLL;
        }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult LoadStickyNote(string objectType, string objectId, string objectTag, int? Channel)
        {
            ViewBag.ObjectType = objectType;
            ViewBag.ObjectId = objectId;
            ViewBag.ObjectTag = objectTag;
            ViewBag.Channel = Channel;
            ViewBag.ParentId = Channel;
            return PartialView("StickyNote/_StickyNote");
        }


        


        public PartialViewResult LoadNotes(string objectType, string objectId, string objectTag, int? Channel)
        {
            var objId = HashIdsManager.Decrypt(objectId);
            var comments = _stickyNoteBLL.GetAllbyObject(objId, objectType, (int)Channel).OrderByDescending(x => x.CreationDate).GroupBy(x => x.ParentId).ToList();
            var rootComments = comments.Where(x => x.Key == null);
            var IcuUsers=new List<TeleICUUserDTO>();
            var MHUsers = new List<TeleMentalHealthUserDTO>();
            var NPICUUsers = new List<NPICUUserDTO>();
            var Id = HashIdsManager.Decrypt(objectId);

            if (objectType== "TeleICU")
            {
                _teleIcuBLL.UpdateInvolvedUsersColors(Id);
                IcuUsers = _teleIcuBLL.GetAll().Where(c => c.Id == Id).Include(f => f.InvolvedUsers).Select(g => g.InvolvedUsers).FirstOrDefault().ToList();
            }
            else if (objectType == "TeleMentalHealth")
            {
                _teleMentalHealthBLL.UpdateInvolvedUsersColors(Id);
                MHUsers = _teleMentalHealthBLL.GetAll().Where(c => c.Id == Id).Include(f => f.InvolvedUsers).Select(g => g.InvolvedUsers).FirstOrDefault().ToList();

            }
            else if (objectType== "NPICU")
            {
                _npicuBLL.UpdateInvolvedUsersColors(Id);
                NPICUUsers = _npicuBLL.GetAll().Where(c => c.Id == Id).Include(f => f.InvolvedUsers).Select(g => g.InvolvedUsers).FirstOrDefault().ToList();

            }
            List<NewsFeedCommentDTO> result = new List<NewsFeedCommentDTO>();
            var UnSeenNotes = 0;
            foreach (var root in rootComments)
            {
                foreach (var comment in root)
                {

                    var commenttingUser = UserManager.Users.Where(x => x.Id == comment.CreatedBy).FirstOrDefault();
                    comment.UserPhoto = GetUserPhoto(commenttingUser);
                    comment.UserFullName = commenttingUser.FullName;
                    if (objectType == "TeleICU")
                    {
                        comment.UserColor = IcuUsers.Where(c => c.UserId == commenttingUser.Id).Select(c => c.Color).FirstOrDefault();
                        if (commenttingUser.Id==GetCurrentUser.UserId)
                        {
                            ViewBag.UserColor = comment.UserColor;
                        }
                    }
                    else if (objectType == "TeleMentalHealth")
                    {
                        comment.UserColor = MHUsers.Where(c => c.UserId == commenttingUser.Id).Select(c => c.Color).FirstOrDefault();
                        if (commenttingUser.Id == GetCurrentUser.UserId)
                        {
                            ViewBag.UserColor = comment.UserColor;
                        }
                    }
                    else if (objectType == "NPICU")
                    {
                        comment.UserColor = NPICUUsers.Where(c => c.UserId == commenttingUser.Id).Select(c => c.Color).FirstOrDefault();
                        if (commenttingUser.Id == GetCurrentUser.UserId)
                        {
                            ViewBag.UserColor = comment.UserColor;
                        }
                    }
                    comment.Duration = DateUtilities.GetPrettyDate(DateTime.Now - comment.CommentDate) + " | " + comment.CommentDate.ToLocalTime();
                    var currentUser = _appContext.GetApplicationUserData();
              
                    var subGroup = comments.Where(x => x.Key == comment.Id);
                    foreach (var group in subGroup)
                    {
                        foreach (var subComment in group)
                        {
                            commenttingUser = UserManager.Users.Where(x => x.Id == subComment.CreatedBy).FirstOrDefault();
                            if (objectType == "TeleICU")
                            {
                                subComment.UserColor = IcuUsers.Where(c => c.UserId == commenttingUser.Id).Select(c => c.Color).FirstOrDefault();
                                if (commenttingUser.Id == GetCurrentUser.UserId)
                                {
                                    ViewBag.UserColor = subComment.UserColor;
                                }
                            }
                            else if (objectType == "TeleMentalHealth")
                            {
                                subComment.UserColor = MHUsers.Where(c => c.UserId == commenttingUser.Id).Select(c => c.Color).FirstOrDefault();
                                if (commenttingUser.Id == GetCurrentUser.UserId)
                                {
                                    ViewBag.UserColor = subComment.UserColor;
                                }
                            }
                            else if (objectType == "NPICU")
                            {
                                subComment.UserColor = NPICUUsers.Where(c => c.UserId == commenttingUser.Id).Select(c => c.Color).FirstOrDefault();
                                if (commenttingUser.Id == GetCurrentUser.UserId)
                                {
                                    ViewBag.UserColor = subComment.UserColor;
                                }
                            }
                            subComment.UserPhoto = GetUserPhoto(commenttingUser);

                            subComment.UserFullName = commenttingUser.FullName;
                          //  subComment.UserColor = commenttingUser.SpecifiedColor;
                            subComment.Duration = DateUtilities.GetPrettyDate(DateTime.Now - subComment.CommentDate) + " | " + comment.CommentDate.ToShortDateString();
                            comment.ChildComments.Add(subComment);
                        
                         
                        }

                    }
                    result.Add(comment);

                }

            }

            CommentsVM commentsVM = new CommentsVM();
            commentsVM.Comments = result;
            var userId = _appContext.GetApplicationUserData().UserId;
            commentsVM.CurrentUserImageURL = GetUserPhoto(UserManager.Users.FirstOrDefault(x => x.Id == userId));


            return PartialView("StickyNote/_CommentBlock", commentsVM);
        }




        [HttpPost]
        public JsonResult AddNote(string objectType, string objectId, string objectTag, int channel, int? parantId, string note, bool isVoice = false)
        {
            try
            {

                int result = 0;
                if (!String.IsNullOrEmpty(note))
                {
                   
                        if (isVoice)
                        {
                            result = _stickyNoteBLL.Insert(new StickyNoteDTO() { Channel = (ChannelEnum)channel, FilePath = note, StickyNoteMessageType = StickyNoteMessageTypeEnum.VoiceMessageNote, ObjectTag = objectTag, ObjectEncriptedId = objectId, ObjectId = HashIdsManager.Decrypt(objectId), ObjectType = objectType, Note = "<audio controls='' src='" + note + "'></audio>", CreatedUserName = GetCurrentUser.FullName, ParentId = parantId });
                        }
                        else
                            result = _stickyNoteBLL.Insert(new StickyNoteDTO() { Channel = (ChannelEnum)channel, ObjectTag = objectTag, ObjectEncriptedId = objectId, ObjectId = HashIdsManager.Decrypt(objectId), ObjectType = objectType, Note = note, CreatedUserName = GetCurrentUser.FullName, ParentId = parantId });
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




        private string GetUserPhoto(ApplicationUser user)
        {
            if (user.HasPhoto)
                return Url.Content(string.Format("{0}{1}.jpg?timestamp=" + DateTime.Now.Ticks, Configurations.ProfileVirtualPath, user.Id));
            else
            {
                if (user.Gender==Gender.Male)
                {
                    return "/assets/media/users/male.png";
                }
                else
                {
                    return "/assets/media/users/female.png";
                }
            }
           
        }




        public ActionResult Read([DataSourceRequest] DataSourceRequest request, string channelName)
        {
            IEnumerable<StickyNoteGridDTO> notes;
            if (!String.IsNullOrWhiteSpace(channelName))
            {
                ChannelEnum currentCH = (ChannelEnum)Enum.Parse(typeof(ChannelEnum), channelName, true);
                notes = _stickyNoteBLL.GetAllForGrid().Where(n => n.Channel == currentCH).OrderBy(t => t.Id);
            }
            else
                notes = _stickyNoteBLL.GetAllForGrid().OrderBy(t => t.Id);

            DataSourceResult result = notes.ToDataSourceResult(request);
            foreach (StickyNoteGridDTO obj in result.Data)
            {
                switch (obj.ObjectType)
                {
                 
                 
                    default:
                        obj.ObjectLink = ":)";
                        break;
                }

            }

            return Json(result);
        }

        public JsonResult GetObjectLink(string objectType, string objectId)
        {
            StickyNoteObjectVM stickyNote = new StickyNoteObjectVM();
            return Json(stickyNote, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RenderAddRemark()
        {
            return PartialView("_AddRemarkModel", 50139);
        }


        [HttpPost]
        public string Upload()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var fileGuide = Guid.NewGuid();
                    var file = Request.Files[0];
                    var fileName = fileGuide.ToString() + ".mp3";
                    var serverPath = Url.RouteUrl("Default", new { controller = "Home", action = "Index" });
                    var path = Path.Combine(Server.MapPath("~/Uploads/Voice"), fileName);
                    file.SaveAs(path);
                    return $"{serverPath}Uploads/Voice/{fileName}";
                }
                else
                    return "";


            }
            catch (Exception)
            {

                throw;
            }


        }

        public string Delete(int id)
        {
            try
            {
                var StickyNoteFilePath = _stickyNoteBLL.GetAll().Where(c => c.Id == id).Select(c => c.FilePath).FirstOrDefault();

                if (User.IsInRole(RoleConsistent.StickyNote.Delete))
                {
                    if (StickyNoteFilePath != null)
                    {
                        string fullPath = Request.MapPath("~/Uploads/Voice/" + StickyNoteFilePath.Replace("/Uploads/Voice/", ""));
                        System.IO.File.Delete(fullPath);
                        _stickyNoteBLL.Delete(id);
                    }

                }
                else
                {
                    return "You Do not Have Permession To This Action";
                }
                return "Success";


            }
            catch (Exception ex)
            {
                return "Failed";

                throw;
            }
        }


      
    }

}