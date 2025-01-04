using kYaChat.Client.Helpers;
using kYaChat.Client.Services.Interfaces;
using kYaChat.Models.Rooms;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace kYaChat.Client.Services.Implementations;

public class SignalRChatService : IChatService, IAsyncDisposable
{
   private readonly HubConnection _hubConnection;
   private readonly string _hubUrl;
   private bool _isConnected;

   public event Action<string> OnUserJoined = delegate { };
   public event Action<string> OnUserLeft = delegate { };
   public event Action<ChatMessage> OnMessageReceived = delegate { };
   public event Action<string> OnError = delegate { };

   public SignalRChatService(AppConfiguration config)
   {
      _hubUrl = config.ChatHubUrl;
      _hubConnection = new HubConnectionBuilder()
          .WithUrl(_hubUrl)
          .WithAutomaticReconnect()
          .Build();

      RegisterHandlers();
   }

   private void RegisterHandlers()
   {
      _hubConnection.On<string>("UserJoined", (user) =>
          OnUserJoined.Invoke(user));

      _hubConnection.On<string>("UserLeft", (user) =>
          OnUserLeft.Invoke(user));

      _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
          OnMessageReceived.Invoke(new ChatMessage
          {
             UserName = user,
             Message = message,
             Timestamp = DateTime.Now
          }));

      _hubConnection.Closed += async (error) =>
      {
         _isConnected = false;
         OnError.Invoke("З'єднання втрачено. Спроба перепідключення...");
         await ConnectAsync();
      };
   }

   public async Task ConnectAsync()
   {
      if (_isConnected)
         return;

      try
      {
         await _hubConnection.StartAsync();
         _isConnected = true;
      }
      catch (Exception ex)
      {
         OnError.Invoke($"Помилка підключення: {ex.Message}");
         throw;
      }
   }

   public async Task DisconnectAsync()
   {
      if (!_isConnected)
         return;

      try
      {
         await _hubConnection.StopAsync();
         _isConnected = false;
      }
      catch (Exception ex)
      {
         OnError.Invoke($"Помилка відключення: {ex.Message}");
         throw;
      }
   }

   public async Task JoinRoomAsync(string userName, string roomName)
   {
      try
      {
         await _hubConnection.InvokeAsync("JoinRoom", userName, roomName);
      }
      catch (Exception ex)
      {
         OnError.Invoke($"Помилка входу в кімнату: {ex.Message}");
         throw;
      }
   }

   public async Task SendMessageAsync(string message)
   {
      try
      {
         await _hubConnection.InvokeAsync("SendMessage", message);
      }
      catch (Exception ex)
      {
         OnError.Invoke($"Помилка відправки повідомлення: {ex.Message}");
         throw;
      }
   }

   public async ValueTask DisposeAsync()
   {
      if (_hubConnection != null)
      {
         await _hubConnection.DisposeAsync();
      }
   }
}
