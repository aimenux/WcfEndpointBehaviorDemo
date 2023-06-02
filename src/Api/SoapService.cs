namespace Api;

[ServiceContract]
public interface ISoapService
{
    [OperationContract]
    SoapContracts.Response Compute(SoapContracts.Request request);
}
    
public class SoapService : ISoapService
{
    public string GetHelloMessage()
    {
        return $"Hello {Guid.NewGuid():N}";
    }

    public SoapContracts.Response Compute(SoapContracts.Request request)
    {
        var sum = request.X + request.Y;
        var mul = request.X * request.Y;
        return new SoapContracts.Response
        {
            Sum = sum,
            Mul = mul
        };
    }
}