using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OptimizerAvalonia.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly string _summerPath = "SummerData.csv";
    private readonly string _winterPath = "WinterData.csv";

    [ObservableProperty] private bool _isWinter;

    [RelayCommand]
    private void TriggerButton()
    {
        SourceDataPath = IsWinter ? _winterPath : _summerPath;
    }
}