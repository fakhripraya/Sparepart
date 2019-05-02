using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Sparepart.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sparepart.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UnitController : Controller
    {
        dbsparepartEntities Dbcontext = new dbsparepartEntities();
        // GET: Unit
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Unit_Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = Dbcontext.masterunits.Where(x => x.IsDelete == 0).ToDataSourceResult(request,
                model => new masterunit
                {
                    UnitID = model.UnitID,
                    NamaUnit = model.NamaUnit,
                    UserInput = model.UserInput,
                    TanggalInput = model.TanggalInput,
                    UserUpdate = model.UserUpdate,
                    TanggalUpdate = model.TanggalUpdate,
                    IsDelete = model.IsDelete
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Unit_Create([DataSourceRequest]DataSourceRequest request, masterunit unit)
        {
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                using (var db = new dbsparepartEntities())
                {
                    var entity = new masterunit
                    {
                        UnitID = unit.UnitID,
                        NamaUnit = unit.NamaUnit,
                        UserInput = CurrentUser.NamaUser,
                        TanggalInput = DateTime.Now,
                        IsDelete = 0
                    };
                    db.masterunits.Add(entity);
                    db.SaveChanges();
                    unit.UnitID = entity.UnitID;
                }
            }
            return Json(new[] { unit }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult Unit_Update([DataSourceRequest]DataSourceRequest request, masterunit unit)
        {
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            masteruser CurrentUser = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                using (var northwind = new dbsparepartEntities())
                {
                    var entity = northwind.masterunits.Where(u => u.UnitID == unit.UnitID).FirstOrDefault();
                    entity.NamaUnit = unit.NamaUnit;
                    entity.UserUpdate = CurrentUser.NamaUser;
                    entity.TanggalUpdate = DateTime.Now;
                    northwind.SaveChanges();
                }
            }
            return Json(new[] { unit }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Unit_Destroy([DataSourceRequest]DataSourceRequest request, masterunit unit)
        {
            if (ModelState.IsValid)
            {
                using (var northwind = new dbsparepartEntities())
                {
                    var entity = northwind.masterunits.Where(u => u.UnitID == unit.UnitID).FirstOrDefault();
                    entity.IsDelete = 1;
                    northwind.SaveChanges();
                }
            }
            return Json(new[] { unit }.ToDataSourceResult(request, ModelState));
        }

        public JsonResult GetUnit(string text)
        {
            var northwind = new dbsparepartEntities();

            var products = northwind.masterunits.Where(x => x.IsDelete == 0).Select(product => new UnitViewModel
            {
                UnitID = product.UnitID,
                NamaUnit = product.NamaUnit,
            });

            if (!string.IsNullOrEmpty(text))
            {
                products = products.Where(p => p.NamaUnit.Contains(text));
            }

            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}