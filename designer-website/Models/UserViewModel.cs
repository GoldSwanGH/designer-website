using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using BC = BCrypt.Net.BCrypt;

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
        [CheckEmailAvailability]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Это обязательное поле.")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }
        // возможно, стоит уже в View шифровать пароль пользователя с помощью bcrypt, чтобы не отправлять пароль текстом
        
        [Required(ErrorMessage = "Это обязательное поле.")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        [ConfirmPassword]
        public string ConfirmPassword { get; set; }
        
        [Required(ErrorMessage = "Это обязательное поле.")]
        [Phone(ErrorMessage = "Номер телефона введен неправильно.")]
        //[RegularExpression(@"^(\s*)?(\+)?([- _():=+]?\d[- _():=+]?){10,14}(\s*)?$", ErrorMessage = "Номер телефона введен неправильно2.")]
        [DisplayName("Phone number")]
        public string Tel { get; set; }

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
        private string GetErrorMessage() => "Пароли не совпадают.";
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

    public class CheckEmailAvailabilityAttribute : ValidationAttribute
    {
        private MSDBcontext _dbcontext;
        private string GetErrorMessage() => "Пользователь с таким Email уже существует.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            _dbcontext = validationContext.GetService(typeof(MSDBcontext)) as MSDBcontext;
            var userViewModel = (UserViewModel)validationContext.ObjectInstance;
            var sameEmailUser = _dbcontext.Users.FirstOrDefault(u => u.Email == userViewModel.Email);
            if (sameEmailUser != null)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }
    }
}