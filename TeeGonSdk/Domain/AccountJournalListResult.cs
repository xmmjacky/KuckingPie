using System.Collections.Generic;

namespace TeeGonSdk.Domain
{
    public class AccountJournalListResult
    {
        public List<AccountJournal> Items { get; set; }
        public int Count { get; set; }
    }
}
