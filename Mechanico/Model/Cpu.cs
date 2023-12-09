using System.Text.RegularExpressions;

public class Cpu
{
    public static readonly string Command = "uptime | awk -F'[a-z]:' '{ printf \"{\\n\\\"Current\\\": %s,\\n\\\"Last_1m\\\": %s,\\n\\\"Last_5m\\\": %s\\n}\", $2,$3,$4 }'";

    public double Current { get; set; }
    public double Last_1m { get; set; }
    public double Last_5m { get; set; }


    public static Cpu Parse(string output)
    {
        var match = Regex.Match(output, @"Current:\s*([\d.]+),\s*Last_1m:\s*([\d.]+),\s*Last_5m:\s*([\d.]+)");
        if (match.Success)
        {
            return new Cpu
            {
                Current = double.Parse(match.Groups[1].Value),
                Last_1m = double.Parse(match.Groups[2].Value),
                Last_5m = double.Parse(match.Groups[3].Value)
            };
        }

        // Handle parsing failure
        throw new ArgumentException($"Failed to parse Cpu output: {output}");
    }
}
