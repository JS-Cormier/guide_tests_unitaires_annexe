using AutoFixture;
using Doc_Technique;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class EmailValidatorTest
    {
        private EmailValidator _subject;

        [TestInitialize]
        public void SetUp()
        {
            _subject = new EmailValidator();
        }

        // Référence #7
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void IsValid_WhenEmailIsNullEmptyOrWhiteSpaces_ThenReturnFalse(string email)
        {
            _subject.IsValid(email).Should().BeFalse();
        }

        // Référence #12
        [DataTestMethod]
        [DataRow("test@g mail.com")]
        [DataRow("test@gmail.co m")]
        public void IsValid_WhenContainsSpacesAfterAtSign_ThenReturnsFalse(string invalidEmail)
        {
            _subject.IsValid(invalidEmail).Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("test@gmail.com")]
        [DataRow("this is valid@tes.io")]
        public void IsValid_WhenContainsisValid_ThenReturnsTrue(string validEmail)
        {
            _subject.IsValid(validEmail).Should().BeTrue();
        }
    }
}

