using kYaChat.Models.Rooms;
using System;
using System.Threading.Tasks;

namespace kYaChat.Client.Services.Interfaces;

public interface IChatService
{
   Task ConnectAsync();
   Task DisconnectAsync();
   Task JoinRoomAsync(string userName, string roomName);
   Task SendMessageAsync(string message);

   event Action<string> OnUserJoined;
   event Action<string> OnUserLeft;
   event Action<ChatMessage> OnMessageReceived;
   event Action<string> OnError;
}
