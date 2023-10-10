using System.Text.Json.Nodes;
using Zeebe.Client.Accelerator.Abstractions;
using Zeebe.Client.Accelerator.Attributes;

namespace tasklistDotNetReact.Services
{
  [JobType("sendEmail")]
  public class MailWorker : IAsyncZeebeWorkerWithResult<JsonNode>

  {
    private readonly MailService mailService;
    public MailWorker(MailService mailService)
    {
      this.mailService = mailService;
    }
    public async Task<JsonNode> HandleJob(ZeebeJob job, CancellationToken cancellationToken)
    {
      // get variables as declared (SimpleJobPayload)
      JsonNode variables = job.getVariables<JsonNode>();

      variables["mailId"] = await mailService.sendMailAsync(variables["usermail"]!.ToString(), variables["subject"]!.ToString(), variables["template"]!.ToString(), "EN", variables);
      // execute business service etc.
      // var result = await _myApiService.DoSomethingAsync(variables.CustomerNo, cancellationToken);
      return variables;
    }
  }
}

