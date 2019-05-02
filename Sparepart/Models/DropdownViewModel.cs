using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sparepart.Models
{
    public class RoleViewModel
    {
        public int RoleID
        {
            get;
            set;
        }

        public string NamaRole
        {
            get;
            set;
        }
    }

    public class CabangViewModel
    {
        public int CabangID
        {
            get;
            set;
        }

        public string NamaCabang
        {
            get;
            set;
        }
    }
    public class KategoriViewModel
    {
        public int KategoriID
        {
            get;
            set;
        }

        public string NamaKategori
        {
            get;
            set;
        }
    }
    public class SatuanViewModel
    {
        public int SatuanID
        {
            get;
            set;
        }

        public string NamaSatuan
        {
            get;
            set;
        }
    }
}