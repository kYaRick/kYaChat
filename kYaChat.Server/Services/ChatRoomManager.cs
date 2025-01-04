namespace kYaChat.Server.Services;

public class ChatRoomManager : IChatRoomManager
{
   private readonly Dictionary<string, string> _userNames = new();
   private readonly Dictionary<string, string> _userRooms = new();
   private readonly Dictionary<string, HashSet<string>> _roomUsers = new();
   private readonly object _lock = new();

   public void AddUserToRoom(string connectionId, string userName, string roomName)
   {
      lock (_lock)
      {
         _userNames[connectionId] = userName;
         _userRooms[connectionId] = roomName;
         if (!_roomUsers.ContainsKey(roomName))
            _roomUsers[roomName] = new HashSet<string>();
         _roomUsers[roomName].Add(userName);
      }
   }

   public List<string> GetUsersInRoom(string roomName)
   {
      lock (_lock)
      {
         return _roomUsers.TryGetValue(roomName, out var users)
             ? users.ToList()
             : new List<string>();
      }
   }

   public (string? UserName, string? RoomName) GetUserInfo(string connectionId)
   {
      lock (_lock)
      {
         if (_userRooms.TryGetValue(connectionId, out var roomName))
         {
            return (_userNames[connectionId], roomName);
         }
         return (null, null);
      }
   }

   public void RemoveUserFromRoom(string connectionId, string roomName)
   {
      lock (_lock)
      {
         if (_userNames.TryGetValue(connectionId, out var userName))
         {
            _userNames.Remove(connectionId);
            _userRooms.Remove(connectionId);
            if (_roomUsers.ContainsKey(roomName))
            {
               _roomUsers[roomName].Remove(userName);
               if (_roomUsers[roomName].Count == 0)
                  _roomUsers.Remove(roomName);
            }
         }
      }
   }
}
