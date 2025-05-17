using BoulevardManagement.BLL.Interfaces;
using BoulevardManagement.Utilities;
using BoulevardManagement.WebApplication.Models.Admin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoulevardManagement.Model.Entities;
using Repository.Pattern.Infrastructure;

namespace BoulevardManagement.WebApplication.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(IErrorLogBLL errorLogBLL) : base(errorLogBLL)
        {
        }

        // GET: Admin
        public ActionResult Scripts()
        {
            return View();
        }

        public ActionResult ReadDiagnosisFile()
        {
            try
            {
                if (System.IO.File.Exists(Path.Combine(Configurations.CsvFilePhysicalPath, "DiagnosisFile.json")))
                {
                    var context = new BoulevardManagementContext();
                    var rawJson = System.IO.File.ReadAllText(Path.Combine(Configurations.CsvFilePhysicalPath, "DiagnosisFile.json"));
                    var model = JsonConvert.DeserializeObject<List<DiagnosisCategoryVM>>(rawJson);

                    var categoriesEn = model.Select(c => c.DiagnosisName).Distinct().ToList();
                    var categoriesAr = model.Select(c => c.DiagnosisArabicName).Distinct().ToList();

                    for (int i = 0; i < categoriesEn.Count; i++)
                    {
                        var catName = categoriesEn[i];
                        if (!context.TelMHDiagnosisCategories.Where(x => x.Name == catName).Any())
                        {
                            context.TelMHDiagnosisCategories.Add(new TelMHDiagnosisCategory { Name = categoriesEn[i], ArabicName = categoriesAr[i], ObjectState = ObjectState.Added, CreatedBy = GetCurrentUser.UserId, CreationDate = DateTime.Now });
                            context.SaveChanges();
                            var subCategories = model.Where(x => x.DiagnosisName == categoriesEn[i]).ToList();

                            foreach (var subCat in subCategories)
                            {

                                if (!context.TelMHDiagnosisSubCategories.Where(x => x.Name == subCat.SubDiagnosisName).Any())
                                {
                                    var catId = context.TelMHDiagnosisCategories.Where(x => x.Name == subCat.DiagnosisName).Select(x => x.Id).FirstOrDefault();
                                    context.TelMHDiagnosisSubCategories.Add(new TelMHDiagnosisSubCategory { Name = subCat.SubDiagnosisName, ArabicName = subCat.SubDiagnosisArabicName, TelMHDiagnosisCategoryId = catId, ObjectState = ObjectState.Added, CreatedBy = GetCurrentUser.UserId, CreationDate = DateTime.Now });
                                    context.SaveChanges();
                                }
                            }

                        }
                    }

                }
                ShowInfoMessage("Done");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult ReadMostLikelyDiagnosisFile()
        {
            try
            {
                if (System.IO.File.Exists(Path.Combine(Configurations.CsvFilePhysicalPath, "MostLikelyDiagnosisFile.json")))
                {
                    var context = new BoulevardManagementContext();
                    var rawJson = System.IO.File.ReadAllText(Path.Combine(Configurations.CsvFilePhysicalPath, "MostLikelyDiagnosisFile.json"));
                    var model = JsonConvert.DeserializeObject<List<MostLikelyDiagnosisVM>>(rawJson);

                    var categoriesEn = model.Select(c => c.Name).Distinct().ToList();

                    for (int i = 0; i < categoriesEn.Count; i++)
                    {
                        var catName = categoriesEn[i];
                        if (!context.MostLikelyDiagnoses.Where(x => x.Name == catName).Any())
                        {
                            context.MostLikelyDiagnoses.Add(new Medication { Name = categoriesEn[i], ObjectState = ObjectState.Added, CreatedBy = GetCurrentUser.UserId, CreationDate = DateTime.Now });
                            context.SaveChanges();
                           

                        }
                    }

                }
                ShowInfoMessage("Done");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}