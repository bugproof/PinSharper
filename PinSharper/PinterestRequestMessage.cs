using System;
using System.Net.Http;

namespace PinSharper
{
    class PinterestRequestMessage : HttpRequestMessage
    {
        public bool IsSigned { get; set; }

        public PinterestRequestMessage()
        {
            IsSigned = false;
        }

        public PinterestRequestMessage(HttpMethod method, string requestUri, bool isSigned = false) : base(method, requestUri)
        {
            IsSigned = isSigned;
        }

        public PinterestRequestMessage(HttpMethod method, Uri requestUri, bool isSigned = false) : base(method, requestUri)
        {
            IsSigned = isSigned;
        }
    }
}
