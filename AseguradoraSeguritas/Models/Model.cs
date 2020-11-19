using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AseguradoraSeguritas.Models
{

    public class MyDBContex : DbContext
    {
        public MyDBContex(DbContextOptions<MyDBContex> options) : base(options)
        {
        }
    
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Plan> Plan { get; set; }
        public DbSet<Cobertura> Cobertura { get; set; }

    }

    public class Cliente
    {
        public int Id { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required (ErrorMessage ="Nombre del Cliente Requerido")]
        public string Nombre { get; set; }

       
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Fecha de Modificacion Requerida")]
        public DateTime FechaModificacion { get; set; }

        public ICollection<Plan> Planes { get; set; }
    }

    public class Plan
    {
      
        public int Id { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required(ErrorMessage = "Nombre De La Descripcion Del Plan Es Requerido")]
        public string Descripcion { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Fecha de Modificacion Requerida")]
        public DateTime FechaModificacion { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public ICollection<Cobertura> Coberturas { get; set; }
    }

    public class Cobertura
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required(ErrorMessage = "Nombre De la Descripcion De La Cobertura Es Requerido")]
        public string Descripcion { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Fecha de Modificacion Requerida")]
        public DateTime FechaModificacion { get; set; }

        public int PlanId { get; set; }
        public Plan Plan { get; set; }
    }
}


