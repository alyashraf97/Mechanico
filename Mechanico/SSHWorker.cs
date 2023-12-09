using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Renci.SshNet;

public class SSHWorker : BackgroundService
{
    private readonly ConcurrentQueue<Output> _aliveQueue;
    private readonly ConcurrentQueue<Output> _deadQueue;
    private readonly Configuration _config;

    public SSHWorker(Configuration config, ConcurrentQueue<Output> parsingQueue, ConcurrentQueue<Output> deadQueue)
    {
        _config = config;
        _aliveQueue = parsingQueue;
        _deadQueue = deadQueue;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<Job> jobs = new List<Job>();
            foreach (var host in _config.Hosts)
            {
                var newJob = new Job(host, _config);
                jobs.Add(newJob);
                _ = newJob.Run(stoppingToken);
            }

            int delay = 60000 - (int)stopwatch.ElapsedMilliseconds;
            if (delay > 0)
            {
                await Task.Delay(delay);
            }
        }
    }
}