using BoulevardManagement.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoulevardManagement.WebApplication.Models.StickyNote
{
    public class CommentsVM
    {
        public List<NewsFeedCommentDTO> Comments { get; set; }
        public string CurrentUserImageURL { get; set; }
        public CommentsVM()
        {
            Comments = new List<NewsFeedCommentDTO>();
        }
    }
}