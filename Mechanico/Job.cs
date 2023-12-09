using Renci.SshNet;
using System.Collections.Concurrent;

public class Job
{
    private SshClient? _sshClient;
    public static ConcurrentQueue<Output> Outputs = new();

    public Job(Machine host, Configuration config)
    {
        _sshClient = new SshClient(host.IpAddress ?? host.Name, config.Username, config.PrivateKey);
    }

    public async Task Run(CancellationToken cancellationToken)
    {
        try
        {
            if (!_sshClient.IsConnected)
            {
                await _sshClient.ConnectAsync(cancellationToken);
            }

            var cpuResult = _sshClient.RunCommand(Cpu.Command);
            var memResult = _sshClient.RunCommand(Memory.Command);
            var diskResult = _sshClient.RunCommand(Disk.Command);
            var connResult = _sshClient.RunCommand(Connection.Command);
            var netResult = _sshClient.RunCommand(NetworkStats.Command);

            // Check for errors in the command results
            if (!string.IsNullOrEmpty(cpuResult.Error) || !string.IsNullOrEmpty(memResult.Error) ||
                !string.IsNullOrEmpty(diskResult.Error) || !string.IsNullOrEmpty(connResult.Error) ||
                !string.IsNullOrEmpty(netResult.Error))
            {
                // Handle command errors
            }

            var cpu = Cpu.Parse(cpuResult.Result);
            var mem = Memory.Parse(memResult.Result);
            var disk = Disk.Parse(diskResult.Result);
            var conn = Connection.Parse(connResult.Result);
            var net = NetworkStats.Parse(netResult.Result);

            Outputs.Enqueue(new Output { Cpu = cpu, Memory = mem, Disk = disk, Connection = conn, NetworkStats = net });
        }
        catch (Exception ex)
        {
            // Handle SSH connection and command execution errors
        }
    }
}