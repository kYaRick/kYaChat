using kYaChat.Shared.Dtos;
using System;
using System.Threading.Tasks;

namespace kYaChat.Client.Services.Interfaces;

public interface IChatService
{
   Task ConnectAsync();
   Task DisconnectAsync();
   Task JoinRoomAsync(string userName, string roomName);
   Task LeaveRoomAsync();
   Task SendMessageAsync(string message);

   event Action<RoomDto>? RoomUpdated;
   event Action<ChatMessageDto>? MessageReceived;
   event Action<string>? ErrorReceived;

   bool IsConnected { get; }
   string? CurrentRoom { get; }
   string? CurrentUser { get; }
}
