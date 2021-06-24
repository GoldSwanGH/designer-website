using System.ComponentModel.DataAnnotations;
using designer_website.Models;

namespace designer_website.Attributes
{
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
        Следующий кусок кода должен был реализовывать IClientModelValidator для работы валидации Confirm password на
        стороне клиента, но, судя по всему, там нужно еще и с JS поработать на стороне клиента, поэтому пока реализация
        отложена.
        
        public void AddValidation(ClientModelValidationContext context)
        {
            throw new NotImplementedException();
        }
        
        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }

            attributes.Add(key, value);
            return true;
        } */
    }
}