using Gts_Schedule.AppointmentEditor.Model;
using Gts_Schedule.Common;
using Gts_Schedule.Dependency;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;


namespace Gts_Schedule.AppointmentEditor.ViewModels
{
    [Preserve(AllMembers = true)]

    #region AppointmentEditorViewModel
    public class AppointmentEditorViewModel : INotifyPropertyChanged
    {
        private List<string> currentDayMeetings;
        private List<string> minTimeMeetings;
        private List<Color> color_collection;


        #region ScheduleType

        private ObservableCollection<ScheduleTypeClass> scheduleTypeCollection = new ObservableCollection<ScheduleTypeClass>();

        public ObservableCollection<ScheduleTypeClass> ScheduleTypeCollection
        {
            get { return scheduleTypeCollection; }
            set
            {
                scheduleTypeCollection = value;
                RaiseOnPropertyChanged("ScheduleTypeCollection");
            }
        }
        #endregion

        #region ListOfMeeting

        private ObservableCollection<Model.Meeting> listOfMeeting;

        public ObservableCollection<Model.Meeting> ListOfMeeting
        {
            get { return listOfMeeting; }
            set
            {
                listOfMeeting = value;
                RaiseOnPropertyChanged("ListOfMeeting");
            }
        }
        #endregion

        #region HeaderLabelValue
        private string headerLabelValue = DateTime.Today.Date.ToString("MMMM yyyy");

        public string HeaderLabelValue
        {
            get { return headerLabelValue; }
            set
            {
                headerLabelValue = value;
                RaiseOnPropertyChanged("HeaderLabelValue");
            }
        }
        #endregion

        public AppointmentEditorViewModel()
        {
            ListOfMeeting = new ObservableCollection<Model.Meeting>();
            InitializeDataForBookings();
            //BookingAppointments();
            AddScheduleType();
        }

        private void AddScheduleType()
        {
            this.ScheduleTypeCollection.Add(new ScheduleTypeClass() { ScheduleType = "Day view" });
            this.ScheduleTypeCollection.Add(new ScheduleTypeClass() { ScheduleType = "Week view" });
            this.ScheduleTypeCollection.Add(new ScheduleTypeClass() { ScheduleType = "Work week view" });
            this.ScheduleTypeCollection.Add(new ScheduleTypeClass() { ScheduleType = "Month view" });
        }

        #region BookingAppointments

        private void BookingAppointments()
        {
            //Should get this from apointment database
            Random randomTime = new Random();
            List<Point> randomTimeCollection = GettingTimeRanges();

            DateTime date;
            DateTime DateFrom = DateTime.Now.AddDays(-10);
            DateTime DateTo = DateTime.Now.AddDays(10);
            DateTime dateRangeStart = DateTime.Now.AddDays(-3);
            DateTime dateRangeEnd = DateTime.Now.AddDays(3);

            for (date = DateFrom; date < DateTo; date = date.AddDays(1))
            {
                if ((DateTime.Compare(date, dateRangeStart) > 0) && (DateTime.Compare(date, dateRangeEnd) < 0))
                {
                    for (int AdditionalAppointmentIndex = 0; AdditionalAppointmentIndex < 3; AdditionalAppointmentIndex++)
                    {
                        Model.Meeting meeting = new Model.Meeting();
                        int hour = (randomTime.Next((int)randomTimeCollection[AdditionalAppointmentIndex].X, (int)randomTimeCollection[AdditionalAppointmentIndex].Y));
                        meeting.From = new DateTime(date.Year, date.Month, date.Day, hour, 0, 0);
                        meeting.To = (meeting.From.AddHours(1));
                        meeting.EventName = currentDayMeetings[randomTime.Next(2)];
                        meeting.color = EvenColor(meeting.EventName);
                        meeting.IsAllDay = false;
                        meeting.StartTimeZone = string.Empty;
                        meeting.EndTimeZone = string.Empty;
                        //ListOfMeeting.Add(meeting);
                    }
                }
                else
                {
                    Model.Meeting meeting = new Model.Meeting();
                    meeting.From = new DateTime(date.Year, date.Month, date.Day, randomTime.Next(9, 11), 0, 0);
                    meeting.To = (meeting.From.AddDays(2).AddHours(1));
                    meeting.EventName = currentDayMeetings[randomTime.Next(2)];
                    meeting.color = EvenColor(meeting.EventName); //color_collection[randomTime.Next(9)];
                    meeting.IsAllDay = true;
                    meeting.StartTimeZone = string.Empty;
                    meeting.EndTimeZone = string.Empty;
                    ListOfMeeting.Add(meeting);
                }
            }

            // Minimum Height Meetings
            DateTime minDate;
            DateTime minDateFrom = DateTime.Now.AddDays(-2);
            DateTime minDateTo = DateTime.Now.AddDays(2);

            for (minDate = minDateFrom; minDate < minDateTo; minDate = minDate.AddDays(1))
            {
                Model.Meeting meeting = new Model.Meeting();
                meeting.From = new DateTime(minDate.Year, minDate.Month, minDate.Day, randomTime.Next(9, 18), 30, 0);
                meeting.To = meeting.From;
                meeting.EventName = minTimeMeetings[randomTime.Next(0, 4)];
                meeting.color = color_collection[randomTime.Next(0, 10)];
                meeting.StartTimeZone = string.Empty;
                meeting.EndTimeZone = string.Empty;
                // Setting Mininmum Appointment Height for Schedule Appointments
                if (Device.RuntimePlatform == "Android")
                    meeting.MinimumHeight = 50;
                else
                    meeting.MinimumHeight = 25;
                ListOfMeeting.Add(meeting);
            }
        }

        #endregion BookingAppointments

        #region GettingTimeRanges

        private List<Point> GettingTimeRanges()
        {
            List<Point> randomTimeCollection = new List<Point>();
            randomTimeCollection.Add(new Point(9, 11));
            randomTimeCollection.Add(new Point(12, 14));
            randomTimeCollection.Add(new Point(15, 17));

            return randomTimeCollection;
        }

        #endregion GettingTimeRanges

        #region InitializeDataForBookings
        public Color EvenColor(string even)
        {
            if (even.ToLower() == "Job".ToLower())
                return Color.Red;
            if (even.ToLower() == "Sales".ToLower())
                return Color.Blue;
            if (even.ToLower() == "Maintenance".ToLower())
                return Color.Yellow;
            return Color.Orange;
        }
     
        private async void InitializeDataForBookings()
        {
            ListOfMeeting=await CommonVariables.GetSettings();
            /*List<Todo> meeting_list = await App.Database.GetItemsAsync();
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
            }*/


            //for test job type
            foreach(var item in ListOfMeeting)
            {
                Debug.WriteLine(item.PostCode);
                Debug.WriteLine( item.From );
                Debug.WriteLine(item.To);
            }

            /*
            
            DateTime minDate;
            DateTime minDateFrom = DateTime.Now.AddDays(-2);
            DateTime minDateTo = DateTime.Now.AddDays(2);   
            ListOfMeeting.Add(
            new Model.Meeting()
            {
                EventName = "Lee",
                PostCode = "Lee",
                ContactID = "Lee",
                Description = "ghh",
                From = new DateTime(DateTime.Now.AddDays(-2).Year, DateTime.Now.AddDays(-2).Month, DateTime.Now.AddDays(-2).Day, new Random().Next(9, 18), 30, 0),
                To = new DateTime(DateTime.Now.AddDays(-2).Year, DateTime.Now.AddDays(-2).Month, DateTime.Now.AddDays(-2).Day, new Random().Next(9, 18), 30, 0),
                StartTimeZone = string.Empty,
                EndTimeZone = string.Empty,
                MinimumHeight = height,
                IsAllDay = true,
                Done = false,
                type = "Job",
                responsible = "Other",
            });*/


            currentDayMeetings = new List<string>();
            currentDayMeetings.Add("Job");
            currentDayMeetings.Add("Sales");
            currentDayMeetings.Add("Maintenance");


            // MinimumHeight Appointment Subjects
            minTimeMeetings = new List<string>();
            minTimeMeetings.Add("Work log alert");
            minTimeMeetings.Add("Birthday wish alert");
            minTimeMeetings.Add("Task due date");
            minTimeMeetings.Add("Status mail");
            minTimeMeetings.Add("Start sprint alert");

            color_collection = new List<Color>();
            color_collection.Add(Color.FromHex("#FF339933"));
            color_collection.Add(Color.FromHex("#FF00ABA9"));
            color_collection.Add(Color.FromHex("#FFE671B8"));
            color_collection.Add(Color.FromHex("#FF1BA1E2"));
            color_collection.Add(Color.FromHex("#FFD80073"));
            color_collection.Add(Color.FromHex("#FFA2C139"));
            color_collection.Add(Color.FromHex("#FFA2C139"));
            color_collection.Add(Color.FromHex("#FFD80073"));
            color_collection.Add(Color.FromHex("#FF339933"));
            color_collection.Add(Color.FromHex("#FFE671B8"));
            color_collection.Add(Color.FromHex("#FF00ABA9"));

        }

        #endregion InitializeDataForBookings

        #region Property Changed Event

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaiseOnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
    #endregion

    #region ScheduleTypeClass
    public class ScheduleTypeClass : INotifyPropertyChanged
    {

        #region ScheduleType

        private string scheduleType = " ";

        public string ScheduleType
        {
            get { return scheduleType; }
            set
            {
                scheduleType = value;
                RaiseOnPropertyChanged("ScheduleType");
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaiseOnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    #endregion
}





