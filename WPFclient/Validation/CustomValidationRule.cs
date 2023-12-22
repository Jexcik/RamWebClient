using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace WPFclient.Validation
{
    public class CustomValidationRule : ValidationRule
    {
        public string FieldName { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string fieldValue = value as string;

            //Реализация логики проверки каждого поля
            switch (FieldName)
            {
                case "Login":
                    if (string.IsNullOrWhiteSpace(fieldValue))
                    {
                        return new ValidationResult(false, "Поле не может быть пустым!");
                    }
                    if (!IsValidEmail(fieldValue))
                    {
                        return new ValidationResult(false, "Введите коректный E-mail");
                    }
                    break;

                case "Password":
                    if (string.IsNullOrWhiteSpace(fieldValue))
                    {
                        return new ValidationResult(false, "Поле не может быть пустым!");
                    }
                    if (!IsValidPassword(fieldValue))
                    {
                        return new ValidationResult(false, "Пароль должен содержать от 8 до 20 символов");
                    };
                    break;
                case "UserName":
                    if (string.IsNullOrWhiteSpace(fieldValue))
                    {
                        return new ValidationResult(false, "Поле не может быть пустым!");
                    }
                    break;
            }
            return ValidationResult.ValidResult;
        }
        private bool IsValidEmail(string email)
        {
            string patern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            return Regex.IsMatch(email, patern);
        }
        private bool IsValidPassword(string password)
        {
            return password.Length > 7 && password.Length < 21;
        }
    }
}
