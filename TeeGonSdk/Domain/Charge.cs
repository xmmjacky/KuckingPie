namespace TeeGonSdk.Domain
{
    public class Charge
    {
        public decimal Amount { get; set; }
        public string Channel { get; set; }
        public long Created { get; set; }
        public string Order_No { get; set; }
        public int Paid { get; set; }
        public string Subject { get; set; }
        public long Time_Paid { get; set; }
        public long Updated { get; set; }
    }
}
