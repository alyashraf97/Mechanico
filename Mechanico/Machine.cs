using System.Net;

public class Machine
{
    public string Name { get; set; }
    public string IpAddress { get; set; }

    public Machine() { }

    public Machine(string name)
    {
        IpAddress = Dns.GetHostAddresses(name)[0].ToString() ?? "";
        Name = name;
    }

    public Machine(string name, string ipAddress)
    {
        IpAddress = ipAddress;
        Name = name;
    }
}
