public class SSHWorker : BackgroundService
{
    private readonly Configuration _config;
    Dictionary<Machine, Job> _jobs;

    public SSHWorker(Configuration config, Dictionary<Machine, Job> jobs)
    {
        _config = config;
        _jobs = jobs;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Create a new CancellationTokenSource that cancels after one minute
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken))
            {
                cts.CancelAfter(TimeSpan.FromMinutes(1));
                try
                {
                    await RunJobs(cts.Token);
                }
                catch (OperationCanceledException)
                {
                    // Handle the tasks being cancelled if they take longer than a minute
                    // You might want to log this event or handle it in some other way
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                }
            }
            await Task.Delay(60000, stoppingToken); // Wait for one minute before starting the next cycle
        }
    }

    private async Task RunJobs(CancellationToken cancellationToken)
    {
        List<Task> tasks = new();
        foreach (var job in _jobs)
        {
            tasks.Add(job.Value.Run(cancellationToken));
        }
        try
        {
            await Task.WhenAll(tasks);
        }
        catch (OperationCanceledException)
        {
            // Handle the tasks being cancelled if they take longer than a minute
            // You might want to log this event or handle it in some other way
        }
    }
}