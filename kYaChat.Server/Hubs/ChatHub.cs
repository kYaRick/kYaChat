using kYaChat.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace kYaChat.Server.Hubs;

public class ChatHub : Hub
{
   private readonly IChatRoomManager _roomManager;

   public ChatHub(IChatRoomManager roomManager)
   {
      _roomManager = roomManager;
   }

   public async Task JoinRoom(string userName, string roomName)
   {
      await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
      _roomManager.AddUserToRoom(Context.ConnectionId, userName, roomName);
      Context.Items["UserName"] = userName;

      var users = _roomManager.GetUsersInRoom(roomName);
      await Clients.Caller.SendAsync("UserList", users);
      await Clients.Group(roomName).SendAsync("UserJoined", userName);
   }

   public async Task SendMessage(string message)
   {
      var userInfo = _roomManager.GetUserInfo(Context.ConnectionId);
      if (userInfo.RoomName != null)
      {
         var userName = Context.Items["UserName"] as string;
         var timestamp = DateTime.UtcNow;
         await Clients.Group(userInfo.RoomName).SendAsync("ReceiveMessage", userName, message, timestamp);
      }
   }

   public async Task LeaveRoom()
   {
      var userInfo = _roomManager.GetUserInfo(Context.ConnectionId);
      if (userInfo.RoomName != null)
      {
         var userName = Context.Items["UserName"] as string;
         await RemoveUserFromRoom(userName!, userInfo.RoomName);
      }
   }

   public override async Task OnDisconnectedAsync(Exception? exception)
   {
      var userInfo = _roomManager.GetUserInfo(Context.ConnectionId);
      if (userInfo.RoomName != null)
      {
         var userName = Context.Items["UserName"] as string;
         await RemoveUserFromRoom(userName!, userInfo.RoomName);
      }
      await base.OnDisconnectedAsync(exception);
   }

   private async Task RemoveUserFromRoom(string userName, string roomName)
   {
      await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
      _roomManager.RemoveUserFromRoom(Context.ConnectionId, roomName);
      await Clients.Group(roomName).SendAsync("UserLeft", userName);
   }
}