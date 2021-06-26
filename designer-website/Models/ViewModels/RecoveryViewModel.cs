using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using designer_website.Attributes;
using designer_website.Models.EntityFrameworkModels;

namespace designer_website.Models
{
    public class RecoveryViewModel : UserViewModel
    {
        [Required(ErrorMessage = "Это обязательное поле.")]
        [EmailAddress(ErrorMessage = "Email адрес введен некорректно.")]
        [DisplayName("Email адрес")]
        public override string Email { get; set; }
    }
}