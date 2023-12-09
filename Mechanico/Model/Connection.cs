
using System.Text.RegularExpressions;

public class Connection
{
    public static readonly string Command = "netstat -an | awk 'BEGIN {listen=0; estab=0;} /LISTEN/ {listen++} /ESTABLISHED/ {estab++} END {printf \"{\\\"Listen_ports\\\":%d,\\\"Connections_established\\\":%d}\\n\", listen, estab}'";

    public int ListenPorts { get; set; }
    public int ConnectionsEstablished { get; set; }

    public static Connection Parse(string output)
    {
        var match = Regex.Match(output, @"Listen_ports:(\d+),\s*Connections_established:(\d+)");
        if (match.Success)
        {
            return new Connection
            {
                ListenPorts = int.Parse(match.Groups[1].Value),
                ConnectionsEstablished = int.Parse(match.Groups[2].Value)
            };
        }

        // Handle parsing failure
        throw new ArgumentException($"Failed to parse Connection output: {output}");
    }
}