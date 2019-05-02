using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sparepart.Models;

namespace Sparepart.Controllers
{
    public class WindowController : Controller
    {
        dbsparepartEntities Dbcontext = new dbsparepartEntities();
        // GET: Window
        public ActionResult CabangEdit(int? CabangID)
        {
            mastercabang cabang = Dbcontext.mastercabangs.Where(x => x.CabangID == CabangID).FirstOrDefault();

            var toko = Dbcontext.mastertokoes.ToList();
            ViewData["Toko"] = toko;
            var unit = Dbcontext.masterunits.ToList();
            ViewData["Unit"] = unit;
            if (CabangID != null)
            {
                CreateCabangViewModel cab = new CreateCabangViewModel();
                cab.CabangID = cabang.CabangID;
                return PartialView(cab);
            }
            else return View();
        }

        public ActionResult UserEdit(string UserID)
        {
            masteruser user = Dbcontext.masterusers.Where(x => x.UserID == UserID).FirstOrDefault();

            var role = Dbcontext.masterroles.ToList();
            ViewData["Role"] = role;
            if (UserID != null)
            {
                UserViewModel edituser = new UserViewModel();
                edituser.UserID = user.UserID;
                return PartialView(edituser);
            }
            else return View();
        }
        public ActionResult BarangEdit(string KodeBarangTipe)
        {
            masterbarang barang = Dbcontext.masterbarangs.Where(x => x.KodeBarangTipe == KodeBarangTipe).FirstOrDefault();

            var kategori = Dbcontext.masterkategoris.ToList();
            ViewData["Kategori"] = kategori;
            var satuan = Dbcontext.mastersatuans.ToList();
            ViewData["Satuan"] = satuan;
            if (KodeBarangTipe != null)
            {
                CreateBarangViewModel bar = new CreateBarangViewModel();
                bar.KodeBarangTipe = barang.KodeBarangTipe;
                return PartialView(bar);
            }
            else return View();
        }
        public ActionResult RoleDetail(int? RoleID)
        {
            masterrole role = Dbcontext.masterroles.Where(x => x.RoleID == RoleID).FirstOrDefault();
            if (RoleID != null)
            {
                ViewBag.Akses = Dbcontext.akses.Where(x => x.RoleID == RoleID).ToList();
                masterrole rol = new masterrole();
                rol.RoleID = role.RoleID;
                rol.NamaRole = role.NamaRole;
                var menu = Dbcontext.hakakses.ToList();
                ViewData["Menu"] = menu;
                return PartialView(rol);
            }
            else return View();
        }

        public ActionResult RoleEdit(int? RoleID)
        {
            masterrole role = Dbcontext.masterroles.Where(x => x.RoleID == RoleID).FirstOrDefault();
            if (RoleID != null)
            {
                ViewBag.hakakses = Dbcontext.hakakses.ToList();
                masterrole rol = new masterrole();
                rol.RoleID = role.RoleID;
                return PartialView(rol);
            }
            else return View();
        }
    }
}