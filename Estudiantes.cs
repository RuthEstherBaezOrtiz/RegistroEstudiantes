using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegistroEstudiantes.Models
{
    public class Estudiantes
    {
        public int IdEstudiantes { get; set; }
        [StringLength (30,MinimumLength = 3, ErrorMessage ="minimo 3 caracteres y 30 caracteres maximo")]
        public string Nombres { get; set; }
        [StringLength(30, MinimumLength = 3, ErrorMessage = "minimo 3 caracteres y 30 caracteres maximo")]
        public string Apellidos { get; set; }
        public string Fecha { get; set; }
        public string Curso { get; set; }
        public bool Estado { get; set; }
    }
}