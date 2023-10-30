namespace GoStay.Web.Model
{
    public class ResultPayment
    {
        public string partnerCode { get; set; }
        public string accessKey { get; set; }
        public string requestId { get; set; }
        public string amount { get; set; }
        public string orderId { get; set; }
        public string orderInfo { get; set; }
        public string orderType { get; set; }
        public string transId { get; set; }
        public string message { get; set; }
        public string localMessage { get; set; }
        public string responseTime { get; set; }
        public string errorCode { get; set; }
        public string payType { get; set; }
        public string extraData { get; set; }
        public string signature { get; set; }
        public int resultCode { get; set; }
        public int idOrder { get; set; }

        //vnpay
        public string vnp_TmnCode { get; set; }
        public int vnp_Amount { get; set; }
        public string vnp_BankCode { get; set; }
        public string vnp_OrderInfo { get; set; }
        public int vnp_TransactionNo { get; set; }
        public int vnp_ResponseCode { get; set; }
        public int vnp_TransactionStatus { get; set; }
        public string vnp_TxnRef { get; set; }
        public string vnp_SecureHash { get; set; }
    }
}
