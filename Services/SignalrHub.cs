using Microsoft.AspNetCore.SignalR;

namespace tasklistDotNetReact.Services
{
	public class SignalrHub : Hub
	{
		public async Task SubscribeToTasks(string processInstanceKey)
		{
			await Clients.User(processInstanceKey).SendAsync("messageReceived");
			//await Clients.All.SendAsync("messageReceived", user, message);
		}
	}
}
