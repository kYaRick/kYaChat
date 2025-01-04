﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace kYaChat.Client.ViewModels;

public abstract class ViewModelBase : ObservableRecipient
{
   public virtual void Cleanup() { }
}