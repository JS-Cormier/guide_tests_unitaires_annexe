using AutoFixture;
using Doc_Technique;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.IO;
using System.IO.Abstractions;

namespace Tests
{
    [TestClass]
    public class RegisterViewModelTests
    {
        private RegisterViewModel _subject;
        private IRegisterService _registerService;
        private IEmailValidator _emailValidator;
        private IPasswordValidator _passwordValidator;
        private IFileSystem _fileSystem;

        private Fixture _auto;

        [TestInitialize]
        public void SetUp()
        {
            _registerService = Substitute.For<IRegisterService>();
            _emailValidator = Substitute.For<IEmailValidator>();
            _passwordValidator = Substitute.For<IPasswordValidator>();
            _fileSystem = Substitute.For<IFileSystem>();

            _subject = new RegisterViewModel(_registerService, _emailValidator, _passwordValidator, _fileSystem);

            _auto = new Fixture();
        }


        // Référence #1, Référence #2, Référence #4, Référence #11
        [TestMethod]
        public void Register_WhenEmailAndPasswordAreValid_ThenCallsRegisterService()
        {
            var email = "unEmail@test.com";
            var password = _auto.Create<string>();
            _subject.Email = email;
            _subject.Password = password;
            _emailValidator.IsValid(email).Returns(true);
            _passwordValidator.IsValid(password).Returns(true);

            _subject.Register();

            _registerService.Received(1).RegisterUser(email, password);
        }

        // Référence #3, Référence #5
        [TestMethod]
        public void Register_RegisterServiceThrowAnException_ThenUnhideFileWriteMessageThenHideItAgain()
        {
            const string exceptionMessage = " is already taken.";
            _registerService.When(x => x.RegisterUser(Arg.Any<string>(), Arg.Any<string>()))
                            .Do(act => throw new Exception($"{act.ArgAt<string>(0)}{exceptionMessage}"));
            _fileSystem.File.Exists(RegisterViewModel.HiddenLogFilePath).Returns(true);
            var email = _auto.Create<string>();
            var password = _auto.Create<string>();
            _subject.Email = email;
            _subject.Password = password;
            _emailValidator.IsValid(Arg.Any<string>()).Returns(true);
            _passwordValidator.IsValid(password).Returns(true);

            _subject.Register();

            Received.InOrder(() =>
            {
                _fileSystem.File.SetAttributes(RegisterViewModel.HiddenLogFilePath, FileAttributes.Normal);
                _fileSystem.File.WriteAllText(RegisterViewModel.HiddenLogFilePath, $"{email}{exceptionMessage}");
                _fileSystem.File.SetAttributes(RegisterViewModel.HiddenLogFilePath, FileAttributes.Hidden);
            });
        }

        // Référence #8
        [TestMethod]
        public void Email_WhenSetToANewValue_ThenRaisesPropertyChanged()
        {
            var monitor = _subject.Monitor();

            _subject.Email = "aNewEmail@gmail.com";

            monitor.Should().RaisePropertyChangeFor(x => x.Email);
        }

        [TestMethod]
        public void Password_WhenSetToANewValue_ThenRaisesPropertyChanged()
        {
            var monitor = _subject.Monitor();

            _subject.Password = "abc123";

            monitor.Should().RaisePropertyChangeFor(x => x.Password);
        }

        // Référence #6, Référence #10
        [TestMethod]
        public void ResetFields_WhenCalled_ShouldSetEmailAndPasswordToEmptyString()
        {
            _subject.Email = _auto.Create<string>();
            _subject.Password = _auto.Create<string>();

            _subject.ResetFields();

            using(new AssertionScope())
            {
                _subject.Email.Should().Be(string.Empty);
                _subject.Password.Should().Be(string.Empty);
            }
        }
    }
}
