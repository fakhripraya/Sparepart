﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbsparepartEntities : DbContext
    {
        public dbsparepartEntities()
            : base("name=dbsparepartEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<aks> akses { get; set; }
        public virtual DbSet<fpsdetail> fpsdetails { get; set; }
        public virtual DbSet<fpsheader> fpsheaders { get; set; }
        public virtual DbSet<hakaks> hakakses { get; set; }
        public virtual DbSet<masterbarang> masterbarangs { get; set; }
        public virtual DbSet<mastercabang> mastercabangs { get; set; }
        public virtual DbSet<masterkategori> masterkategoris { get; set; }
        public virtual DbSet<masterrole> masterroles { get; set; }
        public virtual DbSet<mastersatuan> mastersatuans { get; set; }
        public virtual DbSet<mastertoko> mastertokoes { get; set; }
        public virtual DbSet<masterunit> masterunits { get; set; }
        public virtual DbSet<masteruser> masterusers { get; set; }
    }
}
