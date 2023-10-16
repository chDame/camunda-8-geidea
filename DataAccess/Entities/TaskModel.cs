using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Nodes;

namespace tasklistDotNetReact.DataAccess.Entities
{
	public class TaskModel
	{
		public int Id { get; set; }
		public string correlationId { get; set; }
		public string processInstanceKey { get; set; }
		public string jobKey { get; set; }
		public string formKey { get; set; }
		public string assignee { get; set; }
		[Column(TypeName = "jsonb")]
		[JsonIgnore]
		public string variablesStr { get; set; }

		[NotMapped]
		public JsonNode variables
		{
			get => System.Text.Json.JsonSerializer.Deserialize<JsonNode>(variablesStr);
			set => variablesStr = System.Text.Json.JsonSerializer.Serialize(value);
		}

	}
}
