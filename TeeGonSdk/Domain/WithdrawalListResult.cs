using System.Collections.Generic;

namespace TeeGonSdk.Domain
{
    public class WithdrawalListResult
    {
        public List<Withdrawal> Items { get; set; }
        public int Count { get; set; } 
    }
}
