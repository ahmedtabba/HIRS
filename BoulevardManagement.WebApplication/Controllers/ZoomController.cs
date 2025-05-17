using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.WebApplication.Models.Zoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripleA.Utilities.HashidsNet;
using ZoomAPI;

namespace BoulevardManagement.WebApplication.Controllers
{
    public class ZoomController : BaseController
    {
        private readonly ITeleMentalHealthBLL _teleMentalHealthBLL;
        private readonly IDepartmentBLL _departmentBLL;
        private readonly INPICUBLL _nPICUBLL;
        private readonly ITeleICUBLL _teleICUBLL;
        public ZoomController(IErrorLogBLL errorLogBLL, ITeleMentalHealthBLL teleMentalHealthBLL, IDepartmentBLL departmentBLL, INPICUBLL nPICUBLL, ITeleICUBLL teleICUBLL) : base(errorLogBLL)
        {
            _teleMentalHealthBLL = teleMentalHealthBLL;
            _departmentBLL = departmentBLL;
            _nPICUBLL = nPICUBLL;
            _teleICUBLL = teleICUBLL;
        }

        // GET: Zoom
        public ActionResult CreateZoomMeeting(string topic,string caseId,string caseType)
        {

            var token = "";
            if (caseType == "MH")
                token = ZoomSDK.ZoomToken("UCAELkOBRa2IgkJF7pr33g", "wWx7sZrYIVC6aTtBUa3LpzHY7ua52YctVmPh");

            if (caseType == "ICU")
                token = ZoomSDK.ZoomToken("PR5jLUg3QPGZvcyDhQpekg", "mUsh44knL0jbTa96TsmNahUfgj1kDuxe3hrc");

            if (caseType == "NPICU")
                token = ZoomSDK.ZoomToken("FJB4IMJ8RySYJKLEYFbJJw", "PffJybk58jiyZDbySjEEuDtXf2841fHMoEhJ");


            var meeting = new CreateMeeting() { Topic = topic, Type = 1, StartTime = "2021-08-24T03:02:00", Timezone = "TR",
            Settings=new CreateMeetingSettings { JoinBeforeHost=true}
            };
            var test = ZoomSDK.CreateMeeting(token, meeting);

            var id = HashIdsManager.Decrypt(caseId);



            var UserDepartmentId = GetCurrentUser.DepartmentId;
            var department = _departmentBLL.GetAll().Where(x => x.Id == UserDepartmentId).Select(x => x.Name).FirstOrDefault();
            if (department == "Mental Health")
                _teleMentalHealthBLL.NotifyZoomMeeting(test.JoinUrl, id);
            else if (department == "NPICU")
                _nPICUBLL.NotifyZoomMeeting(test.JoinUrl, id);
            else if (department == "ICU")
                _teleICUBLL.NotifyZoomMeeting(test.JoinUrl, id);




            return Json(new { Success = true ,res=test.StartUrl});
        }


        
    }
}