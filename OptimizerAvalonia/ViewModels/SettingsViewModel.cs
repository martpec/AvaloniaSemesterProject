using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HeatProductionOptimization.Interfaces;
using HeatProductionOptimization.Models;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace OptimizerAvalonia.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    // Hardcoded paths to data sources
    private readonly string _summerPath = "SummerData.csv";
    private readonly string _winterPath = "WinterData.csv";

    [ObservableProperty] private bool _isWinter;

    [RelayCommand]
    private void TriggerButton() // Event on button trigger
    {
        SourceDataPath = IsWinter ? _winterPath : _summerPath;
    }
    
    [ObservableProperty]
    private List<IBoiler>? _dataGridBoilers;

    public SettingsViewModel()
    {
        DataGridBoilers = ListOfIBoilers;
    }
}

// Class to convert the data for the boiler data grid
public class BoilerTypeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var boiler = value as IBoiler;
        var propertyName = parameter as string;

        if (boiler == null || propertyName == null)
            return null;

        return propertyName switch
        {
            "GasConsumption" => boiler is GasBoiler gasBoiler ? gasBoiler.GasConsumption : null,
            "ElectricityConsumption" => boiler is ElectricBoiler electricBoiler ? electricBoiler.ElectricityConsumption : null,
            "OilConsumption" => boiler is OilBoiler oilBoiler ? oilBoiler.OilConsumption : null,
            "ElectricityProduced" => boiler is GasMotor gasMotor ? gasMotor.ElectricityProduced : null,
            _ => null
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}