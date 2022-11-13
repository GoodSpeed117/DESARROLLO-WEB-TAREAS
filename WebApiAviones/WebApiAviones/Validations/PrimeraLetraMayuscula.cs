using System.ComponentModel.DataAnnotations;

namespace WebApiAviones.Validations
{
    public class PrimeraLetraMayuscula
    {
        public class PrimeraLetraMayusculaAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return ValidationResult.Success;
                }

                var primeraLetra = value.ToString()[0].ToString();

                if (primeraLetra != primeraLetra.ToUpper())
                {
                    return new ValidationResult("La primera letra del nombre del avion debe de ser mayúscula");

                }
                return ValidationResult.Success;
            }
        }
    }
}
