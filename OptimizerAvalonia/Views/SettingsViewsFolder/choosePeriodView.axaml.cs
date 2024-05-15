using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OptimizerAvalonia.ViewModels;

namespace OptimizerAvalonia.Views;

public partial class choosePeriodView : UserControl
{
    public choosePeriodView()
    {
        InitializeComponent();
        DataContext = new ChoosePeriodViewModel();
    }
}