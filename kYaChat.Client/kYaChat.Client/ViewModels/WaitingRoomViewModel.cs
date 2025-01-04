using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using kYaChat.Client.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace kYaChat.Client.ViewModels;

public partial class WaitingRoomViewModel : ViewModelBase
{
   private readonly IChatService _chatService;

   [ObservableProperty]
   private string _userName = string.Empty;

   [ObservableProperty]
   private string _chatName = string.Empty;

   [ObservableProperty]
   private string _errorMessage = string.Empty;

   public WaitingRoomViewModel(IChatService chatService)
   {
      _chatService = chatService;
      _chatService.OnError += message => ErrorMessage = message;
   }

   [RelayCommand]
   private async Task JoinAsync()
   {
      if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(ChatName))
      {
         ErrorMessage = "Please enter username and room name";
         return;
      }

      try
      {
         await _chatService.ConnectAsync();
         await _chatService.JoinRoomAsync(UserName, ChatName);
         // TODO: Navigation to ChatViewModel
      }
      catch (Exception ex)
      {
         ErrorMessage = ex.Message;
      }
   }
}