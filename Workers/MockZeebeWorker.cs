using System.Text.Json.Nodes;
using Zeebe.Client.Accelerator.Abstractions;
using Zeebe.Client.Accelerator.Attributes;

namespace tasklistDotNetReact.Services
{
  [JobType("checkIfPhoneIsValidWithId")]
  public class PhoneIdValidWorker : IAsyncZeebeWorkerWithResult<JsonNode>
  {
    public PhoneIdValidWorker()
    {
    }
    public async Task<JsonNode> HandleJob(ZeebeJob job, CancellationToken cancellationToken)
    {
      // get variables as declared (SimpleJobPayload)
      JsonNode variables = job.getVariables<JsonNode>();
      variables["phoneIdValid"] = true;
      return variables;
    }
  }

  
  [JobType("sendOTP")]
  public class SendOTPWorker : IAsyncZeebeWorkerWithResult<JsonNode>
  {
    public SendOTPWorker()
    {
    }
    public async Task<JsonNode> HandleJob(ZeebeJob job, CancellationToken cancellationToken)
    {
      return null;
    }
  }
  

  [JobType("createMmsTaskForSale")]
  public class CreateMmsTaskForSaleWorker : IAsyncZeebeWorkerWithResult<JsonNode>
  {
    public CreateMmsTaskForSaleWorker()
    {
    }
    public async Task<JsonNode> HandleJob(ZeebeJob job, CancellationToken cancellationToken)
    {
      return null;
    }
  }
}

