using System.ComponentModel.DataAnnotations;

namespace MilleniumTest.Model
{
    public class Account
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Imię jest wymagane")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        public string LastName { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email")]
        public string Email { get; set; } = string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public string Location{ get; set; } = string.Empty;
    }
}