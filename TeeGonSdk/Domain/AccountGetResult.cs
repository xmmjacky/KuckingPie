namespace TeeGonSdk.Domain
{
    public class AccountGetResult
    {
        public string Id { get; set; }
        public string Account_Id { get; set; }
        public string Domain_Id { get; set; }
        public string Mobile { get; set; }
        public long Created { get; set; }
        public long Bank_Create_Time { get; set; }
        public string Alias { get; set; }
        public long Cust_Acct_Id { get; set; }
    }
}
