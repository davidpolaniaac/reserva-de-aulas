﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReservaDeAulas.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ReservaAulaEntities : DbContext
    {
        public ReservaAulaEntities()
            : base("name=ReservaAulaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Aula> Aula { get; set; }
        public virtual DbSet<Disponible> Disponible { get; set; }
        public virtual DbSet<Reserva> Reserva { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
    }
}
