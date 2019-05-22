using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Doc_Technique
{
    public class EmailValidator : IEmailValidator
    {
        public bool IsValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                new MailAddress(email);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
