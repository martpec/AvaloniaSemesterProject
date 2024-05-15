using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OptimizerAvalonia.ViewModels;

public class choosePeriodViewModel : ViewModelBase
{
    private bool isSummer;

    public bool IsSummer
    {
        get => isSummer;
        set => SetProperty(ref isSummer, value);
    }
}