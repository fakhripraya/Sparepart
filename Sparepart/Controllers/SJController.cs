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
    public class SJController : Controller
    {
        dbsparepartEntities Dbcontext = new dbsparepartEntities();
        // GET: SJ
        public ActionResult Index()
        {
            Populate();
            return View();
        }

        private void Populate()
        {
            dbsparepartEntities Dbcontext = new dbsparepartEntities();
            var FPS = Dbcontext.fpsheaders
                        .Select(c => new FPSViewModel
                        {
                            FPSID = c.FPSID,
                        })
                        .OrderBy(e => e.FPSID);

            ViewData["FPS"] = FPS;
            ViewData["defaultFPS"] = FPS.First();
        }

        public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            ProductService x = new ProductService(Dbcontext);
            return Json(x.Read().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")]IEnumerable<SJCreateViewModel> products)
        {
            ProductService x = new ProductService(Dbcontext);
            var results = new List<SJCreateViewModel>();

            if (products != null && ModelState.IsValid)
            {
                foreach (var product in products)
                {
                    x.Create(product);

                    results.Add(product);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState), JsonRequestBehavior.AllowGet);
        }

        public ActionResult HierarchyBinding_FPS(int? FPSID, [DataSourceRequest] DataSourceRequest request)
        {

            var DetailFPS = (from det in Dbcontext.fpsdetails
                             join bar in Dbcontext.masterbarangs on det.KodeBarangTipe equals bar.KodeBarangTipe
                             where det.FPSID == FPSID
                             select new DetailPermintaanBarangViewModel
                             {
                                 FPSID = det.FPSID,
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
                    FPSID = fps.FPSID,
                    SeqFPSID = fps.SeqFPSID,
                    SatuanID = fps.SatuanID,
                    KodeBarangTipe = fps.KodeBarangTipe,
                    Quantity = fps.Quantity,
                    Keterangan = fps.Keterangan,
                    JumlahHarga = fps.JumlahHarga
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}