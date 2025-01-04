namespace kYaChat.Client.Helpers;

public class AppConfiguration
{
   public string ApiBaseUrl { get; set; } = "https://localhost:7198";
   public string ChatHubUrl => $"{ApiBaseUrl}/chat";
}
