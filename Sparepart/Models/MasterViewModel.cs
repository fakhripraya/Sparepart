using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sparepart.Models
{

    public class SJCreateViewModel
    {
        public int SJID { get; set; }
        public int? FPSID { get; set; }
        public int CabangID { get; set; }
        public string JenisSJ { get; set; }
        public string Keterangan { get; set; }
        [UIHint("ClientFPS")]
        public FPSViewModel FPS
        {
            get;
            set;
        }
    }

    public class FPSDetailCreateViewModel
    {
        public string KodeBarangTipe { get; set; }
        public int Quantity { get; set; }
        public string Keterangan { get; set; }
    }

    public class DetailPermintaanBarangViewModel
    {
        public int SeqFPSID { get; set; }
        public Nullable<int> FPSID { get; set; }
        public int? JumlahHarga { get; set; }
        public int? SatuanID { get; set; }
        public string KodeBarangTipe { get; set; }
        public int? Quantity { get; set; }
        public string Keterangan { get; set; }
    }

    public class FPSViewModel
    {
        public int? FPSID { get; set; }

    }

    public class FPSDetailViewModel
    {
        public string NoTicket { get; set; }
        public string NamaPemohon { get; set; }
        public string NIKPemohon { get; set; }
        public string NamaCabang { get; set; }
        public string NamaToko { get; set; }
        public string NamaUnit { get; set; }
        public string JenisFPS { get; set; }
        public string Approval { get; set; }
    }

    public class CreateCabangViewModel
    {
        [Required]
        public int CabangID { get; set; }
        [Required]
        public string Toko { get; set; }
        [Required]
        public string Unit { get; set; }
        [Required]
        public string NamaCabang { get; set; }
    }
    public class CreateTokoViewModel
    {
        
        public int TokoID { get; set; }
        [Required]
        public string NamaToko { get; set; }
        [Required]
        public string AlamatToko { get; set; }
    }
    public class CreateUnitViewModel
    {
        [Required]
        public int UnitID { get; set; }
        [Required]
        public string NamaUnit { get; set; }
    }

    public class CreateRoleViewModel
    {
        public int RoleID { get; set; }
        public string NamaRole { get; set; }
    }

    public class CreateKategoriViewModel
    {
        [Required]
        public int KategoriID { get; set; }
        [Required]
        public string NamaKategori { get; set; }
        [Required]
        public string Deskripsi { get; set; }
    }
    public class CreateSatuanViewModel
    {
        [Required]
        public int SatuanID { get; set; }
        [Required]
        public string NamaSatuan { get; set; }
    }

    public class CreateBarangViewModel
    {
        [Required]
        public string KodeBarangTipe { get; set; }
        [Required]
        public int KategoriID { get; set; }
        [Required]
        public string BarangPLU { get; set; }
        [Required]
        public string NamaBarang { get; set; }
        [Required]
        public int SatuanID { get; set; }
        [Required]
        public int HargaSatuan { get; set; }
        [Required]
        public decimal BufferStock { get; set; }
    }

    public class TokoViewModel
    {
        public int TokoID { get; set; }
        public string NamaToko { get; set; }
    }

    public class UnitViewModel
    {
        public int UnitID { get; set; }
        public string NamaUnit { get; set; }
        public Nullable<sbyte> IsDelete { get; set; }
    }
}