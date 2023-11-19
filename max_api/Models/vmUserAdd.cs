using System;
using System.ComponentModel.DataAnnotations;

namespace max_api.Models
{
    public class vmUserAdd
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateTime Fecha { get; set; }
        public int CiudadEstadoId { get; set; }
    }
}
