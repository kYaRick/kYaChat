using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using kYaChat.Client.Pages;
using kYaChat.Client.ViewModels;
using kYaChat.Client.Views;
using System.Linq;

namespace kYaChat.Client;

public partial class App : Application
{
   public override void Initialize() =>
      AvaloniaXamlLoader.Load(this);

   public override void OnFrameworkInitializationCompleted()
   {
      if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
      {
         DisableAvaloniaDataAnnotationValidation();
         desktop.MainWindow = new MainWindow
         {
            DataContext = new WaitingRoomViewModel()
         };
      }
      else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
      {
         singleViewPlatform.MainView = new WaitingRoomPage
         {
            DataContext = new WaitingRoomViewModel()
         };
      }

      base.OnFrameworkInitializationCompleted();
   }

   private void DisableAvaloniaDataAnnotationValidation()
   {
      var dataValidationPluginsToRemove =
          BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

      foreach (var plugin in dataValidationPluginsToRemove)
         BindingPlugins.DataValidators.Remove(plugin);
   }
}