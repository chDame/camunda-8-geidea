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
		public UserTaskWorker(ZeebeClientProvider zeebeClientProvider, IHubContext<SignalrHub> _hubContext)
		{
			this.zeebeClientProvider = zeebeClientProvider;
			this._hubContext = _hubContext;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			this.cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			jobWorker = this.zeebeClientProvider.GetZeebeClient().NewWorker()
			  .JobType("io.camunda.zeebe:userTask")
			  .Handler((c, job) =>
			  {
				  Dictionary<string, string> headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(job.CustomHeaders);
				  Console.WriteLine("User task form " + headers["io.camunda.zeebe:formKey"]);
				  Console.WriteLine("User task job key " + job.Key);
				  Console.WriteLine("User task process instance key " + job.ProcessInstanceKey);
				  if (headers.ContainsKey("io.camunda.zeebe:assignee"))
				  {
					  Console.WriteLine("assignee " + headers["io.camunda.zeebe:assignee"]);
				  }

				  _hubContext.Clients.Group("demo"/*job.ProcessInstanceKey.ToString()*/).SendAsync("newTask",
				new Dictionary<string, string>() { { "formkey", headers["io.camunda.zeebe:formKey"].ToString() } ,
													{"jobKey",job.Key.ToString() }, 
													{"jobProcessInstanceKey",job.ProcessInstanceKey.ToString() },
													{ "assignee",headers.ContainsKey("io.camunda.zeebe:assignee") ? headers["io.camunda.zeebe:assignee"].ToString() : ""} }
					  );

			  })
			  .MaxJobsActive(100)
			  .Name(Environment.MachineName)
			  .PollInterval(TimeSpan.FromMilliseconds(100))
			  .Timeout(TimeSpan.FromMinutes(1))
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

