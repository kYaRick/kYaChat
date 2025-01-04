using Microsoft.Extensions.DependencyInjection;

namespace kYaChat.Client.Helpers.Di;

public static class ConfigurationExtensions
{
   public static IServiceCollection AddConfiguration(this IServiceCollection services)
   {
      services.AddSingleton(new AppConfiguration());
      return services;
   }
}
