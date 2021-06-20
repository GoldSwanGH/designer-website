using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace designer_website.Models
{
    public class UserViewModel // тестовая версия модели для валидации
    {
        [Required]
        [DataType(DataType.Text)]
        [DisplayName("First name")]
        public string FirstName { get; set; }
        
        [DataType(DataType.Text)]
        [DisplayName("Last name")]
        public string LastName { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email address")]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }
        // возможно, стоит уже в View шифровать пароль пользователя с помощью bcrypt, чтобы не отправлять пароль текстом
        
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        [SameAs("Password", ErrorMessage = "It should be similar to Password")]
        public string ConfirmPassword { get; set; }
        
        [Required]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone number")]
        public int? Tel { get; set; }
    }

    public class SameAsAttribute : ValidationAttribute
    {
        public string Property { get; set; }
        public SameAsAttribute(string Property)
        {
            this.Property = Property;
        }
        public override bool IsValid(object value)
        {
            //Any additional validation logic specific to the property can go here.
            return true;
        }   
    }
}