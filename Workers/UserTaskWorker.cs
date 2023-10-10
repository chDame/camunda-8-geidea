using Newtonsoft.Json;
using System.Text.Json.Nodes;
using Zeebe.Client.Accelerator.Abstractions;
using Zeebe.Client.Accelerator.Attributes;
using Zeebe.Client.Api.Worker;

namespace tasklistDotNetReact.Services
{
  public class UserTaskWorker : IHostedService, IDisposable

  {
    private CancellationTokenSource cancellationTokenSource;
    private readonly ZeebeClientProvider zeebeClientProvider;
    private Zeebe.Client.Api.Worker.IJobWorker jobWorker;
    public UserTaskWorker(ZeebeClientProvider zeebeClientProvider)
    {
      this.zeebeClientProvider = zeebeClientProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      this.cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
      jobWorker = this.zeebeClientProvider.GetZeebeClient().NewWorker()
        .JobType("io.camunda.zeebe:userTask")
        .Handler((c, job) =>
        {
          Dictionary<string, string> headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(job.CustomHeaders);
          Console.WriteLine("User task : " + headers["io.camunda.zeebe:formKey"]);
        })
        .MaxJobsActive(1)
        .Name(Environment.MachineName)
        .PollInterval(TimeSpan.FromMilliseconds(100))
        .Timeout(TimeSpan.FromMinutes(1))
        .Open();

      return Task.CompletedTask;
    }

      public Task StopAsync(CancellationToken cancellationToken)
    {
      try
      {
        this.cancellationTokenSource.Cancel();
      }
      finally
      {
        StopInternal();
      }

      return Task.CompletedTask;
    }

    public void Dispose()
    {
      StopInternal();
    }
    public void StopInternal()
    {
      jobWorker.Dispose();
    }
  }
}

