using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OptimizerAvalonia.ViewModels;

namespace OptimizerAvalonia.Views;

public partial class optimizeSettingsView : UserControl
{
    public optimizeSettingsView()
    {
        InitializeComponent();
        DataContext = new OptimizeSettingsViewModel();
    }
}