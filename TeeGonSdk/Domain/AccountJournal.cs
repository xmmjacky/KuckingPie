namespace TeeGonSdk.Domain
{
    public class AccountJournal
    {
        public string Account_Alias { get; set; }
        public string Account_Id { get; set; }
        public string Buyer_Account { get; set; }
        public string Channel { get; set; }
        public string Charge_Id { get; set; }
        public long Created { get; set; }
        public string Id { get; set; }
        public decimal Income { get; set; }
        public long Journal_No { get; set; }
        public int Journal_Type { get; set; }
        public string Message { get; set; }
        public string Order_No { get; set; }
        public decimal Outcome { get; set; }
        public string Relation_Account_Alias { get; set; }
        public string Relation_Account_Id { get; set; }
        public string Trade_Msg { get; set; }
        public string Transaction_No { get; set; }
    }
}
