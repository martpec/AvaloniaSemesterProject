using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
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