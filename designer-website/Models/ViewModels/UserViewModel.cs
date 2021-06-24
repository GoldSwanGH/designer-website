using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using designer_website.Attributes;

namespace designer_website.Models
{
    public class UserViewModel
    {
        [DataType(DataType.Text)]
        [DisplayName("First name")]
        public virtual string FirstName { get; set; }
        
        [DataType(DataType.Text)]
        [DisplayName("Last name")]
        public virtual string LastName { get; set; }
        
        [EmailAddress(ErrorMessage = "Email адрес введен неправильно.")]
        [DisplayName("Email address")]
        public virtual string Email { get; set; }
        
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public virtual string Password { get; set; }
        
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        [ConfirmPassword]
        public virtual string ConfirmPassword { get; set; }
        
        [Phone(ErrorMessage = "Номер телефона введен неправильно.")]
        [DisplayName("Phone number")]
        public virtual string Tel { get; set; }
    }
}