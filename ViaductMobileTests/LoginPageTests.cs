using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Xunit;
using ViaductMobile;
using Assert = Xunit.Assert;

namespace ViaductMobileTests
{
    [TestClass]
    public class LoginPageTests
    {
        [Theory]
        [InlineData("test", "a")]
        public void TestMethod1(string login, string password)
        {
            //arrange
            LoginPage lp = new LoginPage();

            //act
            bool result = lp.TryToLogin(login, password).Result;

            //assert
            Assert.True(result);
        }

        [Fact]
        public void TestMethod2()
        {
            //arrange
            string login = "a";
            string password = "a";

            //act

            //assert
            Assert.Equal(login, password);
        }
    }
}
