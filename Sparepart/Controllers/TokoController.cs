using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Sparepart.Models;

namespace Sparepart.Controllers
{
    [Authorize(Roles = "MasterToko")]
    public class TokoController : Controller
    {
        dbsparepartEntities Dbcontext = new dbsparepartEntities();
        // GET: Toko
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Toko_Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = Dbcontext.mastertokoes.Where(x => x.IsDelete == 0).ToDataSourceResult(request,
                model => new mastertoko
                {
                    TokoID = model.TokoID,
                    NamaToko = model.NamaToko,
                    AlamatToko = model.AlamatToko
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Toko_Create(CreateTokoViewModel model)
        {
            if (ModelState.IsValid)
            {
                string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();

                mastertoko toko = new mastertoko();
                toko.TokoID = model.TokoID;
                toko.NamaToko = model.NamaToko;
                toko.AlamatToko = model.AlamatToko;
                toko.UserInput = CurrentUser.NamaUser;
                toko.TanggalInput = System.DateTime.Now;
                toko.IsDelete = 0;
                Dbcontext.mastertokoes.Add(toko);
                Dbcontext.SaveChanges();
            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Toko_Update(CreateTokoViewModel toko)
        {
            if (ModelState.IsValid)
            {
                string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();
                using (var northwind = new dbsparepartEntities())
                {
                    var entity = northwind.mastertokoes.Where(u => u.TokoID == toko.TokoID).FirstOrDefault();
                    entity.NamaToko = toko.NamaToko;
                    entity.AlamatToko = toko.AlamatToko;
                    entity.UserUpdate = CurrentUser.NamaUser;
                    entity.TanggalUpdate = DateTime.Now;
                    northwind.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Toko_Destroy([DataSourceRequest]DataSourceRequest request,mastertoko toko)
        {
            if (ModelState.IsValid)
            {
                using (var northwind = new dbsparepartEntities())
                {
                    var entity = northwind.mastertokoes.Where(u => u.TokoID == toko.TokoID).FirstOrDefault();
                    entity.IsDelete = 1;
                    northwind.SaveChanges();
                }
            }
            return Json(new[] { toko }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult GetToko(string text)
        {
            var northwind = new dbsparepartEntities();

            var products = northwind.mastertokoes.Where(x => x.IsDelete == 0).Select(product => new TokoViewModel
            {
                TokoID = product.TokoID,
                NamaToko = product.NamaToko
            });

            if (!string.IsNullOrEmpty(text))
            {
                products = products.Where(p => p.NamaToko.Contains(text));
            }

            return Json(products, JsonRequestBehavior.AllowGet);
        }

    }
}