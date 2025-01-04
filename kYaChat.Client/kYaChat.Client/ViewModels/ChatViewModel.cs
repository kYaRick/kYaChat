using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using kYaChat.Client.Services.Interfaces;
using kYaChat.Shared.Dtos;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace kYaChat.Client.ViewModels;

public partial class ChatViewModel : ViewModelBase
{
   private readonly IChatService _chatService;

   [ObservableProperty]
   private ObservableCollection<ChatMessageDto> _messages = new();

   [ObservableProperty]
   private string _newMessage = string.Empty;

   [ObservableProperty]
   private string _errorMessage = string.Empty;

   [ObservableProperty]
   private RoomDto? _currentRoom;

   public ChatViewModel(IChatService chatService)
   {
      _chatService = chatService;

      _chatService.MessageReceived += OnMessageReceived;
      _chatService.RoomUpdated += OnRoomUpdated;
      _chatService.ErrorReceived += OnErrorReceived;
   }

   private void OnMessageReceived(ChatMessageDto message)
   {
      Messages.Add(message);
   }

   private void OnRoomUpdated(RoomDto room)
   {
      CurrentRoom = room;
      Messages.Add(new ChatMessageDto
      {
         UserName = "System",
         Message = $"Room updated: {room.Users.Count} users",
         Timestamp = DateTime.UtcNow,
         Type = MessageType.SystemMessage
      });
   }

   private void OnErrorReceived(string error)
   {
      ErrorMessage = error;
      Messages.Add(new ChatMessageDto
      {
         UserName = "System",
         Message = error,
         Timestamp = DateTime.UtcNow,
         Type = MessageType.Error
      });
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
         ErrorMessage = string.Empty;
      }
      catch (Exception ex)
      {
         ErrorMessage = ex.Message;
      }
   }

   public override void Cleanup()
   {
      _chatService.MessageReceived -= OnMessageReceived;
      _chatService.RoomUpdated -= OnRoomUpdated;
      _chatService.ErrorReceived -= OnErrorReceived;

      base.Cleanup();
   }
}
