using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PinSharper
{
    class PinterestHttpHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var pinRequest = request as PinterestRequestMessage;
            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
            var queryNew = new HttpUtility.HttpQSCollection();
            queryNew["client_id"] = PinterestConstants.CLIENT_ID;
            queryNew["timestamp"] = PinterestHelpers.GenerateTimestamp();
            if (pinRequest.IsSigned)
            {
                var dataContent = new NameValueCollection();
                dataContent.Add(queryNew);
                dataContent.Add(query);
                if (request.Content != null)
                {
                    var postContent = HttpUtility.ParseQueryString(await request.Content.ReadAsStringAsync());
                    dataContent.Add(postContent);
                }
                var sortedContent = new HttpUtility.HttpQSCollection();
                foreach (var item in dataContent.AllKeys.OrderBy(k => k))
                    sortedContent.Add(item, dataContent[item]);
                queryNew["oauth_signature"] = PinterestHelpers.GenerateSignature(
                    request.Method.ToString(), 
                    request.RequestUri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Path, UriFormat.UriEscaped),
                    sortedContent.ToString());
            }
            queryNew.Add(query);
            var uriBuilder = new UriBuilder(request.RequestUri);
            uriBuilder.Query = queryNew.ToString();
            request.RequestUri = uriBuilder.Uri;
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
