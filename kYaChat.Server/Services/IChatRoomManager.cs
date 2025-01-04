namespace kYaChat.Server.Services;

public interface IChatRoomManager
{
   void AddUserToRoom(string connectionId, string userName, string roomName);
   List<string> GetUsersInRoom(string roomName);
   (string? UserName, string? RoomName) GetUserInfo(string connectionId);
   void RemoveUserFromRoom(string connectionId, string roomName);
}