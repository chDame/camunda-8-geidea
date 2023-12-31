﻿using Grpc.Core;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using tasklistDotNetReact.Services;

namespace tasklistDotNetReact.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProcessController : ControllerBase
	{

		private readonly ILogger<ProcessController> _logger;
		private readonly ZeebeClientProvider _zeebeClientProvider;
		private readonly OperateService operateService;
		private readonly IConfiguration _configuration;

		public ProcessController(ILogger<ProcessController> logger, Services.ZeebeClientProvider zeebeClientProvider, OperateService operateService, IConfiguration configuration)
		{
			_logger = logger;
			_zeebeClientProvider = zeebeClientProvider;
			this.operateService = operateService;
			_configuration = configuration;
		}


		[HttpGet]
		public async Task<JsonResult> topology()
		{
			// Get cluster topology
			var topology = await _zeebeClientProvider.GetZeebeClient().TopologyRequest().Send();

			Console.WriteLine("Cluster connected: " + topology);

			return new JsonResult(new { topology = topology });
		}

		[HttpPost]
		public async Task<JsonResult> DeployProcess([FromQuery] string processName)
		{
			var processPath = _configuration.GetValue<string>("resourcePath");
			var demoProcessPath = $"{processPath}/{processName}.bpmn";

			var deployResponse = await _zeebeClientProvider.GetZeebeClient().NewDeployCommand()
				.AddResourceFile(demoProcessPath)
				.Send();

			var processDefinitionKey = deployResponse.Processes[0].ProcessDefinitionKey;

			return new JsonResult(new { processDefinitionKey = processDefinitionKey });
		}

		[HttpPost("{bpmnProcessId}/start")]
		public async Task<JsonResult> CreateProcessInstance(string bpmnProcessId, [FromBody] Dictionary<string, object> variables)
		{
			//Get IP address
			var userIP = Request.HttpContext.Connection.RemoteIpAddress!.ToString();

			// Retreive server / local IP address
			//var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
			//var userIP = feature?.LocalIpAddress?.ToString();
			if (userIP == "::1")
			{
				userIP = GetIPAddress();
			}
			Console.WriteLine(userIP);

			var correlationId = Guid.NewGuid().ToString();
			variables.Add("correlationId", correlationId);
			variables.Add("merchantIP", userIP);

			var processInstance = await _zeebeClientProvider.GetZeebeClient()
				.NewCreateProcessInstanceCommand()
				.BpmnProcessId(bpmnProcessId)
				.LatestVersion()
				.Variables(System.Text.Json.JsonSerializer.Serialize(variables))
				.Send();

			return new JsonResult(new { ProcessInstanceKey = processInstance.ProcessInstanceKey, correlationId = correlationId });
		}

		[HttpGet("definition/latest")]
		public async Task<JsonResult> latestDefinitions()
		{
			return new JsonResult(await operateService.ProcessDefinitions());
		}

		private static string GetIPAddress()
		{
			String address = "";
			WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
			using (WebResponse response = request.GetResponse())
			using (StreamReader stream = new StreamReader(response.GetResponseStream()))
			{
				address = stream.ReadToEnd();
			}

			int first = address.IndexOf("Address: ") + 9;
			int last = address.LastIndexOf("</body>");
			address = address.Substring(first, last - first);

			return address;
		}
	}
}