using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Sparepart.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;

namespace Sparepart.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccessController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext Context;
        dbsparepartEntities Dbcontext = new dbsparepartEntities();
        public AccessController()
        {
            Context = new ApplicationDbContext();
        }

        public AccessController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Access
        public ActionResult Index()
        {
            //if (TempData.ContainsKey("flag"))
            //{
            //    ViewBag.Message = "aa";
            //}
            ViewBag.hakakses = Dbcontext.hakakses.ToList();
            var menu = Dbcontext.hakakses.ToList();
            ViewData["Menu"] = menu;
            return View();
        }

        public ActionResult Access_Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = Dbcontext.hakakses.ToDataSourceResult(request,
                model => new hakaks
                {
                    AksesID = model.AksesID,
                    NamaMenu = model.NamaMenu,
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = Dbcontext.akses.ToDataSourceResult(request,
                akses => new aks
                {
                    ID = akses.ID,
                    RoleID = akses.RoleID,
                    AksesID = akses.AksesID
                });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetRoles()
        //{
        //    dbsparepartEntities db = new dbsparepartEntities();
        //    return Json(db.masterroles.Select(x => new
        //    {
        //        RoleID = x.RoleID,
        //        NamaRole = x.NamaRole
        //    }).ToList(), JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAccess(int[] AksesIDs,masterrole role)
        {
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            masteruser user = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                string rolename = Request.Form["nama"];
                using (var db = new dbsparepartEntities())
                {
                    //var entity = new masterrole
                    //{
                    //    RoleID = role.RoleID,
                    //    NamaRole = rolename,
                    //    UserInput = user.NamaUser,
                    //    TanggalInput = DateTime.Now,
                    //    IsDelete = 0
                    //};
                    masterrole entity = new masterrole();
                    entity.RoleID = role.RoleID;
                    entity.NamaRole = rolename;
                    entity.UserInput = user.NamaUser;
                    entity.TanggalInput = DateTime.Now;
                    entity.IsDelete = 0;
                    db.masterroles.Add(entity);
                    db.SaveChanges();
                    role.RoleID = entity.RoleID;
                }

                foreach (int akses in AksesIDs)
                {
                    aks aksmodel = new aks();
                    hakaks selected = Dbcontext.hakakses.Where(x => x.AksesID == akses).FirstOrDefault();
                    aksmodel.AksesID = selected.AksesID;
                    aksmodel.RoleID = role.RoleID;
                    aksmodel.NamaAkses = selected.NamaMenu;
                    //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    //IdentityRole AspRole = new IdentityRole();
                    //AspRole.Name = selected.NamaMenu;
                    //context.Roles.Add(AspRole);
                    //context.SaveChanges();

                    Dbcontext.akses.Add(aksmodel);
                    Dbcontext.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int[] AksesIDs, masterrole role)
        {
            string id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            masteruser user = Dbcontext.masterusers.Where(x => x.UserID == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                string rolename = Request.Form["nama"];
                using (var db = new dbsparepartEntities())
                {
                    var entity = db.masterroles.Where(u => u.RoleID == role.RoleID).FirstOrDefault();
                    entity.NamaRole = rolename;
                    entity.UserUpdate = user.NamaUser;
                    entity.TanggalUpdate = DateTime.Now;
                    db.SaveChanges();
                }

                //foreach (var value in Dbcontext.akses.Where(x => x.RoleID == role.RoleID))
                ///*{*/
                Dbcontext.akses.RemoveRange(Dbcontext.akses.Where(x => x.RoleID == role.RoleID));
                Dbcontext.SaveChanges();
                //}

                foreach (int akses in AksesIDs)
                {
                    aks aksmodel = new aks();
                    hakaks selected = Dbcontext.hakakses.Where(x => x.AksesID == akses).FirstOrDefault();
                    aksmodel.AksesID = selected.AksesID;
                    aksmodel.RoleID = role.RoleID;
                    aksmodel.NamaAkses = selected.NamaMenu;

                    Dbcontext.akses.Add(aksmodel);
                    Dbcontext.SaveChanges();
                }

                string[] selectedAccess = Dbcontext.akses.AsEnumerable().Where(r => r.RoleID == role.RoleID).Select(r => r.NamaAkses).ToArray();

                foreach (var SelectedUser in Dbcontext.masterusers.Where(x => x.RoleID == role.RoleID))
                { 
                    ApplicationUser aspuser = UserManager.FindByIdAsync(SelectedUser.UserID).Result;
                    UserManager.RemoveFromRolesAsync(aspuser.Id, UserManager.GetRoles(aspuser.Id).ToArray());
                    UserManager.AddToRolesAsync(aspuser.Id, selectedAccess);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, masterrole role)
        {
            if (ModelState.IsValid)
            {
                using (var db = new dbsparepartEntities())
                {
                    //masteruser user = db.masterusers.Where(p => p.RoleID == role.RoleID).FirstOrDefault();
                    //if(user != null)
                    //{
                    //    TempData["flag"] = 1;
                    //    return RedirectToAction("Index");
                    //}

                    aks deleteakses = db.akses.Where(a => a.RoleID == role.RoleID).FirstOrDefault();
                    db.akses.Remove(deleteakses);
                    db.SaveChanges();
                    masterrole deleterole = db.masterroles.Where(a => a.RoleID == role.RoleID).FirstOrDefault();
                    deleterole.IsDelete = 1;
                    db.SaveChanges();
                }
            }
            return Json(new[] { role }.ToDataSourceResult(request, ModelState));
        }

    }
}