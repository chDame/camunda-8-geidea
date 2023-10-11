using System;
using GraphQL;
using System.Threading.Tasks;
using GraphQL.Client.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

using GraphQL.Client.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using static tasklistDotNetReact.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tasklistDotNetReact.Common;

namespace tasklistDotNetReact.Services
{
	public class TaskListService
	{
		private readonly ZeebeClientProvider _provider;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public TaskListService(ZeebeClientProvider provider, IHttpContextAccessor httpContextAccessor)
		{
			_provider = provider;
			_httpContextAccessor = httpContextAccessor;
		}

		public async System.Threading.Tasks.Task AddTask(string processInstanceKey, TaskModel task)
		{
			ProcessInstanceTasksModel processInstanceTasks = new();
			processInstanceTasks = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<ProcessInstanceTasksModel>(processInstanceKey);

			if (processInstanceTasks is null)
			{
				processInstanceTasks = new ProcessInstanceTasksModel
				{
					processInstanceKey = processInstanceKey,
					tasks = new List<TaskModel> { task }
				};
			}
			else
			{
				processInstanceTasks.tasks.Add(task);
			}

			_httpContextAccessor.HttpContext.Session.SetObjectAsJson(processInstanceKey, task);
		}

		public async Task<TaskModel> GetTask(string jobKey, string processInstanceKey)
		{
			throw new NotImplementedException();
		}

		public async Task<List<TaskModel>> GetTaskByProcessInstanceKey(string processInstanceKey)
		{
			var processInstanceTasks = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<ProcessInstanceTasksModel>(processInstanceKey);
			return processInstanceTasks.tasks;
		}

		public async System.Threading.Tasks.Task CompleteTask(string jobKey, Dictionary<string, object> variables)
		{
			await _provider.GetZeebeClient().
				NewCompleteJobCommand(Convert.ToInt64(jobKey)).
				Variables(JsonConvert.SerializeObject(variables)).
				Send();
		}

	}

	public class ProcessInstanceTasksModel
	{
		public string processInstanceKey { get; set; }
		public List<TaskModel> tasks { get; set; }
	}

	public class TaskModel
	{
		public string jobKey { get; set; }
		public string formKey { get; set; }
		public string assignee { get; set; }
		public Dictionary<string, object> variables { get; set; }
	}
}

