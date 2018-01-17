namespace TeeGonSdk.Domain
{
    public class ChargeResult<T>
    {
        public string Id { get; set; }
        public string OrderNo { get; set; }
        public string Channel { get; set; }
        public decimal Amount { get; set; }
        public decimal Real_amount { get; set; }
        public string Client_Ip { get; set; }
        public string Currency { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int Time_Expire { get; set; }
        public string Return_Url { get; set; }
        public string Notify_Url { get; set; }
        public string Device_Id { get; set; }
        public string Charge_Type { get; set; }
        public string Account_Id { get; set; }
        public string Buyer_Account { get; set; }
        public string Seller_Account { get; set; }
        public string Description { get; set; }
        public bool Paid { get; set; }
        public bool Livemode { get; set; }
        public bool Refunded { get; set; }
        public int AmountSettle { get; set; }
        public int Time_Paid { get; set; }
        public int Time_Settle { get; set; }
        public long Created { get; set; }
        public long Updated { get; set; }
        public string Transaction_No { get; set; }
        public decimal Amount_Refunded { get; set; }
        public string Failure_Code { get; set; }
        public string Failure_Msg { get; set; }
        public string Metadata { get; set; }
        public string Wx_Openid { get; set; }
        public ChargeAction<T> Action { get; set; }
        public string Profit_Error { get; set; }
        public long Profit_Apply_Time { get; set; }
        public string Profit_Result { get; set; }
        public decimal Pay_Rate { get; set; }
        public decimal Subsidy_Rate { get; set; }
        public int Manual_Journal { get; set; }
    }
}
