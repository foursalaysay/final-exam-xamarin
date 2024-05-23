using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App4
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Add : ContentPage
    {

        public int AutoIncrementedValue { get; set; }
        public string EMPNAME { get; set; }
        public double HOURSWORK { get; set; }
        public double GROSSINCOME { get; set; }
        public double BONUS { get; set; }
        public double DEDUCTION { get; set; }
        public double NETINCOME { get; set; }
        
        public Add(int autoValue)
        {
            InitializeComponent();
            AutoIncrementedValue = autoValue;
            BindingContext = this; // Make sure to set the BindingContext after initializing the properties
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //display all Item
            await showData();
        }



        public async Task showData()
        {

            var getData = await App.SQLitedb.ReadItemAsync(AutoIncrementedValue);

            if (getData != null)
            {
                // Assign the retrieved data to the UI controls
                empNumberEntry.Text = getData.EMPID.ToString();
                empNameEntry.Text = getData.EMPNAME;
                hoursWorkedEntry.Text = getData.HOURSWORK.ToString();
                empStatusEntry.Text = getData.GROSSINCOME.ToString(); ;
                civilStatusEntry.Text = getData.BONUS.ToString(); ;
                ratePerHourEntry.Text = getData.DEDUCTION.ToString();
                basicIncomeEntry.Text = getData.NETINCOME.ToString();
            }
        }


    }
}