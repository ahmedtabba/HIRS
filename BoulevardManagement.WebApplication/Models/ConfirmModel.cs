using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BoulevardManagement.WebApplication.Resources;

namespace BoulevardManagement.WebApplication.Models
{
    public class ConfirmModel
    {
        public ConfirmModel()
        {

        }
        public string Title { set; get; }
        public string Content { set; get; }
        public string ButtonId { set; get; }
        public string ButtonTextOk { set; get; }
        public string ButtonTextCancel { set; get; }
        public string ModelId { set; get; }
    }


    public class DeleteConfirmModal : ConfirmModel
    {
        public DeleteConfirmModal()
        {
            this.ModelId = "DeleteConfirm";
            this.Content = CommonResource.DeleteConfirmContent;
            this.ButtonId = "DeleteConfirmButton";
            this.ButtonTextOk = CommonResource.Yes;
            this.ButtonTextCancel = CommonResource.No;
            this.Title = CommonResource.DeleteConfirmTitle;
        }
    }
}