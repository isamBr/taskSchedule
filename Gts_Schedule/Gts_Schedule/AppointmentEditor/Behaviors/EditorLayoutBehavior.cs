using Gts_Schedule.AppointmentEditor.Model;
using Gts_Schedule.AppointmentEditor.ViewModels;
using Gts_Schedule.Common;
using Gts_Schedule.Dependency;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Gts_Schedule.AppointmentEditor.Behaviors
{
    internal class EditorLayoutBehavior : Behavior<StackLayout>
    {
        bool update = true;
        #region Fields

        private ScrollView editorScrollView;

        private Button saveButton;
        private Button cancelButton;

        private DatePicker endDatePicker;
        //isam add
        private Picker typeDatePicker;
        private Picker responsiblePicker;
        private Picker responsiblePicker1;
        private Picker responsiblePicker2;
        private Switch done;
        private Entry descriptionText;
        private Entry contactText;

        private TimePicker endTimePicker;
        private Model.Meeting selectedAppointment;
        private DateTime selectedDate;
        private DatePicker startDatePicker;
        private TimePicker startTimePicker;
        private StackLayout editorLayout;
        private Switch switchAllDay;
        private Entry eventNameText;
        private Entry PostCodeText;

        private Grid endTimeLabelLayout;
        private Grid startDateTimePickerLayout;
        private Grid startDatePickerLayout;
        private Grid startTimePickerLayout;
        private Grid EndDateTimePickerLayout;
        private Grid endDatePickerLayout;
        private Grid endTimePickerLayout;
        private Grid startTimeLabelLayout;
        private Grid organizerLayout;

        private Picker startTimeZonePicker;
        private Picker endTimeZonePicker;

        private string startTimeZone;
        private string endTimeZone;

        #endregion

        #region OnAttached
        protected override void OnAttachedTo(StackLayout bindable)
        {
            editorLayout = bindable;

            editorScrollView = bindable.FindByName<ScrollView>("editorScrollView");
            eventNameText = bindable.FindByName<Entry>("eventNameText");
            PostCodeText = bindable.FindByName<Entry>("organizerText");

            cancelButton = bindable.FindByName<Button>("cancelButton");
            saveButton = bindable.FindByName<Button>("saveButton");

            endDatePicker = bindable.FindByName<DatePicker>("endDate_picker");
            endTimePicker = bindable.FindByName<TimePicker>("endTime_picker");

            startDatePicker = bindable.FindByName<DatePicker>("startDate_picker");
            startTimePicker = bindable.FindByName<TimePicker>("startTime_picker");
            switchAllDay = bindable.FindByName<Switch>("switchAllDay");

            startTimeZonePicker = bindable.FindByName<Picker>("startTimeZonePicker");
            startTimeZonePicker.SelectedIndex = 0;
            startTimeZonePicker.ItemsSource = ViewModels.TimeZoneCollection.TimeZoneList;
            startTimeZonePicker.SelectedIndexChanged += StartTimeZonePicker_SelectedIndexChanged;

            endTimeZonePicker = bindable.FindByName<Picker>("endTimeZonePicker");
            endTimeZonePicker.SelectedIndex = 0;
            endTimeZonePicker.ItemsSource = ViewModels.TimeZoneCollection.TimeZoneList;
            endTimeZonePicker.SelectedIndexChanged += EndTimeZonePicker_SelectedIndexChanged;

            endTimeLabelLayout = bindable.FindByName<Grid>("endTimeLabel_layout");
            startDateTimePickerLayout = bindable.FindByName<Grid>("StartdateTimePicker_layout");
            startDatePickerLayout = bindable.FindByName<Grid>("start_datepicker_layout");
            organizerLayout = bindable.FindByName<Grid>("organizer_layout");

            startTimePickerLayout = bindable.FindByName<Grid>("start_timepicker_layout");
            startTimeLabelLayout = bindable.FindByName<Grid>("startTimeLabel_layout");
            EndDateTimePickerLayout = bindable.FindByName<Grid>("EndDateTimePicker_layout");

            endDatePickerLayout = bindable.FindByName<Grid>("end_datepicker_layout");
            EndDateTimePickerLayout = bindable.FindByName<Grid>("EndDateTimePicker_layout");
            endTimePickerLayout = bindable.FindByName<Grid>("end_timepicker_layout");

            //isam add
            typeDatePicker = bindable.FindByName<Picker>("typePicker");
            responsiblePicker = bindable.FindByName<Picker>("responsiblePicker");
            responsiblePicker1 = bindable.FindByName<Picker>("responsiblePicker1");
            responsiblePicker2 = bindable.FindByName<Picker>("responsiblePicker2");
            done = bindable.FindByName<Switch>("switchDone");
            descriptionText = bindable.FindByName<Entry>("descriptionText");
            contactText = bindable.FindByName<Entry>("contactText");

            //// Editor Layout Date and time Picker Alignment for UWP
            if (Device.RuntimePlatform == "UWP" && Device.Idiom == TargetIdiom.Desktop)
            {
                startDatePicker.HorizontalOptions = LayoutOptions.StartAndExpand;
                startTimePicker.HorizontalOptions = LayoutOptions.StartAndExpand;

                endDatePicker.HorizontalOptions = LayoutOptions.StartAndExpand;
                endTimePicker.HorizontalOptions = LayoutOptions.StartAndExpand;

                startDatePicker.WidthRequest = 450;
                startTimePicker.WidthRequest = 450;

                endDatePicker.WidthRequest = 450;
                endTimePicker.WidthRequest = 450;
            }

            saveButton.Clicked += SaveButton_Clicked;
            cancelButton.Clicked += CancelButton_Clicked;
            switchAllDay.Toggled += SwitchAllDay_Toggled;
            done.Toggled += done_Toggled;
        }

        #endregion

        #region Start TimeZone Picker_Selected

        private void StartTimeZonePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as Picker).SelectedItem as string == "Default")
            {
                startTimeZone = string.Empty;
            }
            else
            {
                startTimeZone = (sender as Picker).SelectedItem as string;
            }
        }

        #endregion

        #region End TimeZone Picker_Selected

        private void EndTimeZonePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as Picker).SelectedItem as string == "Default")
            {
                endTimeZone = string.Empty;
            }
            else
            {
                endTimeZone = (sender as Picker).SelectedItem as string;
            }
        }

        #endregion

        #region SwitchAllDay_Toggled
        private void SwitchAllDay_Toggled(object sender, ToggledEventArgs e)
        {
            if ((sender as Switch).IsToggled)
            {
                startTimePicker.Time = new TimeSpan(12, 0, 0);
                startTimePicker.IsEnabled = false;
                endTimePicker.Time = new TimeSpan(12, 0, 0);
                endTimePicker.IsEnabled = false;
                startTimeZonePicker.IsEnabled = false;
                endTimeZonePicker.IsEnabled = false;
            }
            else
            {
                startTimePicker.IsEnabled = true;
                endTimePicker.IsEnabled = true;
                (sender as Switch).IsToggled = false;
                startTimeZonePicker.IsEnabled = true;
                endTimeZonePicker.IsEnabled = true;
            }

        }
        #endregion

        #region done_Toggled
        private void done_Toggled(object sender, ToggledEventArgs e)
        {
            if ((sender as Switch).IsToggled)
            {

            }
            else
            {
                (sender as Switch).IsToggled = false;
            }

        }
        #endregion

        #region CancelButton_Clicked
        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            ViewModels.ScheduleAppointmentModifiedEventArgs args = new ViewModels.ScheduleAppointmentModifiedEventArgs();
            args.Appointment = null;
            args.IsModified = false;
            (editorLayout.BindingContext as ViewModels.EditorLayoutViewModel).OnAppointmentModified(args);
            editorLayout.IsVisible = false;
        }
        #endregion

        #region SaveButton_Clicked
        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            var endDate = endDatePicker.Date;
            var startDate = startDatePicker.Date;
            var endTime = endTimePicker.Time;
            var startTime = startTimePicker.Time;


            if (endDate < startDate)
            {
                Application.Current.MainPage.DisplayAlert("", "End time should be greater than start time", "OK");
            }
            else if (endDate == startDate)
            {
                if (endTime < startTime)
                {
                    Application.Current.MainPage.DisplayAlert("", "End time should be greater than start time", "OK");
                }
                else
                {
                    AppointmentDetails();
                    editorLayout.IsVisible = false;
                }
            }
            else
            {
                AppointmentDetails();
                editorLayout.IsVisible = false;
            }
        }
        #endregion

        #region AppointmentDetails

        private async void AppointmentDetails()
        {
            if (selectedAppointment == null)
            {
                selectedAppointment = new Model.Meeting();
                selectedAppointment.color = Color.FromHex("#5EDAF2");
                update = false;
            }
            else
            {
                update = true;
            }
            if (eventNameText.Text != null)
            {
                selectedAppointment.EventName = eventNameText.Text.ToString();
                if (string.IsNullOrEmpty(selectedAppointment.EventName))
                    selectedAppointment.EventName = "Untitled";
            }
            if (PostCodeText.Text != null)
            {
                selectedAppointment.PostCode = PostCodeText.Text.ToString();
            }

            if (descriptionText.Text != null)
            {
                selectedAppointment.Description = descriptionText.Text.ToString();
            }
            if (contactText.Text != null)
            {
                selectedAppointment.ContactID = contactText.Text.ToString();
            }

            if (typeDatePicker.SelectedItem != null)
            {
                selectedAppointment.type = typeDatePicker.SelectedItem.ToString();
            }
            else
            {
                selectedAppointment.type = "Job";
            }

            if (responsiblePicker.SelectedItem != null)
            {
                selectedAppointment.responsible = responsiblePicker.SelectedItem.ToString();
                if (responsiblePicker1.SelectedItem!=null)
                {
                    if(responsiblePicker1.SelectedItem.ToString()!="NONE")
                        selectedAppointment.responsible = selectedAppointment.responsible+"/"+responsiblePicker1.SelectedItem.ToString();
                }
                if (responsiblePicker2.SelectedItem != null)
                {
                    if (responsiblePicker2.SelectedItem.ToString() != "NONE")
                        selectedAppointment.responsible = selectedAppointment.responsible + "/" + responsiblePicker2.SelectedItem.ToString();
                }
            }
            else
            {
                selectedAppointment.responsible = "OTH";
                if (responsiblePicker1.SelectedItem != null)
                {
                    if (responsiblePicker1.SelectedItem.ToString() != "NONE")
                        selectedAppointment.responsible = selectedAppointment.responsible + "/" + responsiblePicker1.SelectedItem.ToString();
                }
                if (responsiblePicker2.SelectedItem != null)
                {
                    if (responsiblePicker2.SelectedItem.ToString() != "NONE")
                        selectedAppointment.responsible = selectedAppointment.responsible + "/" + responsiblePicker2.SelectedItem.ToString();
                }

            }


            selectedAppointment.From = startDatePicker.Date.Add(startTimePicker.Time);
            selectedAppointment.To = endDatePicker.Date.Add(endTimePicker.Time);
            selectedAppointment.IsAllDay = switchAllDay.IsToggled;
            selectedAppointment.Done = done.IsToggled;
            selectedAppointment.StartTimeZone = startTimeZone;
            selectedAppointment.EndTimeZone = endTimeZone;
            if (selectedAppointment.type == "Job")
            {
                selectedAppointment.color = Color.Red;
            }
            else if (selectedAppointment.type == "Sales")
            {
                selectedAppointment.color = Color.Blue;
            }
            else if (selectedAppointment.type == "Maintenance")
            {
                selectedAppointment.color = Color.Brown;
            }
            else
            {
                selectedAppointment.color = Color.Orange;
            }

            ViewModels.ScheduleAppointmentModifiedEventArgs args = new ViewModels.ScheduleAppointmentModifiedEventArgs();

            Todo item = new Todo
            {
                id = selectedAppointment.id,
                ContactID= selectedAppointment.ContactID,
                Description= selectedAppointment.Description,
                Done=selectedAppointment.Done,
                EndTimeZone= selectedAppointment.EndTimeZone,
                StartTimeZone= selectedAppointment.StartTimeZone,
                EventName= selectedAppointment.EventName,
                From= selectedAppointment.From,
                FromDate= selectedAppointment.FromDate,
                To= selectedAppointment.To,
                ToDate= selectedAppointment.ToDate,
                IsAllDay= selectedAppointment.IsAllDay,
                MinimumHeight= selectedAppointment.MinimumHeight,
                PostCode= selectedAppointment.PostCode,
                responsible= selectedAppointment.responsible,
                type= selectedAppointment.type

            };
            if (update)
            {
                if (CommonVariables.localDatabase)
                    await App.Database.SaveItemAsync(item);
                else
                    await UpdateDatabase(selectedAppointment);

            }
            else
            {
                if (CommonVariables.localDatabase)
                    await App.Database.SaveItemAsync(item);
                else
                    await AddDatabase(selectedAppointment);
                // await App.Database.DeleteItemAsync(item);
                //List<Todo> meeting_list = await App.Database.GetItemsAsync();



            }

            args.Appointment = selectedAppointment;
            args.IsModified = true;
            (editorLayout.BindingContext as ViewModels.EditorLayoutViewModel).OnAppointmentModified(args);
        }

        #endregion
        public async Task UpdateDatabase(Meeting appointment)
        {
            string jsonData;
            jsonData = JsonConvert.SerializeObject(appointment);
            string guidPostUrl = CommonVariables.SERVER + "updateTask";
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, guidPostUrl);
            request.Content = new StringContent(jsonData);
            var content = string.Empty;
            try
            {
                var result = await httpClient.SendAsync(request);
                content = await result.Content.ReadAsStringAsync();

                if (result.StatusCode != HttpStatusCode.OK || content.Equals(string.Empty))
                    throw new Exception();

                await App.Current.MainPage.DisplayAlert("Info", content.ToString(), "Ok");
                DependencyService.Get<ITextToSpeech>().Speak(content.ToString());

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Warning", "Database Offline, Update Failed!", "Ok");

                return;
            }
        }


        public async Task AddDatabase(Meeting appointment)
        {
            string jsonData;

            jsonData = JsonConvert.SerializeObject(appointment);
            string guidPostUrl = CommonVariables.SERVER + "addTask";
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, guidPostUrl);
            request.Content = new StringContent(jsonData);
            var content = string.Empty;
            try
            {
                var result = await httpClient.SendAsync(request);
                content = await result.Content.ReadAsStringAsync();

                if (result.StatusCode != HttpStatusCode.OK || content.Equals(string.Empty))
                    throw new Exception();
             
                 await App.Current.MainPage.DisplayAlert("Info", content.ToString(), "Ok");
                DependencyService.Get<ITextToSpeech>().Speak(content.ToString());

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Warning", "Database Offline, Update Failed!", "Ok");

                return;
            }
        }


        #region OpenEditor

        public void OpenEditor(Model.Meeting appointment, DateTime date)
        {
            editorScrollView.ScrollToAsync(0, 0, false);
            cancelButton.BackgroundColor = Color.FromHex("#E5E5E5");
            saveButton.BackgroundColor = Color.FromHex("#2196F3");
            eventNameText.Placeholder = "Task";
            PostCodeText.Placeholder = "PostCode";
            descriptionText.Placeholder = "Notes";
            contactText.Placeholder = "Mobile";

            selectedAppointment = null;
            if (appointment != null)
            {
                selectedAppointment = appointment;
                selectedDate = date;
            }
            else
            {
                selectedDate = date;
            }
            UpdateEditor();
        }

        #endregion

        #region UpdateEditor

        private void UpdateEditor()
        {
            if (selectedAppointment != null)
            {
                if (selectedAppointment.responsible.Contains("/"))
                {
                    string [] item = selectedAppointment.responsible.Split('/');
                    if(item.Count()==2)
                    {
                        responsiblePicker.SelectedItem = item[0];
                        responsiblePicker1.SelectedItem = item[1];
                    }
                    else
                    {
                        responsiblePicker.SelectedItem = item[0];
                        responsiblePicker1.SelectedItem = item[1];
                        responsiblePicker2.SelectedItem = item[2];
                    }                     
                }                  
                else
                {
                    responsiblePicker.SelectedItem = selectedAppointment.responsible;
                    responsiblePicker1.SelectedItem = "NONE";
                    responsiblePicker2.SelectedItem = "NONE";
                }      
                eventNameText.Text = selectedAppointment.EventName.ToString();
                PostCodeText.Text = selectedAppointment.PostCode;
                descriptionText.Text = selectedAppointment.Description;
                contactText.Text = selectedAppointment.ContactID;
                typeDatePicker.SelectedItem = selectedAppointment.type;
                startDatePicker.Date = selectedAppointment.From;
                endDatePicker.Date = selectedAppointment.To;
                startTimeZonePicker.SelectedIndex = GetTimeZoneIndex(selectedAppointment.StartTimeZone);
                endTimeZonePicker.SelectedIndex = GetTimeZoneIndex(selectedAppointment.EndTimeZone);
                if (!selectedAppointment.Done)
                {
                    done.IsToggled = false;
                }
                else
                {
                    done.IsToggled = true;
                }
                if (!selectedAppointment.IsAllDay)
                {
                    startTimePicker.Time = new TimeSpan(selectedAppointment.From.Hour, selectedAppointment.From.Minute, selectedAppointment.From.Second);
                    endTimePicker.Time = new TimeSpan(selectedAppointment.To.Hour, selectedAppointment.To.Minute, selectedAppointment.To.Second);
                    switchAllDay.IsToggled = false;
                }
                else
                {
                    startTimePicker.Time = new TimeSpan(12, 0, 0);
                    startTimePicker.IsEnabled = false;
                    endTimePicker.Time = new TimeSpan(12, 0, 0);
                    endTimePicker.IsEnabled = false;
                    switchAllDay.IsToggled = true;
                }
            }
            else
            {
                eventNameText.Text = "";
                PostCodeText.Text = "";
                descriptionText.Text = "";
                contactText.Text = "";
                switchAllDay.IsToggled = false;
                done.IsToggled = false;
                responsiblePicker.SelectedItem = "OTH";
                responsiblePicker1.SelectedItem ="NONE";
                responsiblePicker2.SelectedItem ="NONE";
                typeDatePicker.SelectedItem = "Job";
                startDatePicker.Date = selectedDate;
                startTimePicker.Time = new TimeSpan(selectedDate.Hour, selectedDate.Minute, selectedDate.Second);
                endDatePicker.Date = selectedDate;
                endTimePicker.Time = new TimeSpan(selectedDate.Hour + 1, selectedDate.Minute, selectedDate.Second);
                startTimeZonePicker.SelectedIndex = 0;
                endTimeZonePicker.SelectedIndex = 0;
            }
        }

        #endregion

        #region UpdateElements

        internal void AddEditorElements()
        {
            if (Device.RuntimePlatform == "UWP" && Device.Idiom == TargetIdiom.Phone)
            {
                organizerLayout.IsVisible = false;

                startDateTimePickerLayout.Padding = new Thickness(20, 0, 20, 0);
                startDateTimePickerLayout.ColumnDefinitions.Clear();
                startDateTimePickerLayout.RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                };
                Grid.SetRow(startDatePickerLayout, 0);
                Grid.SetColumnSpan(startDatePickerLayout, 3);
                Grid.SetColumn(startTimePickerLayout, 0);
                Grid.SetColumnSpan(startTimePickerLayout, 3);
                Grid.SetRow(startTimePickerLayout, 1);
                startDatePickerLayout.HeightRequest = 40;
                startTimePickerLayout.HeightRequest = 40;
                startTimeLabelLayout.Padding = new Thickness(20, 5, 0, 0);
                startDateTimePickerLayout.Padding = new Thickness(20, 0, 20, 0);

                EndDateTimePickerLayout.ColumnDefinitions.Clear();
                EndDateTimePickerLayout.Padding = new Thickness(20, 0, 20, 0);
                EndDateTimePickerLayout.RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                };
                Grid.SetRow(endDatePickerLayout, 0);
                Grid.SetRow(endTimePickerLayout, 1);
                Grid.SetColumnSpan(endDatePickerLayout, 3);
                Grid.SetColumn(endTimePickerLayout, 0);
                Grid.SetColumnSpan(endTimePickerLayout, 3);
                endDatePickerLayout.HeightRequest = 40;
                endTimePickerLayout.HeightRequest = 40;
                endTimeLabelLayout.Padding = new Thickness(20, 5, 0, 0);
            }
            else if (Device.RuntimePlatform == "Android")
            {
                editorLayout.Padding = 20;
            }
        }

        #endregion

        #region Get TimeZone Index from TimeZoneCollection

        private int GetTimeZoneIndex(string timeZone)
        {
            for (int i = 0; i < ViewModels.TimeZoneCollection.TimeZoneList.Count; i++)
            {
                if (timeZone.Equals(ViewModels.TimeZoneCollection.TimeZoneList[i]))
                {
                    return i;
                }
            }
            return 0;
        }

        #endregion

        #region OnDetachingFrom
        protected override void OnDetachingFrom(StackLayout bindable)
        {
            saveButton.Clicked -= SaveButton_Clicked;
            cancelButton.Clicked -= CancelButton_Clicked;
            switchAllDay.Toggled -= SwitchAllDay_Toggled;

            startTimeZonePicker.SelectedIndexChanged -= StartTimeZonePicker_SelectedIndexChanged;
            endTimeZonePicker.SelectedIndexChanged -= EndTimeZonePicker_SelectedIndexChanged;

            organizerLayout = null;
            editorLayout = null;
            eventNameText = null;
            PostCodeText = null;
            contactText = null;
            descriptionText = null;
            cancelButton = null;
            saveButton = null;
            endDatePicker = null;
            endTimePicker = null;
            startDatePicker = null;
            startTimePicker = null;
            switchAllDay = null;

            done.Toggled -= done_Toggled;
            done = null;
            typeDatePicker = null;

            endTimeLabelLayout = null;
            startDateTimePickerLayout = null;
            startDatePickerLayout = null;

            startTimePickerLayout = null;
            startTimeLabelLayout = null;
            EndDateTimePickerLayout = null;

            endDatePickerLayout = null;
            EndDateTimePickerLayout = null;
            endTimePickerLayout = null;
        }
        #endregion
    }
}
