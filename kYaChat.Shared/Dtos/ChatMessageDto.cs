namespace kYaChat.Shared.Dtos;

public enum MessageType
{
   UserMessage,
   SystemMessage,
   Error
}

public class ChatMessageDto
{
   public string UserName { get; set; } = string.Empty;
   public string Message { get; set; } = string.Empty;
   public DateTime Timestamp { get; set; }
   public MessageType Type { get; set; } = MessageType.UserMessage;
}