using TeeGonSdk.Domain;

namespace TeeGonSdk.Response
{
    public class ChargeResponse<T> : TopResponse
    {
        public ChargeResult<T> Result { get; set; }
    }
}
