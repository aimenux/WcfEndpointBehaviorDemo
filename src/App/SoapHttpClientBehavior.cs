using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace App;

public class SoapHttpClientBehavior : IEndpointBehavior
{
    private readonly HttpClient _client;

    public SoapHttpClientBehavior(HttpClient client)
    {
        _client = client;
    }

    public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
    {
        bindingParameters.Add(new Func<HttpClientHandler, HttpMessageHandler>(GetHttpMessageHandler));
    }

    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    {
    }

    public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
    {
    }

    public void Validate(ServiceEndpoint endpoint)
    {
    }
    
    private HttpMessageHandler GetHttpMessageHandler(HttpClientHandler httpClientHandler)
    {
        return new SoapHttpMessageHandler(httpClientHandler, _client);
    }
    
    private class SoapHttpMessageHandler : DelegatingHandler
    {
        private readonly HttpClient _client;

        public SoapHttpMessageHandler(HttpMessageHandler innerHandler, HttpClient client)
        {
            InnerHandler = innerHandler;
            _client = client;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await _client.PostAsync(request.RequestUri, request.Content, cancellationToken);
            return response;
        }
    }
}