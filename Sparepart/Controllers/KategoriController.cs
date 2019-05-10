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
    [Authorize(Roles = "MasterKategori")]
    public class KategoriController : Controller
    {
        dbsparepartEntities Dbcontext = new dbsparepartEntities();
        // GET: Kategori
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = Dbcontext.masterkategoris.Where(x => x.IsDelete == 0).ToDataSourceResult(request,
                model => new masterkategori
                {
                    KategoriID = model.KategoriID,
                    NamaKategori = model.NamaKategori,
                    Deskripsi = model.Deskripsi
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateKategoriViewModel kat)
        {
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                using (var db = new dbsparepartEntities())
                {
                    var entity = new masterkategori
                    {
                        KategoriID = kat.KategoriID,
                        NamaKategori = kat.NamaKategori,
                        Deskripsi = kat.Deskripsi,
                        UserInput = CurrentUser.NamaUser,
                        TanggalInput = DateTime.Now,
                        IsDelete = 0
                    };
                    db.masterkategoris.Add(entity);
                    db.SaveChanges();
                    kat.KategoriID = entity.KategoriID;
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CreateKategoriViewModel kat)
        {
            if (ModelState.IsValid)
            {
                string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();
                using (var northwind = new dbsparepartEntities())
                {
                    var entity = northwind.masterkategoris.Where(u => u.KategoriID == kat.KategoriID).FirstOrDefault();
                    entity.NamaKategori = kat.NamaKategori;
                    entity.Deskripsi = kat.Deskripsi;
                    entity.UserUpdate = CurrentUser.NamaUser;
                    entity.TanggalUpdate = DateTime.Now;
                    northwind.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, masterkategori kat)
        {
            if (ModelState.IsValid)
            {
                using (var northwind = new dbsparepartEntities())
                {
                    var entity = northwind.masterkategoris.Where(u => u.KategoriID == kat.KategoriID).FirstOrDefault();
                    entity.IsDelete = 1;
                    northwind.SaveChanges();
                }
            }
            return Json(new[] { kat }.ToDataSourceResult(request, ModelState));
        }
        public JsonResult GetKategori(string text)
        {
            var northwind = new dbsparepartEntities();

            var products = northwind.masterkategoris.Where(x => x.IsDelete == 0).Select(product => new KategoriViewModel
            {
                KategoriID = product.KategoriID,
                NamaKategori = product.NamaKategori,
            });

            if (!string.IsNullOrEmpty(text))
            {
                products = products.Where(p => p.NamaKategori.Contains(text));
            }

            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}