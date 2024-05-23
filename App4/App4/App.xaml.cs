using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace App4
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Create instances of your pages
            var mainPage = new MainPage();


            // Create a NavigationPage with the main page as the root
            MainPage = new NavigationPage(mainPage);
        }

        static SQLiteHelper db;

        public static SQLiteHelper SQLitedb
        {
            get
            {
                if (db == null)
                {
                    db = new SQLiteHelper(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "newdata.db3"));
                }
                return db;
            }
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
