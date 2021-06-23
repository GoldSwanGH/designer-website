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
}