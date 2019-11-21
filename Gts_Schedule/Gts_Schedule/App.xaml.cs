using Gts_Schedule.Data;
using Gts_Schedule.Dependency;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Gts_Schedule
{
    public partial class App : Application
    {
        static AppointmentDatabase database;
        public App()
        {
            InitializeComponent();
            Resources = new ResourceDictionary();
            Resources.Add("primaryGreen", Color.FromHex("91CA47"));
            Resources.Add("primaryDarkGreen", Color.FromHex("6FA22E"));

            var nav = CrossConnectivity.Current.IsConnected
                ? new NavigationPage(new Gts_Schedule.AppointmentEditor.Views.AppointmentEditor()): new NavigationPage(new NoNetworkPage());
            nav.BarBackgroundColor = (Color)App.Current.Resources["primaryGreen"];
            nav.BarTextColor = Color.White;

            MainPage = nav;

        }

        public static AppointmentDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new AppointmentDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                }                   
                return database;
            }
        }

        public int ResumeAtTodoId { get; set; }

        protected override void OnStart()
        {
            // Handle when your app starts
            CrossConnectivity.Current.ConnectivityChanged += HandleConnectivityChanged;
        }

        void HandleConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            Type currentPage = this.MainPage.GetType();
            //new NavigationPage(new Gts_Schedule.AppointmentEditor.Views.AppointmentEditor())
            if (e.IsConnected )
                this.MainPage = new NavigationPage(new Gts_Schedule.AppointmentEditor.Views.AppointmentEditor());
            else if (!e.IsConnected && currentPage != typeof(NoNetworkPage))
                this.MainPage = new NoNetworkPage();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
