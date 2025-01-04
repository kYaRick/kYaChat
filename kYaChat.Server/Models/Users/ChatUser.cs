using kYaChat.Server.Rooms;

namespace kYaChat.Server.Users;

public class ChatUser
{
   public string Id { get; set; } = Guid.NewGuid().ToString();
   public string UserName { get; set; } = string.Empty;
   public string RoomId { get; set; } = string.Empty;
   public string ConnectionId { get; set; } = string.Empty;
   public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;

   public virtual ChatRoom? Room { get; set; }
}
