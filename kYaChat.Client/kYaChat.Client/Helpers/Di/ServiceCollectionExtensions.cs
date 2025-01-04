using kYaChat.Client.Services.Implementations;
using kYaChat.Client.Services.Interfaces;
using kYaChat.Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace kYaChat.Client.Helpers.Di;

public static class ServiceCollectionExtensions
{
   public static IServiceCollection AddApplication(this IServiceCollection services)
   {
      services.AddViewModels();
      services.AddServices();

      return services;
   }

   private static IServiceCollection AddViewModels(this IServiceCollection services)
   {
      services.AddTransient<WaitingRoomViewModel>();
      services.AddTransient<ChatViewModel>();
      return services;
   }

   private static IServiceCollection AddServices(this IServiceCollection services)
   {
      services.AddSingleton<IChatService, SignalRChatService>();
      return services;
   }
}
