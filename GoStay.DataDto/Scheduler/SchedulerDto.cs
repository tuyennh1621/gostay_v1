using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoStay.DataDto.Scheduler
{
    public class InputScheduler
    {
        public int IdRoom { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

    }
    public partial class SchedulerRoomPriceDto
    {
        public int PriceId { get; set; }
        public string Title { get; set; } = null!;
        public string Start { get; set; }
        public string End { get; set; }
        public DateTime DStart
        {
            get
            {
                DateTime dateTime = new DateTime();
                var check = DateTime.TryParseExact(End, "yyyy-MM-ddTHH:mm:ss", null, DateTimeStyles.None, out dateTime);
                if (!check)
                    dateTime = DateTime.ParseExact(End, "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);

                return dateTime;
            }
        }
        public DateTime DEnd
        {
            get
            {
                DateTime dateTime = new DateTime();
                var check = DateTime.TryParseExact(End, "yyyy-MM-ddTHH:mm:ss", null, DateTimeStyles.None, out dateTime);
                if (!check)
                    dateTime = DateTime.ParseExact(End, "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);

                return dateTime;
            }
        }
        public int RoomId { get; set; }
        public int OwnerID { get; set; }
        public string RecurrenceRule { get; set; } = null!;
        public bool? IsAllDay { get; set; }
        public string? Description { get; set; }
        public string? RecurrenceException { get; set; }
        public int? Attendees { get; set; }
        public int? RecurrenceId { get; set; }

        public string? EndTimezone { get; set; }
        public string? StartTimezone { get; set; }
    }
}
