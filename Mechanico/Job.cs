using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Job
{
    private SshClient? _sshClient;
    public Output? Output { get; private set; }

    public Job(Machine host, Configuration config)
    {
        _sshClient = new SshClient(host.IpAddress ?? host.Name, config.Username, config.PrivateKey);
    }

    public async Task Run(CancellationToken cancellationToken)
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

        var cpu = Cpu.Parse(cpuResult.Result);
        var mem = Memory.Parse(memResult.Result);
        var disk = Disk.Parse(diskResult.Result);
        var conn = Connection.Parse(connResult.Result);
        var net = NetworkStats.Parse(netResult.Result);

        Output = new Output { Cpu = cpu, Memory = mem, Disk = disk, Connection = conn, NetworkStats = net };
    }
 }
