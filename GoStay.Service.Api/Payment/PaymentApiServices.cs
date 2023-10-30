using GoStay.DataDto.Statistical;
using GoStay.Service.Api.Base;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web;
using GoStay.DataAccess.Entities;
using System.Runtime.InteropServices;
using GoStay.Common.Configuration;
using GoStay.Common.Helper.PaymentHelper;

namespace GoStay.Service.Api.Payment
{
    public class PaymentApiServices : ApiServiceBase, IPaymentApiServices
    {
        public JObject MomoPay(string total, string domain, string urlReturn, string orderCode, string idOrder)
        {
            //string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            orderCode = orderCode.Replace(",", "-");
            string endpoint = AppConfigs.MoMoEndPoint;
            string partnerCode = AppConfigs.MoMoPartnerCode;
            string accessKey = AppConfigs.MoMoAccessKey;
            string serectkey = AppConfigs.MoMoSerectKey;
            string orderInfo = "Đơn hàng: " + orderCode;
            string ipnUrl = AppConfigs.MoMoIpnUrl;
            string redirectUrl = domain + urlReturn + "?idOrder=" + idOrder;
            string notifyurl = AppConfigs.MoMoNotifyurl; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test


            long amount = (long)Convert.ToDouble(total);
            string lang = AppConfigs.MoMoLang;
            string orderid = DateTime.Now.Ticks.ToString() +"-"+ orderCode; //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";
            string requestType = "captureWallet";

            string rawHash =  "accessKey=" +
                                accessKey + "&amount=" +
                                amount + "&extraData=" +
                                extraData + "&ipnUrl=" +
                                ipnUrl + "&orderId=" +
                                orderid + "&orderInfo=" +
                                orderInfo + "&partnerCode=" +
                                partnerCode + "&redirectUrl=" +
                                redirectUrl + "&requestId=" +
                                requestId + "&requestType=" +
                                requestType;


            SecurityHelper crypto = new SecurityHelper();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                //{ "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "requestType", requestType },
                { "extraData", extraData },
                { "lang", lang },
                { "signature", signature }
                
                
                //{ "item", items },

            };

            string responseFromMomo = PaymentRequestHelper.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);
            return jmessage;
        }
        public string VNPay(string total, string user, string domain, string urlReturn, string orderCode, string idOrder)
        {
            string url = AppConfigs.VNPPayment;
            string returnUrl = domain + urlReturn + "?idOrder=" + idOrder;
            string tmnCode = AppConfigs.VNPCode;
            string hashSecret = AppConfigs.VNPSerectKey;
            PayLibHelper pay = new PayLibHelper();
            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", total); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", user); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Đơn hàng: " + orderCode); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString() + "-" + orderCode); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return paymentUrl;
        }

        public JObject Appota(string total, string domain, string urlReturn, string orderCode, string idOrder)
        {
            string endpoint = AppConfigs.AppotaEndPoint;
            string partnerCode = AppConfigs.AppotaIssuer;
            string apiKey = AppConfigs.AppotaKey;
            string serectkey = AppConfigs.AppotaKey;
            string orderInfo = "Đơn hàng: " + orderCode;
            string clientIp = AppConfigs.AppotaClienIp;
            string redirectUrl = domain + urlReturn + "?idOrder=" + idOrder;
            string notifyurl = "https://gostay.realtech.com.vn/order/successful"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            //int amount = 50000;
            long amount = (long)Convert.ToDouble(total);
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "amount=" + amount +
                "&clientIp=" + clientIp +
                "&extraData=" + extraData +
                "&notifyUrl=" + notifyurl + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&redirectUrl=" + redirectUrl;

            SecurityHelper crypto = new SecurityHelper();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "amount", amount },
                { "clientIp", clientIp },
                { "extraData", extraData },
                { "notifyUrl", notifyurl },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                 { "signature", signature }

            };
            var tokenJwt = crypto.GenerateToken();
            string responseFromAppota = PaymentRequestHelper.sendPaymentRequest(endpoint, message.ToString(), tokenJwt);

            var jmessage = JObject.Parse(responseFromAppota);
            return jmessage;
        }
    }
}
