using GoStay.DataAccess.Entities;
using Kendo.Mvc.UI;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GoStay.Web.Areas.Admin.Model
{
    public class Customer
    {
        public int OrderId { get; set; }

        
        public string FullName { get; set; }
        public string Messenge { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Time { get; set; }
    }

    public class SchedulerRoomPriceViewModel : ISchedulerEvent
    {
        public int PriceId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        private DateTime start;

        [Required]
        public DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value.ToUniversalTime();
            }
        }

       

        private DateTime end;

        [Required]
        [DateGreaterThan(OtherField = "Start")]
        public DateTime End
        {
            get
            {
                return end;
            }
            set
            {
                end = value.ToUniversalTime();
            }
        }

        public string EndTimezone { get; set; }
        public string StartTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public int? RecurrenceId { get; set; }
        public string RecurrenceException { get; set; }
        public bool IsAllDay { get; set; }
        public string Timezone { get; set; }
        public int RoomId { get; set; }
        public IEnumerable<int> Attendees { get; set; }

        public SchedulerRoomPrice ToEntity()
        {
            var meeting = new SchedulerRoomPrice
            {
                PriceId = PriceId,
                Title = Title,
                Start = Start,
                StartTimezone = StartTimezone,
                End = End,
                EndTimezone = EndTimezone,
                Description = Description,
                IsAllDay = IsAllDay,
                RecurrenceRule = RecurrenceRule,
                RecurrenceException = RecurrenceException,
                RecurrenceId = RecurrenceId,
                RoomId = 100
            };

            return meeting;
        }
    }
 }
