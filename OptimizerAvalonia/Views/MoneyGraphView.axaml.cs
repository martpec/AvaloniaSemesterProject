using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OptimizerAvalonia.ViewModels;

namespace OptimizerAvalonia.Views;

public partial class MoneyGraphView : UserControl
{
    public MoneyGraphView()
    {
        InitializeComponent();
        DataContext = new MoneyGraphViewModel();
    }
}