namespace Api;

public class SoapContracts
{
    [DataContract]
    public class Request
    {
        [DataMember]
        public int X { get; set; }

        [DataMember]
        public int Y { get; set; }
    }
    
    [DataContract]
    public class Response
    {
        [DataMember]
        public int Sum { get; set; }
        
        [DataMember]
        public int Mul { get; set; }
    }
}