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
    public class PasswordValidatorTest
    {
        private PasswordValidator _subject;

        [TestInitialize]
        public void SetUp()
        {
            _subject = new PasswordValidator();
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void IsValid_WhenPasswordIsNullEmptyOrWhiteSpaces_ThenReturnsFalse(string password)
        {
            _subject.IsValid(password).Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("abc123")]
        [DataRow("abcdefg")]
        [DataRow("12345")]
        public void IsValid_WhenContainsNoUpperCase_ThenReturnsFalse(string invalidPassword)
        {
            _subject.IsValid(invalidPassword).Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("abcDEF")]
        [DataRow("DDDDDD")]
        public void IsValid_WhenContainsNoDigit_ThenReturnsFalse(string invalidPassword)
        {
            _subject.IsValid(invalidPassword).Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("Ab1")]
        [DataRow("Abc12")]
        [DataRow("")]
        public void IsValid_WhenContainsLessThan6Characters_ThenReturnsFalse(string invalidPassword)
        {
            _subject.IsValid(invalidPassword).Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("Abcde1")]
        [DataRow("ABCDE1")]
        [DataRow("1234ABc")]
        [DataRow("♪ This is 1 valid password ♪")]
        public void IsValid_ContainsAnUpperCaseADigitAndMoreThan5Character_ThenReturnsTrue(string validPassword)
        {
            _subject.IsValid(validPassword).Should().BeTrue();
        }
    }
}

