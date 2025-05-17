using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.WebApplication.App_Start;
using Kendo.Mvc.UI;
using Repository.Pattern;
using System;
using System.Web.Mvc;
using TripleA.Utilities.Exceptions;
using BoulevardManagement.DTO;
using BoulevardManagement.WebApplication.Resources;
using System.Web.Routing;

namespace BoulevardManagement.WebApplication.Controllers
{
    [HandleError]
    [Authorize]
    public class BaseController : Controller
    {

        IErrorLogBLL _errorLogBLL;
        public BaseController(IErrorLogBLL errorLogBLL)
        {
            _errorLogBLL = errorLogBLL;
        }

        //public BaseController()
        //{

        //}

        [HttpPost]
        public ActionResult Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }


        public ApplicationUserData GetCurrentUser
        {
            get
            {
                return new ApplicationContext().GetApplicationUserData();
            }
        }




        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    base.OnException(filterContext);
        //}
        protected override void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception;
            string controller = filterContext.RouteData.Values["controller"].ToString();
            string action = filterContext.RouteData.Values["action"].ToString();
            bool isAjax = filterContext.HttpContext.Request.IsAjaxRequest();
            //bool isKendoCall = filterContext.HttpContext.Request.Headers.AllKeys.Where(k => k == "is_kendorequest").Any();
            string errorReferenceNo = ex.Message;

            filterContext.ExceptionHandled = true;

            var code = LogError(this, ex);
            errorReferenceNo = string.Format("{0} [Error Ref: {1}]", errorReferenceNo, code);
            if (isAjax)
            {
                //filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                string ErrorMessage = code + Environment.NewLine + ex.Message;


                if (ex is KendoException)
                {
                    ErrorMessage = ((KendoException)ex).OriginalException.Message;

                    DataSourceResult DSR = new DataSourceResult() { Errors = code + Environment.NewLine + ErrorMessage };

                    filterContext.Result = new JsonResult
                    {
                        Data = DSR,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    };
                }
                else
                {
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            success = false,
                            error = true,
                            message = ErrorMessage
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }

            }
            else
            {
                if (filterContext.Exception.ToString().Contains("The provided anti-forgery token was meant for a different claims-based user than the current user."))
                {
                    filterContext.Result = new RedirectToRouteResult(
                 new RouteValueDictionary
                 {
                    { "controller", "Account" },
                    { "action", "LogOffBySys" }
                 }
                 
                 );
            }
                else
                {
                    var viewData = new ViewDataDictionary<HandleErrorInfo>();
                    viewData.Model = new HandleErrorInfo(filterContext.Exception, controller, action);
                    viewData.Add("Error", ex.Message);
                    viewData.Add("Code", code);

                    filterContext.Result = new ViewResult
                    {
                        ViewName = "Error",
                        MasterName = "",
                        ViewData = viewData,
                        TempData = filterContext.Controller.TempData
                    };

                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.StatusCode = 500;
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                }
             
            }
        }

        protected string LogError(BaseController controller, Exception ex)
        {
            var error = new ErrorLogDTO
            {
                Message = ex.Message,
                Details = ex.ToString(),
                UserName = GetCurrentUser.FullName,
                Location = controller.GetType().Name.ToString()
            };
            if (_errorLogBLL != null)
            {
                _errorLogBLL.Insert(error);
             
                return error.Code;
            }
            else
                return string.Empty;
        }

        protected void ShowInfoMessage(string message)
        {
            TempData["InfoMesage"] = message;
        }

        protected void ShowErrorMessage(string message)
        {
            TempData["ErrorMesage"] = message;
        }


        protected void ShowSuccessfullyAdded(string tag = null)
        {
            if (string.IsNullOrEmpty(tag))
                ShowInfoMessage(CommonResource.SuccessfullyAddedMSG);
            else
                ShowInfoMessage($"Adding {tag} ,Has been completed successfully");
        }

        protected void ShowSuccessfullyUpdated(string tag = null)
        {
            if (string.IsNullOrEmpty(tag))
                ShowInfoMessage(CommonResource.SuccessfullyUpdatedMSG);
            else
                ShowInfoMessage($"Editing {tag} ,Has been completed successfully");
        }

        protected void ShowSuccessfullyDeleted(string tag = null)
        {
            if (string.IsNullOrEmpty(tag))
                ShowInfoMessage(CommonResource.SuccessfullyDeletedMSG);
            else
                ShowInfoMessage($"Deleting {tag} ,Has been completed successfully");
        }

        protected string GetErrorMessage(Exception ex)
        {
            try
            {
                if (ex.InnerException != null)
                    return GetErrorMessage(ex.InnerException);
                else return ex.Message;
            }
            catch (Exception)
            {

                throw;
            }
        }


       

    }
}