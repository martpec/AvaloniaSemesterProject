using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OptimizerAvalonia.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    // Hardcoded paths to data sources
    private readonly string _summerPath = "SummerData.csv";
    private readonly string _winterPath = "WinterData.csv";

    [ObservableProperty] private bool _isWinter;

    [RelayCommand]
    private void TriggerButton() // Event on button trigger
    {
        SourceDataPath = IsWinter ? _winterPath : _summerPath;
    }
}