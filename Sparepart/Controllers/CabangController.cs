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
    [Authorize(Roles = "MasterCabang")]
    public class CabangController : Controller
    {
        dbsparepartEntities Dbcontext = new dbsparepartEntities();
        // GET: Cabang
        public ActionResult Index()
        {
            var toko = Dbcontext.mastertokoes.ToList();
            ViewData["Toko"] = toko;
            var unit = Dbcontext.masterunits.ToList();
            ViewData["Unit"] = unit;
            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = Dbcontext.mastercabangs.Where(x => x.IsDelete == 0).ToDataSourceResult(request,
                cabang => new mastercabang
                {
                    CabangID = cabang.CabangID,
                    NamaCabang = cabang.NamaCabang,
                    TokoID = cabang.TokoID,
                    UnitID = cabang.UnitID
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCabangViewModel model)
        {
            if (ModelState.IsValid)
            {
                string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();
                
                    mastercabang cabang = new mastercabang();
                    cabang.CabangID = model.CabangID;
                    cabang.NamaCabang = model.NamaCabang;
                    cabang.TokoID = Int32.Parse(model.Toko);
                    cabang.UnitID = Int32.Parse(model.Unit);
                    cabang.UserInput = CurrentUser.NamaUser;
                    cabang.TanggalInput = System.DateTime.Now;
                    cabang.IsDelete = 0;
                    Dbcontext.mastercabangs.Add(cabang);
                    Dbcontext.SaveChanges();
            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, CreateCabangViewModel model)
        {
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                using (var northwind = new dbsparepartEntities())
                {
                    var entity = northwind.mastercabangs.Where(u => u.CabangID == model.CabangID).FirstOrDefault();
                    entity.NamaCabang = model.NamaCabang;
                    entity.TokoID = Int32.Parse(model.Toko);
                    entity.UnitID = Int32.Parse(model.Unit);
                    entity.UserUpdate = CurrentUser.NamaUser;
                    entity.TanggalUpdate = DateTime.Now;
                    northwind.SaveChanges();
                }
                return RedirectToAction("Index", "Cabang");
;            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, mastercabang cabang)
        {
            if (ModelState.IsValid)
            {
                using (var northwind = new dbsparepartEntities())
                {
                    var entity = northwind.mastercabangs.Where(u => u.CabangID == cabang.CabangID).FirstOrDefault();
                    entity.IsDelete = 1;
                    northwind.SaveChanges();
                }
            }
            return Json(new[] { cabang }.ToDataSourceResult(request, ModelState));
        }
        public JsonResult GetCabang(string text)
        {
            var northwind = new dbsparepartEntities();

            var products = northwind.mastercabangs.Where(x => x.IsDelete == 0).Select(product => new CabangViewModel
            {
                CabangID = product.CabangID,
                NamaCabang = product.NamaCabang,
            });

            if (!string.IsNullOrEmpty(text))
            {
                products = products.Where(p => p.NamaCabang.Contains(text));
            }

            return Json(products, JsonRequestBehavior.AllowGet);
        }

    }
}