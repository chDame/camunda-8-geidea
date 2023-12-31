﻿using System;
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
using System.Text.Json.Nodes;

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
			_dBContext.TaskModels.AddAsync(task);
			await _dBContext.SaveChangesAsync();
		}

		public async Task<TaskModel?> GetTask(string jobKey, string processInstanceKey)
		{
			return await _dBContext.TaskModels.FirstOrDefaultAsync(t => t.processInstanceKey == processInstanceKey && t.jobKey == jobKey);
		}

		public async Task<List<TaskModel>> GetTaskByCorrelationId(string corrId)
		{
			return await _dBContext.TaskModels.Where(t => t.correlationId == corrId).ToListAsync();
		}

		public async System.Threading.Tasks.Task CompleteTask(string jobKey, JsonNode variables)
		{
			await _provider.GetZeebeClient().
				NewCompleteJobCommand(Convert.ToInt64(jobKey)).
				Variables(System.Text.Json.JsonSerializer.Serialize(variables)).
				Send();
		}

	}
}

