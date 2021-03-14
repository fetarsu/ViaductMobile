using System;
using Xunit;

namespace ViaductMobile.Tests
{
    public class Class1
    {
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
