using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Xunit;
using Acr.UserDialogs;
using ViaductMobile;

namespace ViaductMobileTests
{
    [TestClass]
    public class LoginPageTests
    {
        [Theory]
        [InlineData("testLogin", "testPassword")]
        [InlineData("1234testLogin", "   ")]
        [InlineData("!@#$testLogin", "testPassword+_}{:")]
        [InlineData("śżćąętestLogin", "test  Password")]
        [InlineData("/*/*-][[-775", "是不了了")]
        [InlineData("", "")]
        [InlineData(null, "")]
        [InlineData("", null)]
        [InlineData(null, null)]
        [InlineData(" ", " ")]
        public void TryToLogin_When_CredentialsAreIncorrect_Then_FalseAndNoException(string login, string password)
        {
            //arrange
            ViaductMobile.LoginPage lp = new ViaductMobile.LoginPage();

            //act
            var methodResult = lp.TryToLogin(login, password);

            //assert
            Xunit.Assert.False(methodResult.Result);
            Xunit.Assert.Null(methodResult.Exception);
        }
        [Theory]
        [InlineData("rafatus", "Demant777")]
        public void TryToLogin_When_CredentialsAreCorrect_Then_TrueAndNoException(string login, string password)
        {
            //arrange
            ViaductMobile.LoginPage lp = new ViaductMobile.LoginPage();

            //act
            var methodResult = lp.TryToLogin(login, password);

            //assert
            Xunit.Assert.True(methodResult.Result);
            Xunit.Assert.Null(methodResult.Exception);
        }
    }
}
