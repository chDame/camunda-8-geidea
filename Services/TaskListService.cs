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
using tasklistDotNetReact.DataAccess.Entities;
using tasklistDotNetReact.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace tasklistDotNetReact.Services
{
	public class TaskListService
	{
		private readonly ZeebeClientProvider _provider;
		private readonly AppDBContext _dBContext;

		public TaskListService(ZeebeClientProvider provider, AppDBContext dBContext)
		{
			_provider = provider;
			_dBContext = dBContext;
		}

		public async System.Threading.Tasks.Task AddTask(TaskModel task)
		{
			_dBContext.TaskModels.Add(task);
			await _dBContext.SaveChangesAsync();
		}

		public async Task<TaskModel?> GetTask(string jobKey, string processInstanceKey)
		{
			return _dBContext.TaskModels.FirstOrDefault(t => t.processInstanceKey == processInstanceKey && t.jobKey == jobKey);
		}

		public async Task<List<TaskModel>> GetTaskByProcessInstanceKey(string processInstanceKey)
		{
			return _dBContext.TaskModels.Where(t => t.processInstanceKey == processInstanceKey).ToList();
		}

		public async System.Threading.Tasks.Task CompleteTask(string jobKey, Dictionary<string, object> variables)
		{
			await _provider.GetZeebeClient().
				NewCompleteJobCommand(Convert.ToInt64(jobKey)).
				Variables(JsonConvert.SerializeObject(variables)).
				Send();
		}

	}
}

