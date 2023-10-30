

using GoStay.DataDto.Air;
using GoStay.DataDto.Air.DOMAirPrice;
using GoStay.DataDto.Air.DOMGetBaggages;
using GoStay.DataDto.Air.DOMMakeMultiReservation;
using GoStay.DataDto.Air.DOMSearchFlights;
using GoStay.DataDto.Air.ExtDOMSearchFlights;
using GoStay.DataDto.Air.INTGetFareRuleInfo;
using GoStay.DataDto.Air.INTSearchFlights;
using GoStay.DataDto.Air.IssueTicket;
using GoStay.DataDto.Air.OpenPNR;
using GoStay.DataDto.Air.RequestIssueTicket;
using GoStay.DataDto.Air.RequestSupport;
using GoStay.DataDto.Air.UpdatePaymentInfo;

namespace GoStay.Service.Api.Air
{
    public interface IAirApiServices
    {
        public ResultData<DOMFlightData> DOMSearchFlights(RequestData<DataDto.Air.DOMSearchFlights.FlightParams> request);
        public ResultData<ExtDOMFlightData> ExtDOMSearchFlightsFull(RequestData<DataDto.Air.ExtDOMSearchFlights.FlightParams> request);
        public ResultData<DataDto.Air.ExtDOMSearchFlightsShort.ExtDOMFlightData> ExtDOMSearchFlights(RequestData<DataDto.Air.ExtDOMSearchFlights.FlightParams> request);

        public ResultData<BaggageData> DOMGetBaggages(RequestData<BaggageParams> request);
        public ResultData<DataDto.Air.DOMMakeReservation.ReservationCode> DOMMakeReservation(RequestData<DataDto.Air.DOMMakeReservation.ReservationParams> request);
        public ResultData<DataDto.Air.DOMMakeReservation.ReservationCode> DOMMakeMultiReservation(RequestData<MultiReservationParams> request);
        public ResultData<INTFlightData> INTSearchFlights(RequestData<DataDto.Air.INTSearchFlights.FlightParams> request);
        public ResultData<DataDto.Air.DOMMakeReservation.ReservationCode> INTMakeReservation(RequestData<DataDto.Air.INTMakeReservation.ReservationParams> request);
        public ResultData<string> ReleaseDataSession(RequestData<string> request);
        public ResultData<List<string>> DOMGetFareRuleInfo(RequestData<DataDto.Air.DOMGetFareRuleInfo.FareRuleParams> request);
        public ResultData<List<INTRules>> INTGetFareRuleInfo(RequestData<DataDto.Air.INTGetFareRuleInfo.FareRuleParams> request);
        public ResultData<PNRPriceQuoteResponse> DOMAirPrice(RequestData<DataDto.Air.DOMAirPrice.PNRParams> request);
        public ResultData<string> IssueTicket(RequestData<IssueTicketParams> request);
        public ResultData<PNRResponse> OpenPNR(RequestData<DataDto.Air.OpenPNR.PNRParams> request);
        public ResultData<string> UpdatePaymentInfo(RequestData<PaymentInfoParams> request);
        public ResultData<string> RequestIssueTicket(RequestData<RequestIssueParams> request);

        public ResultData<string> RequestSupport(RequestData<SupportRequestParams> request);
        public ResultData<DataDto.Air.DOMIssueReservation.ReservationCode>
                        DOMIssueReservation(RequestData<DataDto.Air.DOMIssueReservation.ReservationParams> request);

    }
}
