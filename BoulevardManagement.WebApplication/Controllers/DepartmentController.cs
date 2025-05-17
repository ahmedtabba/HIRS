using BoulevardManagement.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TripleA.Utilities.Exceptions;

namespace BoulevardManagement.WebApplication.Controllers
{

    public class DepartmentController : BaseController
    {
        readonly private IDepartmentBLL _departmentBLL;
        public DepartmentController(
            IDepartmentBLL departmentBLL,
            IErrorLogBLL errorLogBLL) : base(errorLogBLL)
        {
            _departmentBLL = departmentBLL;
        }

      

        public JsonResult GetHasLocationsDepartments(string text)
        {
            try
            {
                var res = _departmentBLL.GetAll().Where(x=>x.HaveLocation);
                if (!string.IsNullOrWhiteSpace(text))
                {
                    //Text Filter Here
                    res = res.Where(x => x.Name.Contains(text)|| x.ArabicName.Contains(text)|| x.Code.Contains(text));
                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);

            }
        }


        public JsonResult GetDepartments(string text)
        {
            try
            {
                var res = _departmentBLL.GetAll();
                if (!string.IsNullOrWhiteSpace(text))
                {
                    //Text Filter Here
                    res = res.Where(x => x.Name.Contains(text) || x.ArabicName.Contains(text) || x.Code.Contains(text));
                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new KendoException(ex);

            }
        }

    }

}