using Gts_Schedule.AppointmentEditor.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Gts_Schedule.Common
{
    public static class CommonVariables
    {
        public static string GeneralSettings= "......";
        static string getTask = "getTask";
        public static bool localDatabase = true;
        private static ISettings AppSettings// =>CrossSettings.Current; 
        {
            get
            {
                if (CrossSettings.IsSupported)
                    return CrossSettings.Current;

                return null; // or your custom implementation 
            }
        }
        public static string SERVER
        {
            get => AppSettings.GetValueOrDefault(nameof(SERVER), GeneralSettings);
            set => AppSettings.AddOrUpdateValue(nameof(SERVER), value);
        }
        public static async Task<ObservableCollection<AppointmentEditor.Model.Meeting>> GetSettings()
        {
            ObservableCollection<AppointmentEditor.Model.Meeting> ListOfMeeting = new ObservableCollection<AppointmentEditor.Model.Meeting>();
            if (CommonVariables.localDatabase)
            {

                // await App.Database.ClearDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3")).ConfigureAwait(false);
                //Local Database
                List<Todo> meeting_list = await App.Database.GetItemsAsync();
                foreach (var item in meeting_list)
                {
                    Meeting it = new Meeting
                    {
                        id = item.id,
                        ContactID = item.ContactID,
                        Description = item.Description,
                        Done = item.Done,
                        EndTimeZone = item.EndTimeZone,
                        StartTimeZone = item.StartTimeZone,
                        EventName = item.EventName,
                        From = item.From,
                        FromDate = item.FromDate,
                        To = item.To,
                        ToDate = item.ToDate,
                        IsAllDay = item.IsAllDay,
                        MinimumHeight = item.MinimumHeight,
                        PostCode = item.PostCode,
                        responsible = item.responsible,
                        type = item.type,


                    };
                    ListOfMeeting.Add(it);
                }
                return ListOfMeeting;

            }
            else
            {
                // Setting Mininmum Appointment Height for Schedule Appointments
                double height;
                if (Xamarin.Forms.Device.RuntimePlatform == "Android")
                    height = 50;
                else
                    height = 25;
                string guidPostUrl = SERVER + getTask;
                HttpClient client = new HttpClient();

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, guidPostUrl);

                try
                {
                    var result = await client.SendAsync(request);
                    var content = await result.Content.ReadAsStringAsync();
                    if (result.StatusCode != HttpStatusCode.OK || content.Equals(string.Empty))
                        throw new Exception();

                    ListOfMeeting = JsonConvert.DeserializeObject<ObservableCollection<Meeting>>(content);
                    foreach (var item in ListOfMeeting)
                    {
                        item.MinimumHeight = height;
                        if (item.type != null)
                        {
                            if (item.type.ToLower() == "Job".ToLower())
                                item.color = Color.Red;
                            else if (item.type.ToLower() == "Sales".ToLower())
                                item.color = Color.Blue;
                            else if (item.type.ToLower() == "Maintenance".ToLower())
                                item.color = Color.Brown;
                            else
                                item.color = Color.Orange;
                        }
                        else
                            item.color = Color.Orange;
                    }

                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Warning", "Database Offline, Update Failed!", "Ok");
                  
                }
                client.Dispose();
                return ListOfMeeting;

            }

           
        }
    }
}
