using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Gts_Schedule.AppointmentEditor.Model
{
    [Preserve(AllMembers = true)]
    public class Meeting
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string EventName { get; set; }
        public string PostCode { get; set; }
        public string ContactID { get; set; }
        public string Description { get; set; }
        //public DateTime From { get; set; }

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
        private Color _color = Color.Red;
        public Color color
        {
            get
            {
                return _color;
            }
            set
            {
                if (type != null)
                {
                    if (type.ToLower() == "Job".ToLower())
                        _color = Color.Red;
                    else if (type.ToLower() == "Sales".ToLower())
                        _color = Color.Blue;
                    else if (type.ToLower() == "Maintenance".ToLower())
                        _color = Color.Brown;
                    else
                        _color = Color.Orange;
                }
                else
                    _color = Color.Orange;

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
    public class RootObject
    {
        public List<Meeting> Meetings { get; set; }
    }
}

