using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Sparepart.Models;

namespace Sparepart.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SatuanController : Controller
    {
        dbsparepartEntities Dbcontext = new dbsparepartEntities();
        // GET: Satuan
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = Dbcontext.mastersatuans.Where(x => x.IsDelete == 0).ToDataSourceResult(request,
                model => new mastersatuan
                {
                    SatuanID = model.SatuanID,
                    NamaSatuan = model.NamaSatuan
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(CreateSatuanViewModel sat)
        {
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                using (var db = new dbsparepartEntities())
                {
                    var entity = new mastersatuan
                    {
                        SatuanID = sat.SatuanID,
                        NamaSatuan = sat.NamaSatuan,
                        UserInput = CurrentUser.NamaUser,
                        TanggalInput = DateTime.Now,
                        IsDelete = 0
                    };
                    db.mastersatuans.Add(entity);
                    db.SaveChanges();
                    sat.SatuanID = entity.SatuanID;
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Update(CreateSatuanViewModel sat)
        {
            if (ModelState.IsValid)
            {
                string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();
                using (var northwind = new dbsparepartEntities())
                {
                    var entity = northwind.mastersatuans.Where(u => u.SatuanID== sat.SatuanID).FirstOrDefault();
                    entity.NamaSatuan = sat.NamaSatuan;
                    entity.UserUpdate = CurrentUser.NamaUser;
                    entity.TanggalUpdate = DateTime.Now;
                    northwind.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, mastersatuan sat)
        {
            if (ModelState.IsValid)
            {
                using (var northwind = new dbsparepartEntities())
                {
                    var entity = northwind.mastersatuans.Where(u => u.SatuanID == sat.SatuanID).FirstOrDefault();
                    entity.IsDelete = 1;
                    northwind.SaveChanges();
                }
            }
            return Json(new[] { sat }.ToDataSourceResult(request, ModelState));
        }
        public JsonResult GetSatuan(string text)
        {
            var northwind = new dbsparepartEntities();

            var products = northwind.mastersatuans.Where(x => x.IsDelete == 0).Select(product => new SatuanViewModel
            {
                SatuanID = product.SatuanID,
                NamaSatuan = product.NamaSatuan,
            });

            if (!string.IsNullOrEmpty(text))
            {
                products = products.Where(p => p.NamaSatuan.Contains(text));
            }

            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}