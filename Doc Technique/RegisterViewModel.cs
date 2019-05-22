using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Doc_Technique
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private IRegisterService _registerService;
        private IEmailValidator _emailValidator;
        private IPasswordValidator _passwordValidator;
        IFileSystem _fileSystem;
        private string _email;
        private string _password;

        public const string HiddenLogFilePath = "C:/logs/doc_technique.log";

        public event PropertyChangedEventHandler PropertyChanged;

        public string Email {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string Password {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public RegisterViewModel(IRegisterService registerService, IEmailValidator emailValidator, IPasswordValidator passwordValidator, IFileSystem fileSystem)
        {
            _registerService = registerService;
            _emailValidator = emailValidator;
            _passwordValidator = passwordValidator;
            _fileSystem = fileSystem;
        }

        public void Register()
        {
            if(_emailValidator.IsValid(Email) && _passwordValidator.IsValid(Password))
            { 
                try
                {
                    _registerService.RegisterUser(Email, Password);
                }
                catch(Exception e)
                {
                    LogError(e);
                }
            }
        }

        private void LogError(Exception e)
        {
            if(_fileSystem.File.Exists(HiddenLogFilePath))
                _fileSystem.File.SetAttributes(HiddenLogFilePath, FileAttributes.Normal);

            _fileSystem.File.WriteAllText(HiddenLogFilePath, e.Message);
            _fileSystem.File.SetAttributes(HiddenLogFilePath, FileAttributes.Hidden);
        }

        public void ResetFields()
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
