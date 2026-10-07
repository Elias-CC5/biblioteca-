using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models
{
    public class PrestamoReporte
    {
        public string SocioNombre { get; set; } = string.Empty;
        public string LibroTitulo { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime FechaPrestamo { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaLimite { get; set; }

        public string Estado { get; set; } = string.Empty;
    }
}