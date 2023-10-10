using tasklistDotNetReact.Dtos.Models;

namespace tasklistDotNetReact.Services
{
	public interface IDueDilligenceService
	{
		Task<LeadValidationResponse> LeadCreate_ELMCheck(LeadValidationRequest leadValidationRequest);
	}
}