using kYaChat.Server.Users;

namespace kYaChat.Server.Rooms;

public class ChatRoom
{
   public string Id { get; set; } = Guid.NewGuid().ToString();
   public string Name { get; set; } = string.Empty;
   public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

   public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
   public virtual ICollection<ChatUser> Users { get; set; } = new List<ChatUser>();
}
