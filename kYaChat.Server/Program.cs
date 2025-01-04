using kYaChat.Server.Hubs;
using kYaChat.Server.Services;
using Microsoft.Azure.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IChatRoomManager, ChatRoomManager>();

builder.Services.AddSignalR().AddAzureSignalR(builder.Configuration["Azure:SignalR:ConnectionString"]);
builder.Services.AddCors(options =>
{
   options.AddPolicy("ClientPermission", policy =>
   {
      policy.AllowAnyHeader()
          .AllowAnyMethod()
          .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
          .AllowCredentials();
   });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
   app.UseDeveloperExceptionPage();

app.UseRouting();
app.UseCors("ClientPermission");

app.UseAzureSignalR(routes =>
{
   routes.MapHub<ChatHub>("/chat");
});

app.Run();