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
    
    public partial class mastercabang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public mastercabang()
        {
            this.fpsheaders = new HashSet<fpsheader>();
        }
    
        public int CabangID { get; set; }
        public int TokoID { get; set; }
        public int UnitID { get; set; }
        public string NamaCabang { get; set; }
        public string UserInput { get; set; }
        public System.DateTime TanggalInput { get; set; }
        public string UserUpdate { get; set; }
        public Nullable<System.DateTime> TanggalUpdate { get; set; }
        public sbyte IsDelete { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fpsheader> fpsheaders { get; set; }
        public virtual mastertoko mastertoko { get; set; }
        public virtual masterunit masterunit { get; set; }
    }
}
