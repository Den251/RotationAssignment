using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RotationAssignment.Models
{
    public class TimeStamp : IComparable<TimeStamp>
    {
        public string Id { get; set; }
        public DateTime? Time { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        //public int CompareTo(object obj)
        //{
        //    TimeStamp timeStamp = obj as TimeStamp;
        //    if (this.Time.HasValue && timeStamp.Time.HasValue)
        //    {
        //        var thisTime = (DateTime)this.Time;
        //        var timeStampTime = (DateTime)timeStamp.Time;
        //        return thisTime.CompareTo(timeStampTime);
        //    }
        //    return 0;
        //
        //}

        public int CompareTo(TimeStamp obj)
        {
            if (this.Time.HasValue && obj.Time.HasValue)
            {
                var thisTime = (DateTime)this.Time;
                var timeStampTime = (DateTime)obj.Time;
                return thisTime.CompareTo(timeStampTime);
            }
            return 0;
        }
    }
}
