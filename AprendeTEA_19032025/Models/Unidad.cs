using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AprendeTEA_19032025.Models
{
    public class Unidad
    {
        [Key]
        public int IdUnidad { get; set; }
        public int? IdPlanTrabajo { get; set; }

        public string Objetivo { get; set; }
        public string NombreUnidad { get; set; }
        public string Detalle { get; set; }
        public string Responsable { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool Estatus { get; set; }

        [NotMapped]
        public List<object> Unidades { get; set; }
        
        [NotMapped]
        public Unidad UnidadEdicion { get; set; }

        // NUEVOS CAMPOS DE ACTIVIDADES
        public bool TieneSopaLetras { get; set; }
        public string PalabrasSopa { get; set; }

        public bool TieneCrucigrama { get; set; }
        public string PreguntasCrucigrama { get; set; }

        public bool TieneRelacionar { get; set; }
        public string RelacionarColumnas { get; set; }

        public bool TieneAgrupacion { get; set; }
        public string Agrupacion { get; set; }

        public bool TieneOrdenar { get; set; }
        public string OrdenarPasos { get; set; }



    }

}
