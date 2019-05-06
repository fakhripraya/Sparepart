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
                cab.NamaCabang = cabang.NamaCabang;
                cab.Toko = cabang.TokoID.ToString();
                cab.Unit = cabang.UnitID.ToString();
                return PartialView(cab);
            }
            else return View();
        }

        public ActionResult UserEdit(string UserID)
        {
            masteruser user = Dbcontext.masterusers.Where(x => x.UserID == UserID).FirstOrDefault();

            var role = Dbcontext.masterroles.ToList();
            ViewData["Role"] = role;
            var cabang = Dbcontext.mastercabangs.ToList();
            ViewData["Cabang"] = cabang;
            if (UserID != null)
            {
                RegisterViewModel edituser = new RegisterViewModel();
                edituser.UserID = user.UserID;
                edituser.NamaUser = user.NamaUser;
                edituser.UserName = user.Username;
                edituser.Email = user.Email;
                edituser.UserRoles = user.RoleID.ToString();
                edituser.UserCabang = user.CabangID.ToString();
                return PartialView(edituser);
            }
            else return View();
        }

        public ActionResult FPSDetail()
        {
            //var role = Dbcontext.masterroles.ToList();
            //ViewData["Role"] = role;
            //var cabang = Dbcontext.mastercabangs.ToList();
            //ViewData["Cabang"] = cabang;

            return PartialView();
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
                rol.NamaRole = role.NamaRole;
                return PartialView(rol);
            }
            else return View();
        }

        public ActionResult TokoEdit(int? TokoID)
        {
            mastertoko toko = Dbcontext.mastertokoes.Where(x => x.TokoID == TokoID).FirstOrDefault();
            
            if (TokoID != null)
            {
                CreateTokoViewModel tok = new CreateTokoViewModel();
                tok.TokoID = toko.TokoID;
                tok.NamaToko = toko.NamaToko;
                tok.AlamatToko = toko.AlamatToko;
                return PartialView(tok);
            }
            else return View();
        }
        public ActionResult UnitEdit(int? UnitID)
        {
            masterunit unit = Dbcontext.masterunits.Where(x => x.UnitID == UnitID).FirstOrDefault();

            if (UnitID != null)
            {
                CreateUnitViewModel uni = new CreateUnitViewModel();
                uni.UnitID = unit.UnitID;
                uni.NamaUnit = unit.NamaUnit;
                return PartialView(uni);
            }
            else return View();
        }

        public ActionResult KategoriEdit(int? KategoriID)
        {
            masterkategori kategori = Dbcontext.masterkategoris.Where(x => x.KategoriID == KategoriID).FirstOrDefault();

            if (KategoriID != null)
            {
                CreateKategoriViewModel kat = new CreateKategoriViewModel();
                kat.KategoriID = kategori.KategoriID;
                kat.NamaKategori = kategori.NamaKategori;
                kat.Deskripsi = kategori.Deskripsi;

                return PartialView(kat);
            }
            else return View();
        }

        public ActionResult SatuanEdit(int? SatuanID)
        {
            mastersatuan satuan = Dbcontext.mastersatuans.Where(x => x.SatuanID == SatuanID).FirstOrDefault();

            if (SatuanID != null)
            {
                CreateSatuanViewModel sat = new CreateSatuanViewModel();
                sat.SatuanID = satuan.SatuanID;
                sat.NamaSatuan = satuan.NamaSatuan;

                return PartialView(sat);
            }
            else return View();
        }
    }
}