using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using HeatProductionOptimization.Interfaces;
using HeatProductionOptimization.Models;
using HeatProductionOptimization;
using LiveChartsCore.Defaults;

namespace OptimizerAvalonia.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    /*------------------Boilers-------------------------*/

    // Dummy Boilers representing data in UI
    public static ObservableCollection<Boiler> BoilersList { get; set; } = new();
    // Boilers Data representing Asset Data
    protected static List<IBoiler> ListOfIBoilers { get; private set; } = new();

    /*----------------------------SourceData----------------------------------------*/
    [ObservableProperty] private static string _sourceDataPath = "SummerData.csv";

    /*---------------Bool to optimize for Emissions/Cost ----------------------*/
    [ObservableProperty] private static bool _isEmissions;

    /*-------------------OptimizedData To read by graph---------------------------*/
    [ObservableProperty]
    private static List<HeatProductionOptimization.Models.OptimizedData> _optimizedDataForGraph = new();

    /*----------------------Money Graph------------------------*/
    [ObservableProperty] private static ObservableCollection<DateTimePoint> _costsPoints = new();

    /*--------------------Emissions Graph------------------*/
    [ObservableProperty] private static ObservableCollection<DateTimePoint> _emissionsPoints = new();

    /*--------------------Electricity Graph------------------*/
    [ObservableProperty] private static ObservableCollection<DateTimePoint> _electricityPoints = new();
    
    protected ViewModelBase()
    {
        if (ListOfIBoilers.Count == 0 && BoilersList.Count == 0)
        {
            ListOfIBoilers = new List<IBoiler>();

            // Load Asset Data
            var assetManager = new AssetManager();
            IBoiler gasBoiler = assetManager.LoadBoilerData<GasBoiler>("GasBoiler.csv");
            IBoiler oilBoiler = assetManager.LoadBoilerData<OilBoiler>("OilBoiler.csv");
            IBoiler gasMotor = assetManager.LoadBoilerData<GasMotor>("GasMotor.csv");
            IBoiler electricBoiler = assetManager.LoadBoilerData<ElectricBoiler>("ElectricBoiler.csv");

            // Adds all Boilers to common list
            ListOfIBoilers.Add(gasBoiler);
            ListOfIBoilers.Add(oilBoiler);
            ListOfIBoilers.Add(gasMotor);
            ListOfIBoilers.Add(electricBoiler);

            // add all boilers to BoilersList
            foreach (var boiler in ListOfIBoilers)
            {
                BoilersList.Add(new Boiler(boiler));
            }
        }
    }
    
}

// class for representing data in the UI
public sealed class Boiler(IBoiler boiler) : INotifyPropertyChanged
{
    private bool _isActive = true;

    public bool IsActive
    {
        get => _isActive;
        set
        {
            if (_isActive != value)
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }
    }

    public double HeatProduction { get; set; } = boiler.MaxHeat;
    public string? Name { get; set; } = boiler.Name;
    public double MaxHeat { get; set; } = boiler.MaxHeat;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}