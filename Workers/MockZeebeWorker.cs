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

		public PhoneIdValidWorker(IDueDilligenceService dueDilligenceService)
		{
			this.dueDilligenceService = dueDilligenceService;
		}
		public async Task<JsonNode> HandleJob(ZeebeJob job, CancellationToken cancellationToken)
		{
			// get variables as declared (SimpleJobPayload)
			JsonNode variables = job.getVariables<JsonNode>();


			var leadId = await dueDilligenceService.LeadCreate_ELMCheck(new LeadValidationRequest()
			{
				NationalId = variables["nationalId"].ToString(),
				CountryPrefix = "+966",
				PhoneNumber = variables["phoneNumber"].ToString()
			});

			variables["phoneIdValid"] = (leadId is not null && leadId.LeadId != Guid.Empty);

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

