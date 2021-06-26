using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using designer_website.Attributes;
using designer_website.Models.EntityFrameworkModels;

namespace designer_website.Models
{
    public class PasswordChangeViewModel : UserViewModel
    {
        [Required(ErrorMessage = "Это обязательное поле.")]
        [DataType(DataType.Password)]
        [DisplayName("Пароль")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Длина пароля должна быть от 8 до 50 символов")]
        public override string Password { get; set; }
        
        [Required(ErrorMessage = "Это обязательное поле.")]
        [DataType(DataType.Password)]
        [DisplayName("Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public override string ConfirmPassword { get; set; }
        
        
    }
}