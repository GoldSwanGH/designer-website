using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using BC = BCrypt.Net.BCrypt;

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
        [ConfirmPassword]
        public string ConfirmPassword { get; set; }
        
        [Required]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("Phone number")]
        public int? Tel { get; set; }

        public User ToUser()
        {
            User user = new User();
            user.Email = this.Email;
            user.Password = BC.HashPassword(this.Password);
            user.Tel = this.Tel;
            user.FirstName = this.FirstName;
            user.LastName = this.LastName;
            return user;
        }
    }

    public class ConfirmPasswordAttribute : ValidationAttribute//, IClientModelValidator
    {
        private string GetErrorMessage() => "Пароли не совпадают";
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var userViewModel = (UserViewModel)validationContext.ObjectInstance;
            if (userViewModel.Password != userViewModel.ConfirmPassword)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }
        /*
        public void AddValidation(ClientModelValidationContext context)
        {
            throw new NotImplementedException();
        }
        */
    }
}