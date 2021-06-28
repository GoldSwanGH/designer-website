using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using designer_website.Attributes;
using designer_website.Models.EntityFrameworkModels;

namespace designer_website.Models
{
    public class UserViewModel
    {
        public int? userId { get; set; }
        [DataType(DataType.Text)]
        [DisplayName("Имя")]
        public virtual string FirstName { get; set; }
        
        [DataType(DataType.Text)]
        [DisplayName("Фамилия")]
        public virtual string LastName { get; set; }
        
        [EmailAddress(ErrorMessage = "Email адрес введен неправильно.")]
        [DisplayName("Почта")]
        public virtual string Email { get; set; }

        [Phone(ErrorMessage = "Номер телефона введен неправильно.")]
        [DisplayName("Телефон")]
        public virtual string Tel { get; set; }

        public static UserViewModel ToUserViewModel(User user)
        {
            var model = new UserViewModel
            {
                userId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Tel = user.Tel
            };
            return model;
        }
    }
}