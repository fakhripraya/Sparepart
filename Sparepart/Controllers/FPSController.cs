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
    }
}