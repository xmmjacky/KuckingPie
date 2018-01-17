namespace TeeGonSdk.Domain
{
    public class Withdrawal
    {
        public long Created { get; set; }
        public string Front_Log_No { get; set; }
        public decimal Tran_Amount { get; set; }
        public string Wrs_error { get; set; }
        public string Wrs_result { get; set; }
    }
}
