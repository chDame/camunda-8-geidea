using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using tasklistDotNetReact.Common;
using tasklistDotNetReact.DataAccess;
using tasklistDotNetReact.Services;
using Zeebe.Client.Accelerator.Extensions;
using Zeebe.Client.Accelerator.Options;
using Zeebe.Client.Impl.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

// in memory database used for simplicity, change to a real db for production applications
builder.Services.AddDbContext<AppDBContext>
	(o => o.UseInMemoryDatabase("MyDatabase"));

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: "_myAllowSpecificOrigins",
					  policy =>
					  {
						  policy.WithOrigins("https://localhost:44403").AllowAnyHeader()
													  .AllowAnyMethod().SetIsOriginAllowed(origin => true).AllowCredentials();
						  // allow any origin .AllowCredentials()); // allow credentials;

					  });
});
// Add services to the container.

builder.Services.AddOptions<UrlSettings>().Bind(builder.Configuration.GetSection("UrlSettings"));

builder.Services.AddControllersWithViews();
builder.Services.AddTransient(typeof(ZeebeConfigProvider));
builder.Services.AddTransient(typeof(TaskListClientProvider));
builder.Services.AddTransient(typeof(OperateClientProvider));
builder.Services.AddTransient(typeof(TaskListService));
builder.Services.AddTransient(typeof(OperateService));
builder.Services.AddTransient(typeof(FormService));
builder.Services.AddTransient(typeof(MailService));
builder.Services.AddTransient(typeof(TemplateService));
builder.Services.AddTransient(typeof(MockService));
builder.Services.AddTransient(typeof(FileService));
builder.Services.AddTransient(typeof(ZeebeClientProvider));
builder.Services.AddHostedService<UserTaskWorker>();

builder.Services.AddScoped<HttpClient>();
builder.Services.AddTransient<IDueDilligenceService, DueDilligenceService>();

builder.Services.BootstrapZeebe(builder.Configuration.GetSection("ZeebeConfiguration"), o =>
{
	IConfigurationSection clientConf = builder.Configuration.GetSection("ZeebeConfiguration").GetSection("Client");
	string audience = clientConf.GetValue<string>("clusterId") + "." + clientConf.GetValue<string>("region") + ".zeebe.camunda.io";
	o.Client = new ZeebeClientAcceleratorOptions.ClientOptions()
	{
		GatewayAddress = audience + ":443",

		TransportEncryption = new ZeebeClientAcceleratorOptions.ClientOptions.TransportEncryptionOptions()
		{
			AccessTokenSupplier = CamundaCloudTokenProvider.Builder()
						.UseClientId(clientConf.GetValue<string>("clientId"))
						.UseClientSecret(clientConf.GetValue<string>("clientSecret"))
						.UseAudience(audience)
						.Build()
		}
	};


}, typeof(Program).Assembly);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("_myAllowSpecificOrigins");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Auth}/{action=Index}/{id?}");

app.MapHub<SignalrHub>("/hub");

app.MapFallbackToFile("index.html"); ;

app.Run();
