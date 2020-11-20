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
            readReport.Start = decimal.Parse(startEntry.Text);
            readReport.ReportAmount = decimal.Parse(reportEntry.Text);
            readReport.Terminal = decimal.Parse(terminalEntry.Text);
            readReport.AmountIn = decimal.Parse(isEntry.Text);
            readReport.Pizzas = int.Parse(pizzasEntry.Text);
            readReport.ShouldBe = readReport.ShouldBe + readReport.ReportAmount - readReport.Terminal;
            readReport.Difference = readReport.AmountIn - readReport.ShouldBe;
            bool result = await readReport.UpdateReport();
            List<Report> rList = new List<Report>();
            rList.Add(readReport);
            reportDataGrid.ItemsSource = rList;
        }
        
    }
}