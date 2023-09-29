using System.ComponentModel.DataAnnotations;

namespace HelmoBilite.CustomValidation;

public class BirthDateValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        DateTime dateTime = Convert.ToDateTime(value);
        return DateTime.Now.Year-dateTime.Year <= 100;
    }
}