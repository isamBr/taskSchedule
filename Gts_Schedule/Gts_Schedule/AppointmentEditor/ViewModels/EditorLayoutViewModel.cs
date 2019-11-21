using Gts_Schedule.AppointmentEditor.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gts_Schedule.AppointmentEditor.ViewModels
{
   
    internal class EditorLayoutViewModel
    {
        public virtual void OnAppointmentModified(ScheduleAppointmentModifiedEventArgs e)
        {
            EventHandler<ScheduleAppointmentModifiedEventArgs> handler = AppointmentModified;
            if (handler != null)
            {
                handler(this, e);
            }

        }
        public event EventHandler<ScheduleAppointmentModifiedEventArgs> AppointmentModified;
    }

    public class ScheduleAppointmentModifiedEventArgs : EventArgs
    {
        public Meeting Appointment { get; set; }
        public bool IsModified { get; set; }
    }
}
