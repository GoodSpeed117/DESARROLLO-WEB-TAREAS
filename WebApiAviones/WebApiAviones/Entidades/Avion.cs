using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiAviones.Entidades
{
    public class Avion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The information from  [0] is required")]
        [StringLength(maximumLength: 10, ErrorMessage = "The information from {0} only could have 10 characters")]
        [NotMapped]
        public string Name { get; set; }

        [Range(1, 10000, ErrorMessage = "Date only can be of four digits")]
        [Required(ErrorMessage = "The information from  [0] is required")]
        [NotMapped]
        public int Units { get; set; }

        [Range(1900, 2015, ErrorMessage = "Date only can be of four digits")]
        [Required(ErrorMessage = "The information from  [0] is required")]
        [NotMapped]
        public int LaunchYear { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                var primeraLetra = Name[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula",
                        new String[] { nameof(Name) });
                }
            }
        }

    }
}