using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OptimizerAvalonia.ViewModels;

public partial class SettingsViewModel: ViewModelBase
{
    
    [ObservableProperty]
    private bool _isWinter = false;
    
}