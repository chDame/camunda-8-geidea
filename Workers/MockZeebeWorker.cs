using System.Text.Json.Nodes;
using tasklistDotNetReact.Dtos.Models;
using Zeebe.Client.Accelerator.Abstractions;
using Zeebe.Client.Accelerator.Attributes;

namespace tasklistDotNetReact.Services
{
  [JobType("checkIfPhoneIsValidWithId")]
  public class PhoneIdValidWorker : IAsyncZeebeWorkerWithResult<JsonNode>
  {
    private readonly IDueDilligenceService dueDilligenceService;
    private readonly ZeebeClientProvider _zeebeClientProvider;

    public PhoneIdValidWorker(IDueDilligenceService dueDilligenceService, ZeebeClientProvider zeebeClientProvider)
    {
      this.dueDilligenceService = dueDilligenceService;
      _zeebeClientProvider = zeebeClientProvider;
    }
    public async Task<JsonNode> HandleJob(ZeebeJob job, CancellationToken cancellationToken)
    {
      // get variables as declared (SimpleJobPayload)
      JsonNode variables = job.getVariables<JsonNode>();

      if (variables["phoneNumber"]?.ToString() == "451123333")
      {
        variables["phoneIdValid"] = false;

        return variables;
      }

      try
      {
        var lead = await dueDilligenceService.LeadCreate_ELMCheck(new LeadValidationRequest()
        {
          NationalId = variables["nationalId"]!.ToString(),
          CountryPrefix = "+966",
          PhoneNumber = variables["phoneNumber"]!.ToString()
        });

        variables["phoneIdValid"] = (lead is not null && lead.ElmCheck.HasValue && lead.ElmCheck.Value);


        return variables;
      }
      catch (Exception e)
      {
        await this._zeebeClientProvider.GetZeebeClient().NewThrowErrorCommand(job.Key).ErrorCode("ErrorValidation").ErrorMessage(e.Message).Send();

        /*await this._zeebeClientProvider.GetZeebeClient().NewPublishMessageCommand()
          .MessageName("ErrorValidation").CorrelationKey(variables["correlationId"].ToString())
          .MessageId("ErrorValidation")
          .Variables("{ \"ExceptionValidation\":  \""+e.Message+"\" }").Send();*/
        return null;

      }
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

