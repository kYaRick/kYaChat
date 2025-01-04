using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using kYaChat.Client.Helpers.Di;
using kYaChat.Client.Pages;
using kYaChat.Client.ViewModels;
using kYaChat.Client.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace kYaChat.Client;

public partial class App : Application
{
   private IServiceProvider? _serviceProvider;

   public override void Initialize()
   {
      AvaloniaXamlLoader.Load(this);
      RegisterServices();
   }

   private void RegisterServices()
   {
      var services = new ServiceCollection();

      services.AddConfiguration();
      services.AddApplication();

      _serviceProvider = services.BuildServiceProvider();
   }

   public override void OnFrameworkInitializationCompleted()
   {
      if (_serviceProvider == null)
         throw new InvalidOperationException("Service provider is not initialized");

      DisableAvaloniaDataAnnotationValidation();

      if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
      {
         desktop.MainWindow = new MainWindow
         {
            DataContext = _serviceProvider.GetRequiredService<WaitingRoomViewModel>()
         };
      }
      else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
      {
         singleViewPlatform.MainView = new WaitingRoomPage
         {
            DataContext = _serviceProvider.GetRequiredService<WaitingRoomViewModel>()
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