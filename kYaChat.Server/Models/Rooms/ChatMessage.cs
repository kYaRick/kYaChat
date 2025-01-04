namespace kYaChat.Server.Rooms;

public class ChatMessage
{
   public string UserName { get; set; } = string.Empty;
   public string Message { get; set; } = string.Empty;
   public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
