using Avalonia.Controls;
using OptimizerAvalonia.ViewModels;

namespace OptimizerAvalonia.Views;

public partial class EmissionsGraphView : UserControl
{
    public EmissionsGraphView()
    {
        InitializeComponent();
        DataContext = new EmissionsGraphViewModel();
    }
}