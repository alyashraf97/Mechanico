using System.Text.RegularExpressions;

public class Memory
{
    public static readonly string Command = "free -b | awk '/Mem|Swap/ { printf \"\\\"%s_total\\\":%s, \\\"%s_used\\\":%s, \", $1, $2, $1, $3 }' | awk '{ print \"{\\n\"substr($0, 1, length($0)-2)\"\\n}\" }'";

    public long Total { get; set; }
    public long Used { get; set; }
    public long TotalSwap { get; set; }
    public long UsedSwap { get; set; }

    public static Memory Parse(string output)
    {
        var match = Regex.Match(output, @"(\w+)_total:(\d+),\s*(\w+)_used:(\d+)");
        if (match.Success)
        {
            return new Memory
            {
                Total = long.Parse(match.Groups[2].Value),
                Used = long.Parse(match.Groups[4].Value)
            };
        }

        // Handle parsing failure
        throw new ArgumentException($"Failed to parse Memory output: {output}");
    }
}