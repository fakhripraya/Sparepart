//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sparepart.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class fpsdetail
    {
        public int SeqFPSID { get; set; }
        public int FPSID { get; set; }
        public string KodeBarangTipe { get; set; }
        public Nullable<int> Qty { get; set; }
        public string Keterangan { get; set; }
    
        public virtual fpsheader fpsheader { get; set; }
        public virtual masterbarang masterbarang { get; set; }
    }
}
