using Microsoft.AspNetCore.SignalR;

namespace tasklistDotNetReact.Services
{
	public class SignalrHub : Hub
	{
		public async Task Join(string processInstanceKey)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, processInstanceKey);

			await Clients.Group(processInstanceKey).SendAsync("messageReceived");
			//await Clients.All.SendAsync("messageReceived", user, message);
		}
	}
}
