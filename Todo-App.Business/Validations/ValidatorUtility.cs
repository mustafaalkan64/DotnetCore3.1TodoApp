using FluentValidation;

namespace Todo_App.Business.Validations
{
    public static class ValidatorUtility
    {
        public static string FluentValidate(IValidator validator, object entity)
        {
            var result = validator.Validate(entity);
            if (result.Errors.Count > 0)
                return string.Join(",", result.Errors);

            return "";
        }
    }
}
