using System.Text.RegularExpressions;

public class NetworkStats
{
    public static readonly string Command = "awk 'BEGIN {rx=0; tx=0;} {if ($2 ~ /^[0-9]/) {rx+=$2; tx+=$10}} END {printf \"{\\\"Total_RX\\\":%.2f,\\\"Total_Tx\\\":%.2f}\\n\", rx/1024^3, tx/1024^3}' /proc/net/dev";

    public double Total_RX { get; set; }
    public double Total_Tx { get; set; }

    public static NetworkStats Parse(string output)
    {
        var match = Regex.Match(output, @"Total_RX:([\d.]+),\s*Total_Tx:([\d.]+)");
        if (match.Success)
        {
            return new NetworkStats
            {
                Total_RX = double.Parse(match.Groups[1].Value),
                Total_Tx = double.Parse(match.Groups[2].Value)
            };
        }

        // Handle parsing failure
        throw new ArgumentException($"Failed to parse NetworkStats output: {output}");
    }
}