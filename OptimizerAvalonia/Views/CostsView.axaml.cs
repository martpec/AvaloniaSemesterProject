using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OptimizerAvalonia.ViewModels;

namespace OptimizerAvalonia.Views;

public partial class CostsView : UserControl
{
    public CostsView()
    {
        InitializeComponent();
        DataContext = new CostsViewModel();
    }
}