using GoStay.DataDto.Air.ExtDOMSearchFlightsShort;
using GoStay.DataDto.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace GoStay.DataDto.Air
{
    public class TicketOrder
    {
        public UserInfoDto UserInfo { get; set; }
        public string DataSession { get; set; }
        //Danh sách chuyến đi
        public ExtDOMFlightInfo Flights { get; set; }
        public DOMFareOption FareOption { get; set; }
        public string AirlineName { get; set; }

        public string StartPoint { get; set; }

        public string EndPoint { get; set; }


        public int Adult { get; set; }
        public int Child { get; set; }
        public int Infant { get; set; }
        public double TotalPrice
        {
            get
            {
                return (
                    (FareOption.PriceAdult + FareOption.FeeAdult + FareOption.TaxAdult + FareOption.ServiceFee+ FareOption.IssueFee)* Adult+
                    (FareOption.PriceChild + FareOption.FeeChild + FareOption.TaxChild + FareOption.ServiceFee + FareOption.IssueFee) * Child+
                    (FareOption.PriceInfant + FareOption.FeeInfant + FareOption.TaxInfant + FareOption.ServiceFee + FareOption.IssueFee) * Infant
                    );
            }
        }
        public int TotalPeople
        {
            get
            {
                return ( Adult +Child +Infant
                    );
            }
        }
    }
    public class ContactInfor
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

    }
    public class TicketHomePage
    {
        public string DataSession { get; set; }
        public int UserId { get; set; }
        public List<ExtDOMFlightInfo> data { get; set; }
    }
}
