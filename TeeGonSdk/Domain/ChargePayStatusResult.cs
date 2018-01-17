namespace TeeGonSdk.Domain
{
    public class ChargePayStatusResult<T>
    {
        public string charge_id { get; set; }
        public string order_no { get; set; }
        public string pay_channel { get; set; }
        public string trade_status { get; set; }

    }
}
