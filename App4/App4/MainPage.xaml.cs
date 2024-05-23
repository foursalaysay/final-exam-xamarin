using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xamarin.Forms;

namespace App4
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


        // MAIN FORMULAS
        public double returnGrossIncome(double HW, double HR, double OT, double B)
        {
            double finalHW;
            if(HW > 80)
            {
                finalHW = 80;
            }
            else
            {
                finalHW = HW;
            }

            double preGI = finalHW * HR;
            return preGI + OT + B;
        }

        public double returnDeduction(double FT, double ST, double SST, double M, double HI)
        {
            return FT + ST + SST + M + HI;
        }

        public double returnNetIncome(double GI, double D)
        {
            return GI - D;
        }
        

        // CONDITIONS
        public double returnOvertimeHours(double HW)
        {
            double OT;
            if(HW > 80)
            {
                OT = HW - 80;
            }
            else
            {
                OT = 0;
            }
            return OT;
        }

        public double returnOvertimePay(double HR, double OT)
        {
            double OTPay;
            double finalOTPay;

            OTPay = 1.5 * HR;
            finalOTPay = OT * OTPay;

            return finalOTPay;
        }

        public double returnBonus(double HW)
        {
            double bonus; 
           if(HW > 85)
            {
                bonus = 1000;
            }
            else
            {
                bonus = 0;
            }
            return bonus;
        }

        public double returnFTR(double GI)
        {
            double percent;
            if(GI > 3000)
            {
                percent = 0.12 * GI;
            }
            else
            {
                percent = 0.10 * GI;
            }
            return percent;
        }

        public double returnSTR(double GI)
        {
            double percent;
            if (GI > 3000)
            {
                percent = 0.06 * GI;
            }
            else
            {
                percent = 0.05 * GI;
            }
            return percent;
        }

        public double returnSSTR(double GI)
        {
            return GI * 0.062;
        } 

        public double returnMTR(double GI)
        {
            return GI * 0.0145;
        }

        public double returnHIP()
        {
            return 400;
        }

        // GLOBAL VARIABLES

        public string name;
        public double hWork;
        public double HRate = 65.5;

        //GETTING VALUES FROM FORMULA

        public double regularHW;
        public double oTime;
        public double bonusValue;
        public double grossIncomeValue;
        public double overtimePayValue;
        public double overtimeHourValue;

        // TAXES
        public double federalTR;
        public double stateTR;
        public double socialSTR;
        public double medicareTR;
        public double HealthIP;

        public double deductionValue;
        public double netIncome;


        public bool isFieldsEmpty()
        {
            if (string.IsNullOrEmpty(empName.Text) || string.IsNullOrEmpty(hoursWork.Text))
            {
                DisplayAlert("Required", "Invalid Please Enter/Select Required Fields", "OK");
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isUpdateEmpty()
        {
            if (string.IsNullOrEmpty(hoursWork.Text) || string.IsNullOrEmpty(empId.Text))
            {
                DisplayAlert("Required", "Invalid Please Enter/Select Required Fields", "OK");
                return true;
            }
            else
            {
                return false;
            }
        }




        // ADDING BUTTON
        private async void Button_Clicked(object sender, EventArgs e)
        {
            bool isNotEmp = isFieldsEmpty();

            if (!isNotEmp)
            {
                //GETTING THE VALUES
                name = empName.Text;
                hWork = double.Parse(hoursWork.Text);

                regularHW = hWork;
                overtimeHourValue = returnOvertimeHours(regularHW);
                overtimePayValue = returnOvertimePay(HRate, overtimeHourValue);



                oTime = returnOvertimeHours(regularHW);
                bonusValue = returnBonus(regularHW);
                grossIncomeValue = returnGrossIncome(regularHW, HRate, overtimePayValue, bonusValue);

                // TAXES

                federalTR = returnFTR(grossIncomeValue);
                stateTR = returnSTR(grossIncomeValue);
                socialSTR = returnSSTR(grossIncomeValue);
                medicareTR = returnMTR(grossIncomeValue);
                HealthIP = returnHIP();

                deductionValue = federalTR + stateTR + socialSTR + medicareTR + HealthIP;
                netIncome = grossIncomeValue - deductionValue;


                Data data = new Data()
                {
                    EMPNAME = name,
                    HOURSWORK = hWork,
                    GROSSINCOME = grossIncomeValue,
                    BONUS = bonusValue,
                    DEDUCTION = deductionValue,
                    NETINCOME = netIncome
                };

                // Proceed with saving data
                await App.SQLitedb.SaveItemAsync(data);
                await DisplayAlert("Success", "Successfully saved!", "OK");


                Data latestData = await App.SQLitedb.ReadLatestItemAsync();

                if (latestData != null)
                {
                    int autoValue = latestData.EMPID;
                    await Navigation.PushAsync(new Add(autoValue));
                }
                else
                {
                    // Handle the case when no data is available
                    Console.WriteLine("No data available in the database.");
                }
            }





        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            
            try
            {
                int intID = int.Parse(empId.Text);
                bool isNotEmp = isUpdateEmpty();

                if (!isNotEmp)
                {
                    //GETTING THE VALUES
                    
                    name = empName.Text;
                    hWork = double.Parse(hoursWork.Text);

                    regularHW = hWork;
                    overtimeHourValue = returnOvertimeHours(regularHW);
                    overtimePayValue = returnOvertimePay(HRate, overtimeHourValue);



                    oTime = returnOvertimeHours(regularHW);
                    bonusValue = returnBonus(regularHW);
                    grossIncomeValue = returnGrossIncome(regularHW, HRate, overtimePayValue, bonusValue);

                    // TAXES

                    federalTR = returnFTR(grossIncomeValue);
                    stateTR = returnSTR(grossIncomeValue);
                    socialSTR = returnSSTR(grossIncomeValue);
                    medicareTR = returnMTR(grossIncomeValue);
                    HealthIP = returnHIP();

                    deductionValue = federalTR + stateTR + socialSTR + medicareTR + HealthIP;
                    netIncome = grossIncomeValue - deductionValue;

                    Data getRecord = await App.SQLitedb.ReadItemAsync(intID);
                    string newName;

                    if(name == "")
                    {
                        newName = getRecord.EMPNAME;
                    }
                    else
                    {
                        newName = name;
                    }

                    Data updatedData = new Data()
                    {
                        EMPNAME = newName,
                        HOURSWORK = hWork,
                        GROSSINCOME = grossIncomeValue,
                        BONUS = bonusValue,
                        DEDUCTION = deductionValue,
                        NETINCOME = netIncome
                    };

                    // Proceed with saving data
                    Data updatedRecord = await App.SQLitedb.UpdateItemAsync(updatedData, intID);

                    if (updatedRecord != null)
                    {
                        await DisplayAlert("Success", "Successfully Updated!", "OK");
                        await Navigation.PushAsync(new Add(intID));
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to update record.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error Occurred: {ex.Message}", "OK");
            }
        }
    }
}
