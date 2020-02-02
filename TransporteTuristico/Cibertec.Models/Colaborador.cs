using System;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;

namespace Cibertec.Models
{
    public class Colaborador
    {
        [Write(false)]
        public int Id { get; set; }
        [Key]
        public int IdColaborador { get; set; }
        public string Documento { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string EmailPersonal { get; set; }
        public bool Estado { get; set; }

    }
}
