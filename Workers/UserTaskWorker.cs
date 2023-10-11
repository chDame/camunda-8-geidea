using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using Zeebe.Client.Accelerator.Abstractions;
using Zeebe.Client.Accelerator.Attributes;
using Zeebe.Client.Api.Worker;

namespace tasklistDotNetReact.Services
{
	public class UserTaskWorker : IHostedService, IDisposable
	{
		private readonly IHubContext<SignalrHub> _hubContext;

		private CancellationTokenSource cancellationTokenSource;
		private readonly ZeebeClientProvider zeebeClientProvider;
		private Zeebe.Client.Api.Worker.IJobWorker jobWorker;
		private readonly TaskListService _taskListService;

		public UserTaskWorker(ZeebeClientProvider zeebeClientProvider, IHubContext<SignalrHub> _hubContext, TaskListService taskListService)
		{
			this.zeebeClientProvider = zeebeClientProvider;
			this._hubContext = _hubContext;
			_taskListService = taskListService;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			this.cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			jobWorker = this.zeebeClientProvider.GetZeebeClient().NewWorker()
			  .JobType("io.camunda.zeebe:userTask")
			  .Handler(async (c, job) =>
			  {
				  Dictionary<string, string> headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(job.CustomHeaders);

				  TaskModel task = new()
				  {
					  assignee = headers.ContainsKey("io.camunda.zeebe:assignee") ? headers["io.camunda.zeebe:assignee"].ToString() : "",
					  formKey = headers["io.camunda.zeebe:formKey"].ToString(),
					  jobKey = job.Key.ToString(),
					  variables = JsonConvert.DeserializeObject<Dictionary<string, object>>(job.Variables)
				  };

				  await _hubContext.Clients.Group(job.ProcessInstanceKey.ToString()).SendAsync("newTask", task);

				  await _taskListService.AddTask(job.ProcessInstanceKey.ToString(), task);

			  })
			  .MaxJobsActive(100)
			  .Name(Environment.MachineName)
			  .PollInterval(TimeSpan.FromMilliseconds(100))
			  .Timeout(TimeSpan.FromMinutes(20))
			  .Open();

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			try
			{
				this.cancellationTokenSource.Cancel();
			}
			finally
			{
				StopInternal();
			}

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			StopInternal();
		}
		public void StopInternal()
		{
			jobWorker.Dispose();
		}
	}
}

