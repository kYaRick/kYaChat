using Microsoft.AspNetCore.SignalR;

namespace kYaChat.Server.Hubs;

public class ChatHub : Hub
{
   private static readonly Dictionary<string, string> _userRooms = new();

   public async Task JoinRoom(string userName, string roomName)
   {
      await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
      _userRooms[Context.ConnectionId] = roomName;
      Context.Items["UserName"] = userName;
      await Clients.Group(roomName).SendAsync("UserJoined", userName);
   }

   public async Task SendMessage(string message)
   {
      if (_userRooms.TryGetValue(Context.ConnectionId, out string? roomName))
      {
         var userName = Context.Items["UserName"] as string;
         await Clients.Group(roomName).SendAsync("ReceiveMessage", userName, message);
      }
   }

   public override async Task OnDisconnectedAsync(Exception? exception)
   {
      if (_userRooms.TryGetValue(Context.ConnectionId, out string? roomName))
      {
         var userName = Context.Items["UserName"] as string;
         await Clients.Group(roomName).SendAsync("UserLeft", userName);
         _userRooms.Remove(Context.ConnectionId);
      }
      await base.OnDisconnectedAsync(exception);
   }
}

