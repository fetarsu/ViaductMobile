using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Xunit;
using Acr.UserDialogs;
using ViaductMobile;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [Theory]
        [InlineData("test", "a")]
        public void TestMethod1(string login, string password)
        {
            //arrange
            LoginPage lp = new LoginPage();

            //act
            var result = lp.TryToLogin(login, password).Result;

            //assert

            Xunit.Assert.True(result);
        }
    }
}
