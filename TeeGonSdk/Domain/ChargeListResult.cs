using System.Collections.Generic;

namespace TeeGonSdk.Domain
{
    public class ChargeListResult
    {
        public List<Charge> Items { get; set; }
        public int Count { get; set; }
    }
}
