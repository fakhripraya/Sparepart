using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
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
    public class RoleController : Controller
    {
        ApplicationDbContext Context = new ApplicationDbContext();
        dbsparepartEntities Dbcontext = new dbsparepartEntities();
        // GET: Role
        public ActionResult Index()
        {
            var data = new dbsparepartEntities();
            ViewBag.Roles = data.masterroles;
            return View();
        }

        public ActionResult Roles_Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = Dbcontext.masterroles.Where(x => x.IsDelete == 0).ToDataSourceResult(request,
                model => new masterrole
                {
                    RoleID = model.RoleID,
                    NamaRole = model.NamaRole,
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Roles_Create([DataSourceRequest]DataSourceRequest request, masterrole role)
        {
            if (ModelState.IsValid)
            {
                using (var db = new dbsparepartEntities())
                {
                    var entity = new masterrole
                    {
                        RoleID = role.RoleID,
                        NamaRole = role.NamaRole
                    };
                    db.masterroles.Add(entity);
                    db.SaveChanges();
                    role.RoleID = entity.RoleID;
                }
            }
            return Json(new[] { role }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Roles_Update([DataSourceRequest]DataSourceRequest request, masterrole role)
        {
            if (ModelState.IsValid)
            {
                using (var northwind = new dbsparepartEntities())
                {
                    var entity = new masterrole
                    {
                        RoleID = role.RoleID,
                        NamaRole = role.NamaRole
                    };
                    northwind.masterroles.Attach(entity);
                    northwind.Entry(entity).State = EntityState.Modified;
                    northwind.SaveChanges();
                }
            }
            return Json(new[] { role }.ToDataSourceResult(request, ModelState));
        }
        [HttpPost]
        public ActionResult Roles_Destroy([DataSourceRequest]DataSourceRequest request, masterrole role)
        {
            if (ModelState.IsValid)
            {
                using (var db = new dbsparepartEntities())
                {
                    var entity = new masterrole
                    {
                        RoleID = role.RoleID,
                        NamaRole = role.NamaRole
                    };
                    db.masterroles.Attach(entity);
                    db.masterroles.Remove(entity);
                    db.SaveChanges();
                }
            }
            return Json(new[] { role }.ToDataSourceResult(request, ModelState));
        }

    }
}