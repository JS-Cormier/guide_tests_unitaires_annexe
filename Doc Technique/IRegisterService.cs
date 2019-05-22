using System.Threading.Tasks;

namespace Doc_Technique
{
    public interface IRegisterService
    {
        Task RegisterUser(string email, string password);
    }
}