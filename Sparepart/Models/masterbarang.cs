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
    
    public partial class masterbarang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public masterbarang()
        {
            this.fpsdetails = new HashSet<fpsdetail>();
        }
    
        public string KodeBarangTipe { get; set; }
        public int KategoriID { get; set; }
        public string BarangPLU { get; set; }
        public string NamaBarang { get; set; }
        public int SatuanID { get; set; }
        public int HargaSatuan { get; set; }
        public decimal BufferStock { get; set; }
        public string UserInput { get; set; }
        public System.DateTime TanggalInput { get; set; }
        public string UserUpdate { get; set; }
        public Nullable<System.DateTime> TanggalUpdate { get; set; }
        public sbyte IsDelete { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fpsdetail> fpsdetails { get; set; }
        public virtual masterkategori masterkategori { get; set; }
        public virtual mastersatuan mastersatuan { get; set; }
    }
}
