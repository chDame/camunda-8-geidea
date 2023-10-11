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


			var lead = await dueDilligenceService.LeadCreate_ELMCheck(new LeadValidationRequest()
			{
				NationalId = variables["nationalId"]!.ToString(),
				CountryPrefix = "+966",
				PhoneNumber = variables["phoneNumber"]!.ToString()
			});

			variables["phoneIdValid"] = (lead is not null && lead.ElmCheck.HasValue && lead.ElmCheck.Value);

			return variables;
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

