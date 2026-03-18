using FluentValidation;
using PharmaStock.Core.DTO.Register;

namespace PharmaStock.Core.Validators
{
    public class UserRegistrationValidator : AbstractValidator<UserRegistrationDTO>
    {
        public UserRegistrationValidator()
        {
            
        }
    }
}