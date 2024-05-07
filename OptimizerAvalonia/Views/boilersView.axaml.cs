using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OptimizerAvalonia.ViewModels;

namespace OptimizerAvalonia.Views;

public partial class BoilersView : UserControl
{
    public BoilersView()
    {
        InitializeComponent();
        DataContext = new BoilersViewModel(); 
    }
    
    
}