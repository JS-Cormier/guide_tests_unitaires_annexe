using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc_Technique
{
    public interface IEmailValidator
    {
        bool IsValid(string email);
    }
}
