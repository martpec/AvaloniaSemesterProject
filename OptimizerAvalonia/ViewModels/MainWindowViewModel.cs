namespace OptimizerAvalonia.ViewModels;
using System.Reactive;
using ReactiveUI;
public class MainWindowViewModel : BaseViewModel
{
    //========================================= Pane open/close
    private bool _isPaneOpen = true;
    public MainWindowViewModel() {
        boiler = new boilersViewModel();
    }
    public bool IsPaneOpen
    {
        get { return _isPaneOpen; }
        set => this.RaiseAndSetIfChanged(ref _isPaneOpen, value);
    }
    public void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
    private boilersViewModel boiler;
    public boilersViewModel Boiler {
        get => boiler;
        set => this.RaiseAndSetIfChanged(ref boiler, value);
    }
    //=========================================
}
