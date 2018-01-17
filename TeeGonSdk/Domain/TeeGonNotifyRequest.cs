namespace TeeGonSdk.Domain
{
    public class TeeGonNotifyRequest
    {
        public decimal Amount { get; set; }
        public string Bank { get; set; }
        public string Buyer { get; set; }
        public string Channel { get; set; }
        public string Charge_Id { get; set; }
        public string Device_Info { get; set; }
        public bool Is_Success { get; set; }
        public string Metadata { get; set; }
        public string Order_No { get; set; }
        public long Pay_Time { get; set; }
        public decimal Real_Amount { get; set; }
        public string Sign { get; set; }
        public string Status { get; set; }
        public long Timestamp { get; set; }
    }
}
