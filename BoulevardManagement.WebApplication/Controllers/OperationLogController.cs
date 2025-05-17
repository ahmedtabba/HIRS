using BoulevardManagement.BLL.Interfaces;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity.Owin;
using Repository.Pattern;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripleA.Utilities.Exceptions;

namespace BoulevardManagement.WebApplication.Controllers
{
    public class OperationLogController : BaseController
    {
        private readonly IOperationLogBLL _operationLogBLL;
        private readonly IApplicationUserDataContext _appContext;

        public OperationLogController(
            IOperationLogBLL operationLogBLL,
            IApplicationUserDataContext appContext,
             IErrorLogBLL errorLogBLL) : base(errorLogBLL)
        {
            _operationLogBLL = operationLogBLL;
            _appContext = appContext;
        }


        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        // GET: OperationLog
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ViewLog(string objectType, string objectId)
        {
            ViewBag.ObjectType = objectType ;
            ViewBag.ObjectId = objectId;
            return View();
        }


        public PartialViewResult LoadOperationLog(string objectType, string objectId)
        {
            ViewBag.ObjectType = objectType;
            ViewBag.ObjectId = objectId;
            ViewBag.isObjectSummary = objectId == "0" ? false : true;
            return PartialView("OperationLog/_OperationLog");
        }


        public ActionResult Read([DataSourceRequest]DataSourceRequest request, string objectType, string objectId,
            string fromDate, string toDate, string operationType, string searchText, string userId)
        {
            try
            {
                string userName = "";
                if (!string.IsNullOrEmpty(userId))
                    userName = ApplicationUserManager.GetUser(userId).FullName;
                if (fromDate == null)
                    return Json(_operationLogBLL.GetAll(objectType, Convert.ToInt32(objectId)).ToDataSourceResult(request));
                else
                {
                    var result = _operationLogBLL.GetAll(objectType, Convert.ToInt32(objectId), DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture), Convert.ToInt32(operationType), searchText);
                    return Json(result.Where(c => c.UserName == userName || userId == "" || userId == null).ToDataSourceResult(request));
                }
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);
            }
        }

    }
}