using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace kYaChat.Client.ViewModels;

public partial class WaitingRoomViewModel : ViewModelBase
{
   [ObservableProperty]
   private string _userName;

   [ObservableProperty]
   private string _chatName;

   public WaitingRoomViewModel()
   {
      UserName = "";
      ChatName = "";
   }

   [RelayCommand]
   public void Join()
   {

   }
}
