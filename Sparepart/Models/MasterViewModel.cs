﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sparepart.Models
{
    public class CreateCabangViewModel
    {
        public int CabangID { get; set; }
        public string Toko { get; set; }
        public string Unit { get; set; }
        public string NamaCabang { get; set; }
    }

    public class CreateBarangViewModel
    {
        public string KodeBarangTipe { get; set; }
        public int KategoriID { get; set; }
        public string BarangPLU { get; set; }
        public string NamaBarang { get; set; }
        public int SatuanID { get; set; }
        public int HargaSatuan { get; set; }
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