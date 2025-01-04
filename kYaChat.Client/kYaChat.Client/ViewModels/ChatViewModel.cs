using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using kYaChat.Client.Services.Interfaces;
using kYaChat.Models.Rooms;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace kYaChat.Client.ViewModels;

public partial class ChatViewModel : ViewModelBase
{
   private readonly IChatService _chatService;

   [ObservableProperty]
   private ObservableCollection<ChatMessage> _messages = new();

   [ObservableProperty]
   private string _newMessage = string.Empty;

   [ObservableProperty]
   private string _errorMessage = string.Empty;

   public ChatViewModel(IChatService chatService)
   {
      _chatService = chatService;

      _chatService.OnMessageReceived += message =>
      {
         Messages.Add(message);
      };

      _chatService.OnUserJoined += userName =>
      {
         Messages.Add(new ChatMessage
         {
            UserName = "System",
            Message = $"{userName} joined the chat",
            Timestamp = DateTime.UtcNow
         });
      };

      _chatService.OnUserLeft += userName =>
      {
         Messages.Add(new ChatMessage
         {
            UserName = "System",
            Message = $"{userName} left the chat",
            Timestamp = DateTime.UtcNow
         });
      };

      _chatService.OnError += message => ErrorMessage = message;
   }

   [RelayCommand]
   private async Task SendMessageAsync()
   {
      if (string.IsNullOrWhiteSpace(NewMessage))
         return;

      try
      {
         await _chatService.SendMessageAsync(NewMessage);
         NewMessage = string.Empty;
      }
      catch (Exception ex)
      {
         ErrorMessage = ex.Message;
      }
   }
}
