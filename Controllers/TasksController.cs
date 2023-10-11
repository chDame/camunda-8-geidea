using Microsoft.AspNetCore.Mvc;
using tasklistDotNetReact.Services;

namespace tasklistDotNetReact.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TasksController : ControllerBase
	{

		private readonly ILogger<TasksController> _logger;
		private readonly TaskListService _taskListService;

		public TasksController(ILogger<TasksController> logger, TaskListService taskListService)
		{
			_logger = logger;
			_taskListService = taskListService;
		}

		[HttpGet("{processInstanceKey}")]
		public async Task<JsonResult> GetPInstanceTasks(string processInstanceKey)
		{
			List<TaskModel> tasks = await _taskListService.GetTaskByProcessInstanceKey(processInstanceKey);

			return new JsonResult(tasks);
		}

		[HttpPost("{jobKey}")]
		public async Task<IActionResult> complete(string jobKey, [FromBody] Dictionary<string, object> variables)
		{
			await _taskListService.CompleteTask(jobKey, variables);

			return Ok();
		}

	}
}