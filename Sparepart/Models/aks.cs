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
    
    public partial class aks
    {
        public int ID { get; set; }
        public int AksesID { get; set; }
        public int RoleID { get; set; }
        public string NamaAkses { get; set; }
    
        public virtual hakaks hakaks { get; set; }
        public virtual masterrole masterrole { get; set; }
    }
}
