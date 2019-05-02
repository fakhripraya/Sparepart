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
    public class BarangController : Controller
    {
        dbsparepartEntities Dbcontext = new dbsparepartEntities();
        // GET: Barang
        public ActionResult Index()
        {
            var toko = Dbcontext.masterkategoris.ToList();
            ViewData["Kategori"] = toko;
            var unit = Dbcontext.mastersatuans.ToList();
            ViewData["Satuan"] = unit;
            return View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = Dbcontext.masterbarangs.Where(x => x.IsDelete == 0).ToDataSourceResult(request,
                barang => new masterbarang
                {
                    KodeBarangTipe = barang.KodeBarangTipe,
                    BarangPLU = barang.BarangPLU,
                    HargaSatuan = barang.HargaSatuan,
                    BufferStock = barang.BufferStock,
                    NamaBarang = barang.NamaBarang,
                    KategoriID = barang.KategoriID,
                    SatuanID = barang.SatuanID
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateBarangViewModel model)
        {
            if (ModelState.IsValid)
            {
                string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();
                int SatuanID = Int32.Parse(Request.Form["satuan"]);
                int KategoriID = Int32.Parse(Request.Form["kategori"]);

                masterbarang barang = new masterbarang();
                barang.KodeBarangTipe = model.KodeBarangTipe;
                barang.BarangPLU = model.BarangPLU;
                barang.NamaBarang = model.NamaBarang;
                barang.BufferStock = model.BufferStock;
                barang.KategoriID = KategoriID;
                barang.SatuanID = SatuanID;
                barang.HargaSatuan = model.HargaSatuan;
                barang.UserInput = CurrentUser.NamaUser;
                barang.TanggalInput = System.DateTime.Now;
                barang.IsDelete = 0;
                Dbcontext.masterbarangs.Add(barang);
                Dbcontext.SaveChanges();
            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, CreateBarangViewModel model)
        {
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();
            int SatuanID = Int32.Parse(Request.Form["satuan"]);
            int KategoriID = Int32.Parse(Request.Form["kategori"]);
            if (ModelState.IsValid)
            {
                using (var northwind = new dbsparepartEntities())
                {
                    var entity = northwind.masterbarangs.Where(u => u.KodeBarangTipe == model.KodeBarangTipe).FirstOrDefault();
                    entity.NamaBarang = model.NamaBarang;
                    entity.BufferStock = model.BufferStock;
                    entity.KategoriID = KategoriID;
                    entity.SatuanID = SatuanID;
                    entity.HargaSatuan = model.HargaSatuan;
                    entity.UserUpdate = CurrentUser.NamaUser;
                    entity.TanggalUpdate = DateTime.Now;
                    northwind.SaveChanges();
                }
                return RedirectToAction("Index");
                ;
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, CreateBarangViewModel barang)
        {
            if (ModelState.IsValid)
            {
                using (var northwind = new dbsparepartEntities())
                {
                    var entity = northwind.masterbarangs.Where(u => u.KodeBarangTipe == barang.KodeBarangTipe).FirstOrDefault();
                    entity.IsDelete = 1;
                    northwind.SaveChanges();
                }
            }
            return Json(new[] { barang }.ToDataSourceResult(request, ModelState));
        }
    }
}