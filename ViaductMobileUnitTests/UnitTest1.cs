using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Xunit;

namespace ViaductMobileUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [Fact]
        public void TestMethod2()
        {
            //arrange
            string login = "a";
            string password = "a";

            //act

            //assert
            Xunit.Assert.Equal(login, password);
        }
    }
}


//[Theory]
//[InlineData("test", "a")]
//public void TestMethod1(string login, string password)
//{
//    //arrange
//    LoginPage lp = new LoginPage();

//    //act
//    bool result = lp.TryToLogin(login, password).Result;

//    //assert
//    Assert.True(result);
//}

//[Fact]
//public void TestMethod2()
//{
//    //arrange
//    string login = "a";
//    string password = "a";

//    //act

//    //assert
//    Assert.Equal(login, password);
//}