using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OptimizerAvalonia.ViewModels;

namespace OptimizerAvalonia.Views;

public partial class MoneyDisplayView : UserControl
{
    public MoneyDisplayView()
    {
        InitializeComponent();
        DataContext = new MoneyDisplayViewModel();
    }
}