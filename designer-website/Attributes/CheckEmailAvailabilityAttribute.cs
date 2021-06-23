using System.ComponentModel.DataAnnotations;
using System.Linq;
using designer_website.Models;

namespace designer_website.Attributes
{
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