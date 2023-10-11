namespace tasklistDotNetReact.DataAccess.Entities
{
	public class TaskModel
	{
		public int Id { get; set; }
		public string processInstanceKey { get; set; }		
		public string jobKey { get; set; }
		public string formKey { get; set; }
		public string assignee { get; set; }
		public Dictionary<string, object> variables { get; set; }
	}
}
