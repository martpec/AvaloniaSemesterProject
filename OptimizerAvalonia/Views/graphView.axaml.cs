using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OptimizerAvalonia.ViewModels;

namespace OptimizerAvalonia.Views;

public partial class graphView : UserControl
{
    public graphView()
    {
        InitializeComponent();
        DataContext = new graphViewModel();
    }
}