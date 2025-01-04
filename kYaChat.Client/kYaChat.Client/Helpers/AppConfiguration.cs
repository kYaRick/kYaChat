namespace kYaChat.Client.Helpers;

public class AppConfiguration
{
   public string ApiBaseUrl => "https://kyachat.service.signalr.net";
   public string ChatHubUrl => $"{ApiBaseUrl}/chat";
}

