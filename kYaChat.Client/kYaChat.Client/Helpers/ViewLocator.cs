using Avalonia.Controls;
using Avalonia.Controls.Templates;
using kYaChat.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace kYaChat.Client.Helpers;

public class ViewLocator : IDataTemplate
{
   private static readonly IReadOnlyList<(string From, string To)> _controlMapper =
   [
        ("ViewModel", "View"),
        ("ViewModel", "Page"),
   ];

   public Control? Build(object? param)
   {
      if (param is null)
         return null;

      var fullName = param.GetType().FullName!;

      var type = _controlMapper
          .Select(mapping => Type.GetType(fullName.Replace(
              mapping.From,
              mapping.To,
              StringComparison.Ordinal)))
          .FirstOrDefault(t => t is not null);

      if (type is null)
         return new TextBlock { Text = $"Not Found: {fullName}" };

      try
      {
         return (Control)Activator.CreateInstance(type)!;
      }
      catch (Exception ex)
      {
         return new TextBlock { Text = $"Failed To Create: {type.Name}\n{ex.Message}" };
      }
   }

   public bool Match(object? data) =>
      data is ViewModelBase;
}