using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.DTO
{
    public class NewsFeedCommentDTO : StickyNoteDTO
    {
        public virtual string UserPhoto { get; set; }
        public virtual string UserFullName { get; set; }
        public string UserColor { get; set; }
        public string Duration { get; set; }
        public DateTime CommentDate { get; set; }
      //  public bool HasBookMark { get; set; }
        public List<NewsFeedCommentDTO> ChildComments { get; set; }
        public NewsFeedCommentDTO()
        {
            ChildComments = new List<NewsFeedCommentDTO>();
        }
    }
}
