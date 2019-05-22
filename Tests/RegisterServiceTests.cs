using AutoFixture;
using Doc_Technique;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class RegisterServiceTests
    {
        private RegisterService _sujet;
        private Fixture _auto = new Fixture();

        [TestInitialize]
        public void SetUp()
        {
            _sujet = new RegisterService();
        }

        // Référence #9
        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void RegisterUser_WhenEmailIsNullOrEmpty_ShouldThrowException(string email)
        {
            Func<Task> act = () => _sujet.RegisterUser(email, "password");

            act.Should().Throw<ArgumentException>();
        }
    }
}
