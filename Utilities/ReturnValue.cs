using System.Net;

namespace PuppyLearn.Utilities
{
    public class ReturnValue
    {
        public dynamic? Value { get; set; }
        public HttpStatusCode? HttpCode { get; set; }
        public string? Msg { get; set; }
    }
}
