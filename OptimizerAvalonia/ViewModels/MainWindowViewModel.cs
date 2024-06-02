using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OptimizerAvalonia.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    //========================================= Pane open/close
    [ObservableProperty] private bool _isPaneOpen = true;

    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
    //=========================================

    [ObservableProperty] private ViewModelBase _currentPage = new HomePageViewModel();

    [ObservableProperty] private ListItemTemplate? _selectedListItem;

    private readonly Dictionary<Type, ViewModelBase> _viewModelCache = new();

    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null) return;

        // Check if we have already created an instance of this ViewModel type
        if (!_viewModelCache.TryGetValue(value.ModelType, out var viewModel))
        {
            // If not, create and store it
            var instance = Activator.CreateInstance(value.ModelType);
            if (instance is null) return;
            viewModel = (ViewModelBase)instance;
            _viewModelCache[value.ModelType] = viewModel;
        }

        // Set the current page to the cached instance
        CurrentPage = viewModel;
    }

    // Adds Views to the list
    public ObservableCollection<ListItemTemplate> Items { get; } =
    [
        new ListItemTemplate(typeof(HomePageViewModel), "HomeRegular", "Home"),
        new ListItemTemplate(typeof(OptimizerViewModel), "DataHistogram", "Optimizer"),
        new ListItemTemplate(typeof(CostsViewModel), "MoneyRegular", "Costs"),
        new ListItemTemplate(typeof(EmissionsViewModel), "DataPie", "Emissions"),
        new ListItemTemplate(typeof(ElectricityViewModel), "DataWaterfall", "Electricity Price"),
        new ListItemTemplate(typeof(SettingsViewModel), "BoilerSettings", "Settings")
    ];
}

public class ListItemTemplate
{
    // Creates template for every Item in Pane
    public ListItemTemplate(Type type, string iconKey, string label)
    {
        ModelType = type;
        Label = label;

        Application.Current!.TryFindResource(iconKey, out var res);
        ListItemIcon = (StreamGeometry)res!;
    }

    public string Label { get; }
    public Type ModelType { get; }
    public StreamGeometry ListItemIcon { get; }
}