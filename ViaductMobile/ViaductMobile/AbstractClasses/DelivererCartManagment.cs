using System;
using System.Collections.Generic;
using System.Text;
using ViaductMobile.Models;

namespace ViaductMobile.AbstractClasses
{
    abstract class DelivererCartManagment
    {
        List<string> userNicknameList = new List<string>();
        bool reload = false;
        async void ReloadData(User loggedUser)
        {
            User u = new User();
            Deliverer d = new Deliverer();
            userNicknameList = await u.ReadAllUsers();
            //SetCorrectUserPicker(loggedUser);
            //SetCorrectDatePicker();
            //SetCorrectDelivererCart();
        }
        //public List<string> SetCorrectUserPicker(User loggedUser)
        //{
        //    reload = false;
        //    if (loggedUser.Permission.Equals("Admin") || loggedUser.Permission.Equals("Manager"))
        //    {
        //        usersPicker.ItemsSource = userNicknameList;
        //        return userNicknameList;
        //    }
        //    else
        //    {
        //        usersPicker.ItemsSource.Add(loggedUser.Nickname);
        //    }
        //    if (usersPicker.SelectedItem is null)
        //    {
        //        if (changedUser is null)
        //        {
        //            foreach (var item in userNicknameList)
        //            {
        //                if (item.Equals(loggedUser.Nickname))
        //                {
        //                    usersPicker.SelectedItem = item;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            usersPicker.SelectedItem = changedUser;
        //        }
        //    }
        //    reload = true;
        //}
        //void SetCorrectDatePicker()
        //{
        //    reload = false;
        //    if (changedDate != null)
        //    {
        //        chooseDayPicker.Date = changedDate.Date;
        //    }
        //    reload = true;
        //}

        //async void SetCorrectDelivererCart()
        //{
        //    delivererCart = await Deliverer.ReadDelivererCartt(chooseDayPicker.Date, usersPicker.SelectedItem.ToString());
        //    if (delivererCart != null)
        //    {
        //        if (delivererCart.Closed == false)
        //        {
        //            delivererCartDataGrid.ItemsSource = new ViewModels.DelivererCartVM(delivererCart.Id).Supplies;
        //        }
        //        else
        //        {
        //            App.Current.MainPage = new NavigationPage(new CloseDelivererCart(loggedUser, delivererCart, chooseDayPicker.Date, usersPicker.SelectedItem.ToString()))
        //            {
        //                BarBackgroundColor = Color.FromHex(Texts.appBackgroundColor),
        //                BarTextColor = Color.White
        //            };
        //        }
        //    }
        //    else
        //    {
        //        string x = null;
        //        delivererCartDataGrid.ItemsSource = new ViewModels.DelivererCartVM(x).Supplies;
        //    }
        //    report = await Report.ReadTodayReport(chooseDayPicker.Date);
        //}
    }
}
