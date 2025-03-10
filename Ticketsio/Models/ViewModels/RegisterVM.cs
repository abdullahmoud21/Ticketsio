using System.ComponentModel.DataAnnotations;

namespace Ticketsio.Models.ViewModels
{
    public class RegisterVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a username")]
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Please enter an email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
