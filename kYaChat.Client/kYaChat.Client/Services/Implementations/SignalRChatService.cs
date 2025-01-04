using kYaChat.Client.Helpers;
using kYaChat.Client.Services.Interfaces;
using kYaChat.Shared.Dtos;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace kYaChat.Client.Services.Implementations;

public class SignalRChatService : IChatService, IAsyncDisposable
{
   private readonly HubConnection _hubConnection;
   private string? _currentRoom;
   private string? _currentUser;

   public SignalRChatService(AppConfiguration configuration)
   {
      _hubConnection = new HubConnectionBuilder()
          .WithUrl(configuration.ChatHubUrl)
          .WithAutomaticReconnect()
          .Build();

      _hubConnection.On<RoomDto>("RoomUpdated", room =>
          RoomUpdated?.Invoke(room));

      _hubConnection.On<ChatMessageDto>("ReceiveMessage", message =>
          MessageReceived?.Invoke(message));

      _hubConnection.On<string>("Error", error =>
          ErrorReceived?.Invoke(error));
   }

   public bool IsConnected =>
       _hubConnection.State == HubConnectionState.Connected;

   public string? CurrentRoom => _currentRoom;
   public string? CurrentUser => _currentUser;

   public event Action<RoomDto>? RoomUpdated;
   public event Action<ChatMessageDto>? MessageReceived;
   public event Action<string>? ErrorReceived;

   public async Task ConnectAsync()
   {
      try
      {
         await _hubConnection.StartAsync();
      }
      catch (Exception ex)
      {
         ErrorReceived?.Invoke($"Connection error: {ex.Message}");
         throw;
      }
   }

   public async Task JoinRoomAsync(string userName, string roomName)
   {
      try
      {
         await _hubConnection.InvokeAsync("JoinRoom", userName, roomName);
         _currentUser = userName;
         _currentRoom = roomName;
      }
      catch (Exception ex)
      {
         ErrorReceived?.Invoke($"Failed to join room: {ex.Message}");
         throw;
      }
   }

   public async Task LeaveRoomAsync()
   {
      if (_currentRoom != null)
      {
         await _hubConnection.InvokeAsync("LeaveRoom");
         _currentRoom = null;
      }
   }

   public async Task SendMessageAsync(string message)
   {
      if (!IsConnected || string.IsNullOrEmpty(_currentRoom))
      {
         ErrorReceived?.Invoke("Not connected to a room");
         return;
      }

      await _hubConnection.InvokeAsync("SendMessage", message);
   }

   public async Task DisconnectAsync()
   {
      if (IsConnected)
      {
         await LeaveRoomAsync();
         await _hubConnection.StopAsync();
      }
   }

   public async ValueTask DisposeAsync()
   {
      await DisconnectAsync();
      await _hubConnection.DisposeAsync();
      GC.SuppressFinalize(this);
   }
}
