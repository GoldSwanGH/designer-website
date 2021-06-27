using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using designer_website.Attributes;

namespace designer_website.Models
{
    public class UserViewModel
    {
        [DataType(DataType.Text)]
        [DisplayName("Имя")]
        public virtual string FirstName { get; set; }
        
        [DataType(DataType.Text)]
        [DisplayName("Фамилия")]
        public virtual string LastName { get; set; }
        
        [EmailAddress(ErrorMessage = "Email адрес введен неправильно.")]
        [DisplayName("Почта")]
        public virtual string Email { get; set; }
        
        [DataType(DataType.Password)]
        [DisplayName("Пароль")]
        public virtual string Password { get; set; }

        [Phone(ErrorMessage = "Номер телефона введен неправильно.")]
        [DisplayName("Телефон")]
        public virtual string Tel { get; set; }
    }
}