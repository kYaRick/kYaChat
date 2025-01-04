namespace kYaChat.Shared.Dtos;

public class RoomDto
{
   public string Name { get; set; } = string.Empty;
   public List<string> Users { get; set; } = new();
   public List<ChatMessageDto> Messages { get; set; } = new();
}
