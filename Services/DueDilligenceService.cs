using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using tasklistDotNetReact.Common;
using tasklistDotNetReact.Dtos.Models;

namespace tasklistDotNetReact.Services
{
	[ExcludeFromCodeCoverage]
	public class DueDilligenceService: IDueDilligenceService
	{
		private readonly ILogger<DueDilligenceService> logger;
		private readonly HttpClient httpClient;

		private readonly string DueDilligenceServiceBaseUrl;
		public DueDilligenceService(ILogger<DueDilligenceService> logger, HttpClient httpClient,
			IOptionsMonitor<UrlSettings> urlSettingsOptions)
		{
			this.logger = logger;
			this.httpClient = httpClient;
			//to updated in mmsintegrator later remove /users
			DueDilligenceServiceBaseUrl = $"{urlSettingsOptions.CurrentValue.MerchantExperienceMMSIntegratorBaseUrl}/api/v1/DueDilligence";
		}

		public async Task<LeadValidationResponse> LeadCreate_ELMCheck(LeadValidationRequest leadValidationRequest)
		{
			var url = $"{DueDilligenceServiceBaseUrl}/validate";

			using (logger.BeginScope("LeadValidate({url})", url))
			{
				logger.LogInformation("Calling DueDilligence Service to create lead and make elm check.");

				var requestBody = new StringContent(JsonConvert.SerializeObject(leadValidationRequest), Encoding.Default, "application/json");
				var response = await httpClient.PostAsync(url, requestBody);
				var responseBody = await response.Content.ReadAsStringAsync();

				if (!response.IsSuccessStatusCode)
				{
					logger.LogCritical("Error when calling DueDilligence API. Error was {StatusCode} {@responseBody}", (int)response.StatusCode, responseBody);
					throw new Exception("Error when calling DueDilligence API");
				}

				logger.LogInformation("Success calling Users API");

				return JsonConvert.DeserializeObject<LeadValidationResponse>(responseBody);
			}
		}
	}
}