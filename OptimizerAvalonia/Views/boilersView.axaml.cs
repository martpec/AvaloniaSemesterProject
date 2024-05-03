using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OptimizerAvalonia.ViewModels;

namespace OptimizerAvalonia.Views;

public partial class boilersView : UserControl
{
    public boilersView()
    {
        InitializeComponent();
        DataContext = new boilersViewModel();
    }
}