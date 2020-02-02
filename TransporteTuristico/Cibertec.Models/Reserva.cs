using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;


namespace Cibertec.Models
{
    public class Reserva
    {
        [Write(false)]
        public int Id { get; set; }
        [Key]
        public int IdReserva { get; set; }
        public string Itinerario { get; set; }
        public int? IdItinerario { get; set; }
        public int IdCliente { get; set; }
        //public string NombresApellidosCliente { get; set; }          
        public string Nombres { get; set; }
        public string Apellidos { get; set; }   
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public decimal Costo { get; set; }
    }

}
