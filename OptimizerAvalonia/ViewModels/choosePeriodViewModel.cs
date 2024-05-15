using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OptimizerAvalonia.ViewModels;

public partial class ChoosePeriodViewModel: ViewModelBase
{
    private readonly string _summerPath = "SummerData.csv";
    private readonly string _winterPath = "WinterData.csv";
    
    [ObservableProperty]
    private bool _isSummer;

    [RelayCommand]
    private void TriggerButton()
    {
        if (IsSummer)
        {
            SourceDataPath = _summerPath;
            IsSummer = false;
        }
        else
        {
            SourceDataPath = _winterPath;
            IsSummer = true;
        }
    }
}