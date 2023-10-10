using System;
using System.Diagnostics.CodeAnalysis;

namespace tasklistDotNetReact.Dtos.Models
{
	[ExcludeFromCodeCoverage]
	public class LeadValidationRequest
	{
		public string CountryPrefix { get; set; } = null!;
		public string NationalId { get; set; } = null!;
		public string PhoneNumber { get; set; } = null!;
		public string PhoneWithPrefix => CountryPrefix + PhoneNumber;
	}
}