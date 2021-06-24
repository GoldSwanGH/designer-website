using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using designer_website.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using BC = BCrypt.Net.BCrypt;

namespace designer_website.Models
{
    public class RegisterViewModel : UserViewModel
    {
        [Required(ErrorMessage = "Это обязательное поле.")]
        [EmailAddress(ErrorMessage = "Email адрес введен неправильно.")]
        [DisplayName("Email address")]
        [CheckEmailAvailability(false)]
        public override string Email { get; set; }
        
        [Required(ErrorMessage = "Это обязательное поле.")]
        [DataType(DataType.Text)]
        [DisplayName("First name")]
        public override string FirstName { get; set; }
        
        [Required(ErrorMessage = "Это обязательное поле.")]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Длина пароля должна быть от 6 до 50 символов")]
        public override string Password { get; set; }
        
        [Required(ErrorMessage = "Это обязательное поле.")]
        [DataType(DataType.Password)]
        [DisplayName("Confirm password")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public override string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Это обязательное поле.")]
        [Phone(ErrorMessage = "Номер телефона введен неправильно.")]
        [DisplayName("Phone number")]
        public override string Tel { get; set; }

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
}