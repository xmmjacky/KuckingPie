namespace TeeGonSdk.Domain
{
    public class ChargeAction<T>
    {
        public string Type { get; set; }
        public string Url { get; set; }
        public T Params { get; set; }
    }
}
