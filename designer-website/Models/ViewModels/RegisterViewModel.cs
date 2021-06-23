﻿using System;
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
        public new string Email { get; set; }

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