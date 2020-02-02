using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cibertec.Mvc.Models
{
    public class ReservaModel
    {
        public int IdReserva { get; set; }  
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }     
        public int? IdItinerario { get; set; }
        public List<SelectListItem> Itinerario { set; get; }
        public int IdCliente { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_Inicio { get; set; } = DateTime.Now;
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha_Fin { get; set; } = DateTime.Now;
        public decimal Costo { get; set; }
        
    }
}