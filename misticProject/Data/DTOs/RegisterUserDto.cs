using System.ComponentModel.DataAnnotations;

namespace misticProject.Data.DTOs
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Imię nie może być dłuższe niż 20 znaków.")]
        [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]*$", ErrorMessage = "Imię musi zaczynać się wielką literą.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Nazwisko nie może być dłuższe niż 20 znaków.")]
        [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłńóśźż]*$", ErrorMessage = "Nazwisko musi zaczynać się wielką literą.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy adres email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Hasło musi mieć przynajmniej 8 znaków.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Hasło musi zawierać przynajmniej jedną wielką literę, jedną cyfrę i jeden znak specjalny.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data urodzenia jest wymagana.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(RegisterUserDto), nameof(ValidateAge))]
        public DateTime DateOfBirth { get; set; }

        public static ValidationResult? ValidateAge(DateTime dateOfBirth, ValidationContext context)
        {
            int age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;

            return age >= 18
                ? ValidationResult.Success
                : new ValidationResult("Użytkownik musi mieć przynajmniej 18 lat.");
        }
    }
}
