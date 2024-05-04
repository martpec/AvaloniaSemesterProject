namespace OptimizerAvalonia.ViewModels;
using System.Reactive;
using ReactiveUI;
public class MainWindowViewModel : ViewModelBase
{
    //========================================= Pane open/close
    private bool _isPaneOpen = true;
    public bool IsPaneOpen
    {
        get { return _isPaneOpen; }
        set => this.RaiseAndSetIfChanged(ref _isPaneOpen, value);
    }
    public void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
    //=========================================
}
