using System;
using System.Diagnostics.CodeAnalysis;

namespace tasklistDotNetReact.Dtos.Models
{
	[ExcludeFromCodeCoverage]
	public class LeadValidationResponse
	{
		public Guid LeadId { get; set; }
		public bool? ElmCheck { get; set; }
	}
}