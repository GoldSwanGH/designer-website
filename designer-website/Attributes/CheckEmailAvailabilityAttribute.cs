using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using designer_website.Models;

namespace designer_website.Attributes
{
    public class CheckEmailAvailabilityAttribute : ValidationAttribute
    {
        private MSDBcontext _dbcontext;

        private bool ExistenceExpected;

        public CheckEmailAvailabilityAttribute(bool existenceExpected)
        {
            this.ExistenceExpected = existenceExpected;
        }

        private string GetErrorMessage()
        {
            string errorMessage = "Пользователь с таким Email уже существует.";

            if (ExistenceExpected)
            {
                errorMessage = "Пользователя с таким Email не существует.";
            }

            return errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            _dbcontext = validationContext.GetService(typeof(MSDBcontext)) as MSDBcontext;
            
            var userViewModel = (UserViewModel)validationContext.ObjectInstance;
            var sameEmailUser = _dbcontext.Users.FirstOrDefault(u => u.Email == userViewModel.Email);
            
            if (!ExistenceExpected && sameEmailUser != null)
            {
                return new ValidationResult(GetErrorMessage());
            }
            if (ExistenceExpected && sameEmailUser == null)
            {
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }
    }
}