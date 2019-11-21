using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gts_Schedule.AppointmentEditor.Model
{
    public class Todo
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string EventName { get; set; }
        public string PostCode { get; set; }
        public string ContactID { get; set; }
        public string Description { get; set; }

        public DateTime FromDate;
        public DateTime From
        {
            get
            {
                return FromDate;
            }
            set
            {

                FromDate = value;

            }
        }

        public DateTime ToDate;
        public DateTime To
        {
            get
            {
                return ToDate;
            }
            set
            {

                ToDate = value;

            }
        }
        public double MinimumHeight { get; set; }
        public bool IsAllDay { get; set; }
        public string StartTimeZone { get; set; }
        public string EndTimeZone { get; set; }
        public bool Done { get; set; }
        public string type { get; set; }
        public string responsible { get; set; }
    }
}
