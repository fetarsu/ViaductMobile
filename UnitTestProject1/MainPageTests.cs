using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Xunit;
using Acr.UserDialogs;
using ViaductMobile;
using System.Collections.Generic;

namespace ViaductMobileTests
{
    [TestClass]
    public class MainPageTests
    {
        [Theory]
        [InlineData(6, "Admin", 0)]
        [InlineData(6, "Admin", 10)]
        [InlineData(5, "Pracownik", 10)]
        [InlineData(2, "Pracownik", 0)]
        public void AddToolbarItemsDependingPermissions_When_UserHasSpecificPermissions_Then_ToolbarItemsDisplayedCorrectly(int correctToolBarItems, string permission, int deliverRate)
        {
            //arrange
            List<string> nameList = new List<string>();
            List<Xamarin.Forms.ImageSource> iconList = new List<Xamarin.Forms.ImageSource>();
            User user = new User() { Permission = permission, DeliverRate = deliverRate, Nickname = "test" };
            ViaductMobile.MainPage mp = new ViaductMobile.MainPage(user);

            //act
            foreach (var item in mp.ToolbarItems)
            {
                nameList.Add(item.Text);
                iconList.Add(item.IconImageSource);
            }

            //asset
            Xunit.Assert.Equal(mp.ToolbarItems.Count, correctToolBarItems);
            Xunit.Assert.Equal(nameList.Count, correctToolBarItems);
            Xunit.Assert.Equal(iconList.Count, correctToolBarItems);
        }

        [Theory]
        [InlineData(5, "Admin", 0)]
        [InlineData(5, "Admin", 10)]
        [InlineData(7, "Admin", 0)]
        [InlineData(7, "Admin", 10)]
        [InlineData(3, "Pracownik", 10)]
        [InlineData(3, "Pracownik", 0)]
        [InlineData(1, "Pracownik", 10)]
        [InlineData(1, "Pracownik", 0)]
        public void AddToolbarItemsDependingPermissions_When_UserHasSpecificPermissions_Then_ToolbarItemsDisplayedIncorrectly(int correctToolBarItems, string permission, int deliverRate)
        {
            //arrange
            List<string> nameList = new List<string>();
            List<Xamarin.Forms.ImageSource> iconList = new List<Xamarin.Forms.ImageSource>();
            User user = new User() { Permission = permission, DeliverRate = deliverRate, Nickname = "test" };
            ViaductMobile.MainPage mp = new ViaductMobile.MainPage(user);

            //act
            foreach (var item in mp.ToolbarItems)
            {
                nameList.Add(item.Text);
                iconList.Add(item.IconImageSource);
            }

            //asset
            Xunit.Assert.NotEqual(mp.ToolbarItems.Count, correctToolBarItems);
            Xunit.Assert.NotEqual(nameList.Count, correctToolBarItems);
            Xunit.Assert.NotEqual(iconList.Count, correctToolBarItems);
        }

    }
}
