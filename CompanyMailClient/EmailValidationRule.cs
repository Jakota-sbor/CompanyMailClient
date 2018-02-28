using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CompanyMailClient
{
    public class EmailValidationRule : ValidationRule
    {
        Regex validateEmailRegex = new Regex(@"^[\w-_\.]+@(?>[\w-_]+)+(?>\.[\w-_]{2,4}){1,3}$", RegexOptions.Compiled);

        public EmailValidationRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (validateEmailRegex.IsMatch((string)value))
                    return new ValidationResult(true, null);
                else
                    return new ValidationResult(false, "Email введен неверно");
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Ошибка валидации: " + e.Message);
            }
        }
    }
}
