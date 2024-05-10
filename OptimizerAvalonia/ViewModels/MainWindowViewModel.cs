using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OptimizerAvalonia.ViewModels;
using System.Reactive;
using ReactiveUI;
public partial class MainWindowViewModel : ViewModelBase
{
    //========================================= Pane open/close
    [ObservableProperty] 
    private bool _isPaneOpen = true;
    
    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
    //=========================================
    
    [ObservableProperty]
    private ViewModelBase _currentPage = new HomePageViewModel();

    [ObservableProperty] 
    private ListItemTemplate _selectedListItem;

    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null) return;
        var instance = Activator.CreateInstance(value.ModelType);
        if(instance is null) return;
        CurrentPage = (ViewModelBase)instance;

    }

    public ObservableCollection<ListItemTemplate> Items { get; } = new()
    {
        new ListItemTemplate(typeof(HomePageViewModel),"HomeRegular"),
        new ListItemTemplate(typeof(OptimizerViewModel), "DataHistogram"),
        new ListItemTemplate(typeof(SettingsViewModel), "BoilerSettings")
    };
}
public class ListItemTemplate
{
    public ListItemTemplate(Type type, string iconKey)
    {
        ModelType = type;
        //var createLabel = SplitByCapitalLetters(type.Name.Replace("ViewModel", ""));
        Label = type.Name.Replace("ViewModel", "");
        
        Application.Current!.TryFindResource(iconKey, out var res);
        ListItemIcon = (StreamGeometry)res!;
    }
        
    public string Label { get; }
    public Type ModelType { get; }
    public StreamGeometry ListItemIcon { get; }
    
    private static string SplitByCapitalLetters(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Use regular expression to split words by capital letters
        return Regex.Replace(input, "(\\B[A-Z])", " $1");
    }
    
}
