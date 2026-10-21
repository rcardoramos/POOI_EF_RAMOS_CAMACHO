using System.ComponentModel.DataAnnotations;

namespace POOI_EF_RAMOS_CAMACHO.Models
{
    public class Consumidor
    {
        [Display(Name = "DNI")]
        public string dnicont { get; set; }

        [Display(Name = "Nombres")]
        public string nomcont { get; set; }

        [Display(Name = "Apellidos")]
        public string apecont { get; set; }

        [Display(Name = "Correo Electrónico")]
        public string emailcont { get; set; }

        [Display(Name = "País")]
        public string idpais { get; set; }

        [Display(Name = "Nombre del País")]
        public string nombrepais { get; set; }
    }
}