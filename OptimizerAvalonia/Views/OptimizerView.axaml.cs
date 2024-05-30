using Avalonia.Controls;
using OptimizerAvalonia.ViewModels;

namespace OptimizerAvalonia.Views;

public partial class OptimizerView : UserControl
{
    public OptimizerView()
    {
        InitializeComponent();
        DataContext = new OptimizerViewModel();
    }
}