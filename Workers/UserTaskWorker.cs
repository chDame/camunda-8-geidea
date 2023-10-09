using System.Text.Json.Nodes;
using Zeebe.Client.Accelerator.Abstractions;
using Zeebe.Client.Accelerator.Attributes;

namespace tasklistDotNetReact.Services
{
  [JobType("io.camunda.zeebe:userTask")]
  [Timeout(2592000000)]
  public class UserTaskWorker : IAsyncZeebeWorker

  {
    public UserTaskWorker()
    {
    }

    public async Task HandleJob(ZeebeJob job, CancellationToken cancellationToken)
    {
      Dictionary<string, string> headers = job.getCustomHeaders<Dictionary<string, string>>();
      Console.WriteLine("User task : " + headers["io.camunda.zeebe:formKey"]);
    }
  }
}

