using System.Text.RegularExpressions;

public class Disk
{
    public static readonly string Command = "df -BG | awk 'BEGIN { disks=0; total=0; used=0; free=0; } /\\/$/ { root_total=$2; root_used=$3; root_free=$4; } { disks++; total+=$2; used+=$3; free+=$4; } END { printf \"{\\n\\\"Disks\\\":%d, \\n\\\"Total_storage\\\": %d, \\n\\\"Total_used\\\":%d, \\n\\\"Total_free\\\":%d, \\n\\\"Root_mount\\\":%d, \\n\\\"Used_root\\\":%d, \\n\\\"Free_root\\\":%d\\n}\", disks-1, total, used, free, root_total, root_used, root_free; }'";

    public int Disks { get; set; }
    public long TotalStorage { get; set; }
    public long TotalUsed { get; set; }
    public long TotalFree { get; set; }
    public long RootMount { get; set; }
    public long UsedRoot { get; set; }
    public long FreeRoot { get; set; }

    public static Disk Parse(string output)
    {
        var match = Regex.Match(output, @"Disks:(\d+),\s*Total_storage:(\d+),\s*Total_used:(\d+),\s*Total_free:(\d+),\s*Root_mount:(\d+),\s*Used_root:(\d+),\s*Free_root:(\d+)");
        if (match.Success)
        {
            return new Disk
            {
                Disks = int.Parse(match.Groups[1].Value),
                TotalStorage = long.Parse(match.Groups[2].Value),
                TotalUsed = long.Parse(match.Groups[3].Value),
                TotalFree = long.Parse(match.Groups[4].Value),
                RootMount = long.Parse(match.Groups[5].Value),
                UsedRoot = long.Parse(match.Groups[6].Value),
                FreeRoot = long.Parse(match.Groups[7].Value)
            };
        }

        // Handle parsing failure
        throw new ArgumentException($"Failed to parse Disk output: {output}");
    }
}