using TeeGonSdk.Domain;

namespace TeeGonSdk.Response
{
    
    public class ChargePayStatusResponse<T> : TopResponse
    {
        public ChargePayStatusResult<T> Result { get; set; }
    }
}
