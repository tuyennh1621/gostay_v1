using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GoStay.Service.Api.Payment
{
    public interface IPaymentApiServices
    {
        public JObject MomoPay(string total, string domain, string urlReturn, string orderCode, string idOrder);
        public string VNPay(string total, string user, string domain, string urlReturn, string orderCode, string idOrder);
        public JObject Appota(string total, string domain, string urlReturn, string orderCode, string idOrder);
    }
}
