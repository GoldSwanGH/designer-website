using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using designer_website.Attributes;

namespace designer_website.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Это обязательное поле.")]
        [DataType(DataType.Text)]
        [DisplayName("First name")]
        public string FirstName { get; set; }
        
        [DataType(DataType.Text)]
        [DisplayName("Last name")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Это обязательное поле.")]
        [EmailAddress(ErrorMessage = "Email адрес введен неправильно.")]
        [DisplayName("Email address")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Это обязательное поле.")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Это обязательное поле.")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        [ConfirmPassword]
        public string ConfirmPassword { get; set; }
    }
}