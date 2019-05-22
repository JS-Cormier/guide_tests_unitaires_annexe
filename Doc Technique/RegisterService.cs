using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Doc_Technique
{
    public class RegisterService : IRegisterService
    {
        private const string ApiBaseRoute = "http://not-an-actual-api.com/api";
        private const string ApiEmailCheckRoute = "/emailCheck";
        private const string ApiRegisterRoute = "/register";

        private HttpClient _httpClient;

        public RegisterService()
        {
            _httpClient = new HttpClient();
        }

        public async Task RegisterUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Email or password is null or empty");

            if (CheckIfEmailAlreadyTaken(email).Result)
                throw new Exception($"{email} is already taken");

            var newUser = new NewUser(email, password);

            var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
            try
            {
                await _httpClient.PostAsync($"{ApiBaseRoute}{ApiEmailCheckRoute}/{email}", content);
            }
            catch
            {
                throw;
            }
        }

        private async Task<bool> CheckIfEmailAlreadyTaken(string email)
        {
            HttpResponseMessage response = null;

            try
            {
                response = await _httpClient.GetAsync($"{ApiBaseRoute}{ApiEmailCheckRoute}/{email}");
            }
            catch
            {
                throw;
            }

            bool alreadyTaken;
            Boolean.TryParse(response.Content.ReadAsStringAsync().Result, out alreadyTaken);

            return alreadyTaken;
        }

        private class NewUser
        {
            private string _email;
            private string _password;

            public NewUser(string email, string password)
            {
                _email = email;
                _password = password;
            }
        }
    }
}
