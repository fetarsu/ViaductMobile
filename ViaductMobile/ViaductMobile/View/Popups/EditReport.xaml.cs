using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ViaductMobile.Models;
using ViaductMobile.Algorithms;

namespace ViaductMobile.View.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditReport : PopupPage
    {
        Report readReport;
        bool add;
        string notification;
        decimal shouldBe, reportAmount, terminal;
        Xamarin.Forms.DataGrid.DataGrid reportDataGrid;
        public EditReport(Report readReportt, Xamarin.Forms.DataGrid.DataGrid repordGrid)
        {
            InitializeComponent();
            this.reportDataGrid = repordGrid;
            this.readReport = readReportt;
            startEntry.Text = readReport.Start.ToString();
            reportEntry.Text = readReport.ReportAmount.ToString();
            terminalEntry.Text = readReport.Terminal.ToString();
            isEntry.Text = readReport.AmountIn.ToString();
            pizzasEntry.Text = readReport.Pizzas.ToString();
            empPizzasEntry.Text = readReport.EmployeePizzas.ToString();
            shouldBe = readReport.ShouldBe;
            reportAmount = readReport.ReportAmount;
            terminal = readReport.Terminal;
        }

        [Obsolete]
        private async void Back_Clicked(object sender, EventArgs e)
        {
            BindingContext = new ViewModels.EmployeeTableVM(readReport);
            await PopupNavigation.PopAsync(true);
        }
        [Obsolete]
        private async void Add_Clicked(object sender, EventArgs e)
        {
            add = true;
            notification = "";
            try { readReport.Start = decimal.Parse(startEntry.Text); }
            catch
            {
                if (startEntry.Text == null || startEntry.Text == "")
                    readReport.Start = 0;
                else { add = false; notification += " start"; }
            }
            try { readReport.ReportAmount = decimal.Parse(reportEntry.Text); }
            catch
            {
                if (reportEntry.Text == null || reportEntry.Text == "")
                    readReport.ReportAmount = 0;
                else { add = false; notification += " raport"; }
            }

            try { readReport.Terminal = decimal.Parse(terminalEntry.Text); }
            catch
            {
                if (terminalEntry.Text == null || terminalEntry.Text == "")
                    readReport.Terminal = 0;
                else { add = false; notification += " terminal"; }
            }
            try { readReport.AmountIn = decimal.Parse(isEntry.Text); }
            catch
            {
                if (isEntry.Text == null || isEntry.Text == "")
                    readReport.AmountIn = 0;
                else { add = false; notification += " jest"; }
            }
            try { readReport.Pizzas = int.Parse(pizzasEntry.Text); }
            catch
            {
                if (pizzasEntry.Text == null || pizzasEntry.Text == "")
                    readReport.Pizzas = 0;
                else { add = false; notification += " pizze"; }
            }
            try { readReport.EmployeePizzas = int.Parse(empPizzasEntry.Text); }
            catch
            {
                if (empPizzasEntry.Text == null || empPizzasEntry.Text == "")
                    readReport.EmployeePizzas = 0;
                else { add = false; notification += " pizze pracownicze"; }
            }
            if (add == false)
            {
                await DisplayAlert("Uwaga", "Pole"+notification+" zostało źle wypełnione", "OK");
            }
            else
            {
                readReport.ShouldBe = shouldBe + (readReport.ReportAmount - reportAmount) - (readReport.Terminal - terminal);
                readReport.Difference = readReport.AmountIn - readReport.ShouldBe;
                bool result = await readReport.UpdateReport();
                List<Report> rList = new List<Report>();
                rList.Add(readReport);
                reportDataGrid.ItemsSource = rList;
            }
        }
        
    }
}