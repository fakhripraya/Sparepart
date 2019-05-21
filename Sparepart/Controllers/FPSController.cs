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
    [Authorize]
    public class FPSController : Controller
    {
        dbsparepartEntities Dbcontext = new dbsparepartEntities();
        // GET: FPS
        public ActionResult Index()
        {
            var toko = Dbcontext.mastertokoes.ToList();
            ViewData["Toko"] = toko;
            var unit = Dbcontext.masterunits.ToList();
            ViewData["Unit"] = unit;
            var cabang = Dbcontext.mastercabangs.ToList();
            ViewData["Cabang"] = cabang;
            var barang = Dbcontext.masterbarangs.ToList();
            ViewData["Barang"] = barang;
            var satuan = Dbcontext.mastersatuans.ToList();
            ViewData["Satuan"] = satuan;
            return View();
        }
        //Read
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = Dbcontext.fpsheaders.ToDataSourceResult(request,
                fps => new fpsheader
                {
                    FPSID = fps.FPSID,
                    UnitID = fps.UnitID,
                    TokoID = fps.TokoID,
                    CabangID = fps.CabangID,
                    NoTicket = fps.NoTicket,
                    NamaPemohon = fps.NamaPemohon,
                    NIKPemohon = fps.NIKPemohon,
                    JenisFPS = fps.JenisFPS,
                    Approval = fps.Approval,
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Destroy
        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, fpsheader model)
        {
            if (ModelState.IsValid)
            {
                using (var northwind = new dbsparepartEntities())
                {
                    var entity = northwind.fpsheaders.Where(u => u.FPSID == model.FPSID).FirstOrDefault();
                    northwind.fpsheaders.Remove(entity);
                    foreach (var detail in northwind.fpsdetails.Where(p => p.FPSID == model.FPSID))
                    {
                        northwind.fpsdetails.Remove(detail);
                    }
                    northwind.SaveChanges();
                }
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        //Create Master
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Mutasi(fpsheader model)
        {
            int CabangID = Int32.Parse(Request.Form["Cabang_CreateMasterMutasi"]);
            int UnitID = Int32.Parse(Request.Form["Unit_CreateMasterMutasi"]);
            if (ModelState.IsValid)
            {

                string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();

                fpsheader newfps = new fpsheader();
                newfps.NoTicket = model.NoTicket;
                newfps.NamaPemohon = model.NamaPemohon;
                newfps.NIKPemohon = model.NIKPemohon;
                newfps.CabangID = CabangID;
                newfps.UnitID = UnitID;
                newfps.UserID = id;
                newfps.Approval = "Pending";
                newfps.JenisFPS = "Mutasi";
                newfps.UserInput = CurrentUser.NamaUser;
                newfps.TanggalInput = System.DateTime.Now;
                Dbcontext.fpsheaders.Add(newfps);
                foreach (var detail in Dbcontext.fpsdetails.Where(p => p.FPSID == null))
                {
                    detail.FPSID = newfps.FPSID;
                }

                Dbcontext.SaveChanges();
            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Perbaikan(fpsheader model)
        {
            int TokoID = Int32.Parse(Request.Form["Toko_CreateMasterPerbaikan"]);
            if (ModelState.IsValid)
            {

                string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
                masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();

                fpsheader newfps = new fpsheader();
                newfps.NoTicket = model.NoTicket;
                newfps.NamaPemohon = model.NamaPemohon;
                newfps.NIKPemohon = model.NIKPemohon;
                newfps.TokoID = TokoID;
                newfps.UserID = id;
                newfps.Approval = "Pending";
                newfps.JenisFPS = "Perbaikan";
                newfps.UserInput = CurrentUser.NamaUser;
                newfps.TanggalInput = System.DateTime.Now;
                Dbcontext.fpsheaders.Add(newfps);
                foreach (var detail in Dbcontext.fpsdetails.Where(p => p.FPSID == null))
                {
                    detail.FPSID = newfps.FPSID;
                }

                Dbcontext.SaveChanges();
            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Index");
        }
        //Create Detail in Create FPS Create Window
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Window_Detail_Create(FPSDetailCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                //string KodeBarangTipe = Request.Form["barang"];
                var barang = Dbcontext.masterbarangs.Where(k => k.KodeBarangTipe == model.KodeBarangTipe).FirstOrDefault();
                fpsdetail detail = new fpsdetail();
                detail.KodeBarangTipe = model.KodeBarangTipe;
                detail.SatuanID = barang.SatuanID;
                detail.Qty = model.Quantity;
                detail.Keterangan = model.Keterangan;
                detail.DeleteCheck = 0;
                Dbcontext.fpsdetails.Add(detail);
                Dbcontext.SaveChanges();
                var toko = Dbcontext.mastertokoes.ToList();
                ViewData["Toko"] = toko;
                var unit = Dbcontext.masterunits.ToList();
                ViewData["Unit"] = unit;
                var cabang = Dbcontext.mastercabangs.ToList();
                ViewData["Cabang"] = cabang;
                var barangS = Dbcontext.masterbarangs.ToList();
                ViewData["Barang"] = barangS;
                var satuan = Dbcontext.mastersatuans.ToList();
                ViewData["Satuan"] = satuan;
                return Json(new { success = true, responseText = "Berhasil Menambah Barang." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, responseText = "Gagal Menambah Barang." }, JsonRequestBehavior.AllowGet);
        }
        //Read Detail in Create FPS Create Window
        public ActionResult Create_Window_Detail_Read([DataSourceRequest]DataSourceRequest request)
        {
            var DetailFPS = (from det in Dbcontext.fpsdetails
                              join bar in Dbcontext.masterbarangs on det.KodeBarangTipe equals bar.KodeBarangTipe
                              where det.FPSID == null
                              select new DetailPermintaanBarangViewModel
                              {
                                  SeqFPSID = det.SeqFPSID,
                                  KodeBarangTipe = det.KodeBarangTipe,
                                  SatuanID = det.SatuanID,
                                  Keterangan = det.Keterangan,
                                  Quantity = det.Qty,
                                  JumlahHarga = det.Qty * bar.HargaSatuan,
                              }).ToList();

            DataSourceResult result = DetailFPS.ToDataSourceResult(request,
                fps => new DetailPermintaanBarangViewModel
                {
                    SeqFPSID = fps.SeqFPSID,
                    SatuanID = fps.SatuanID,
                    KodeBarangTipe = fps.KodeBarangTipe,
                    Quantity = fps.Quantity,
                    Keterangan = fps.Keterangan,
                    JumlahHarga = fps.JumlahHarga
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Destroy Detail in Create FPS Create Window
        public ActionResult Create_Window_Detail_Destroy([DataSourceRequest]DataSourceRequest request, fpsdetail det)
        {
            var detail = Dbcontext.fpsdetails.Where(p => p.SeqFPSID == det.SeqFPSID).FirstOrDefault();
            Dbcontext.fpsdetails.Remove(detail);
            Dbcontext.SaveChanges();
            //// If we got this far, something failed, redisplay form
            return RedirectToAction("Index");
        }
        //Update
        //Create Detail In FPS Update Window
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_Window_Detail_Create(FPSDetailCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                //string KodeBarangTipe = Request.Form["barang"];
                var barang = Dbcontext.masterbarangs.Where(k => k.KodeBarangTipe == model.KodeBarangTipe).FirstOrDefault();
                fpsdetail detail = new fpsdetail();
                detail.KodeBarangTipe = model.KodeBarangTipe;
                detail.SatuanID = barang.SatuanID;
                detail.Qty = model.Quantity;
                detail.Keterangan = model.Keterangan;
                detail.DeleteCheck = 0;
                Dbcontext.fpsdetails.Add(detail);
                Dbcontext.SaveChanges();
                var toko = Dbcontext.mastertokoes.ToList();
                ViewData["Toko"] = toko;
                var unit = Dbcontext.masterunits.ToList();
                ViewData["Unit"] = unit;
                var cabang = Dbcontext.mastercabangs.ToList();
                ViewData["Cabang"] = cabang;
                var barangS = Dbcontext.masterbarangs.ToList();
                ViewData["Barang"] = barangS;
                var satuan = Dbcontext.mastersatuans.ToList();
                ViewData["Satuan"] = satuan;
                return Json(new { success = true, responseText = "Berhasil Menambah Barang." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, responseText = "Gagal Menambah Barang." }, JsonRequestBehavior.AllowGet);
        }
        //Read Detail In FPS Update Window
        public ActionResult Edit_Window_Detail_Read([DataSourceRequest]DataSourceRequest request,int? FPSID)
        {
            var DetailFPS = (from det in Dbcontext.fpsdetails
                             join bar in Dbcontext.masterbarangs on det.KodeBarangTipe equals bar.KodeBarangTipe
                             where det.FPSID == FPSID && det.DeleteCheck == 0 || det.FPSID == null && det.DeleteCheck == 0
                             select new DetailPermintaanBarangViewModel
                             {
                                 SeqFPSID = det.SeqFPSID,
                                 KodeBarangTipe = det.KodeBarangTipe,
                                 SatuanID = det.SatuanID,
                                 Keterangan = det.Keterangan,
                                 Quantity = det.Qty,
                                 JumlahHarga = det.Qty * bar.HargaSatuan,
                             }).ToList();

            DataSourceResult result = DetailFPS.ToDataSourceResult(request,
                fps => new DetailPermintaanBarangViewModel
                {
                    SeqFPSID = fps.SeqFPSID,
                    SatuanID = fps.SatuanID,
                    KodeBarangTipe = fps.KodeBarangTipe,
                    Quantity = fps.Quantity,
                    Keterangan = fps.Keterangan,
                    JumlahHarga = fps.JumlahHarga
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //Destroy Detail In FPS Update Window
        public ActionResult Edit_Window_Detail_Destroy([DataSourceRequest]DataSourceRequest request, fpsdetail det)
        {
            var detail = Dbcontext.fpsdetails.Where(p => p.SeqFPSID == det.SeqFPSID).FirstOrDefault();
            detail.DeleteCheck = 1;
            Dbcontext.SaveChanges();
            //// If we got this far, something failed, redisplay form
            return RedirectToAction("Index");
        }
    }
}