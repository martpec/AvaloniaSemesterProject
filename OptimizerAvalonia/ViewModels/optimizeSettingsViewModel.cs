using System.Reactive;
using ReactiveUI;

namespace OptimizerAvalonia.ViewModels;

public class optimizeSettingsViewModel: ViewModelBase
{
    private bool isEmissions;

    public bool IsEmissions
    {
        get => isEmissions;
        set => SetProperty(ref isEmissions, value);
    }
}