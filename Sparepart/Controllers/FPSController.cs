using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Sparepart.Models;
namespace Sparepart.Controllers
{
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
            return View();
        }

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
        public ActionResult Detail_Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = Dbcontext.fpsdetails.ToDataSourceResult(request,
                fps => new fpsdetail
                {
                    FPSID = fps.FPSID,
                    SeqFPSID = fps.SeqFPSID,
                    KodeBarangTipe = fps.KodeBarangTipe,
                    Qty = fps.Qty,
                    Keterangan = fps.Keterangan
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Detail_Create(FPSDetailCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                fpsdetail detail = new fpsdetail();
                detail.KodeBarangTipe = model.KodeBarangTipe;
                detail.FPSID = model.FPSID;
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
    }
}