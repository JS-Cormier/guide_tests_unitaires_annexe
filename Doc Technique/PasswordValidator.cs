using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc_Technique
{
    public class PasswordValidator : IPasswordValidator
    {
        public bool IsValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            return (password.Any(char.IsUpper) && password.Any(char.IsDigit) && password.Length > 5);
        }
    }
}
