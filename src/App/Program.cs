using App;
using App.SoapService;
using Microsoft.Extensions.DependencyInjection;
using static App.SoapServiceConstants;

var services = new ServiceCollection();

services.AddHttpClient<SoapHttpClientBehavior>();

services.AddScoped(_ =>
{
    var soapLogger = new SoapLogger();
    var soapInspector = new SoapLoggingClientMessageInspector(soapLogger);
    var soapBehaviour = new SoapLoggingBehaviour(soapInspector);
    return soapBehaviour;
});

services.AddScoped(serviceProvider =>
{
    var client = new SoapServiceClient(Https.Configuration, Https.Address);
    var soapLoggingBehaviour = serviceProvider.GetService<SoapLoggingBehaviour>();
    var soapHttpClientBehaviour = serviceProvider.GetService<SoapHttpClientBehavior>();
    client.Endpoint.EndpointBehaviors.Add(soapLoggingBehaviour);
    client.Endpoint.EndpointBehaviors.Add(soapHttpClientBehaviour);
    return client;
});

var serviceProvider = services.BuildServiceProvider();

using var client = serviceProvider.GetService<SoapServiceClient>();

var request = new SoapContractsRequest
{
    X = RandomNumber(),
    Y = RandomNumber()
};

var response = await client.ComputeAsync(request);
Console.WriteLine($"{request.X} + {request.Y} = {response.Sum}");
Console.WriteLine($"{request.X} * {request.Y} = {response.Mul}");

Console.WriteLine("Press any key to exit program !");
Console.ReadKey();

static int RandomNumber() => Random.Shared.Next(1, 1000);
