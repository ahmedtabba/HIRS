using BoulevardManagement.DTO;
using BoulevardManagement.Model.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulevardManagement.BLL.Interfaces
{
    public interface IStickyNoteBLL : IService<StickyNote>
    {
        int Insert(StickyNoteDTO obDTO, List<string> SalesUsers = null);

        IQueryable<StickyNoteDTO> GetAllbyObject(string objectId, string objectType, int channel);
        IQueryable<NewsFeedCommentDTO> GetAllbyObject(int objectId, string objectType, int channel);
        IQueryable<StickyNoteDTO> GetAll();
        IQueryable<StickyNoteGridDTO> GetAllForGrid();
        void Update(StickyNoteDTO obDTO);
        void Delete(int id);



    }
}
