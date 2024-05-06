using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using HeatProductionOptimization.Interfaces;
using HeatProductionOptimization.Models;
using HeatProductionOptimization;

namespace OptimizerAvalonia.ViewModels;

public class BaseViewModel : ViewModelBase
{

// to access BoilerList use "Boiler.BoilerList"
    public ObservableCollection<Boiler> BoilersList { get; set; } = new();
    public List<IBoiler> IBoilersList { get; set; } = new();

    public BaseViewModel()
    {
        IBoilersList = new List<IBoiler>();

        var assetManager = new AssetManager();
        IBoiler gasBoiler = assetManager.LoadBoilerData<GasBoiler>("GasBoiler.csv");
        IBoiler oilBoiler = assetManager.LoadBoilerData<OilBoiler>("OilBoiler.csv");
        IBoiler gasMotor = assetManager.LoadBoilerData<GasMotor>("GasMotor.csv");
        IBoiler electricBoiler = assetManager.LoadBoilerData<ElectricBoiler>("ElectricBoiler.csv");

        IBoilersList.Add(gasBoiler);
        IBoilersList.Add(oilBoiler);
        IBoilersList.Add(gasMotor);
        IBoilersList.Add(electricBoiler);

        foreach (var boiler in IBoilersList)
        {
            BoilersList.Add(new Boiler(boiler));
        }
    }
}

public class Boiler(IBoiler boiler) : INotifyPropertyChanged
{
    private bool _isActive;

    public bool IsActive
    {
        get { return _isActive; }
        set
        {
            if (_isActive != value)
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }
    }
    public double HeatProduction {get; set;} 
    public string? Name { get; set; } = boiler.Name;
    public double MaxHeat { get; set; } = boiler.MaxHeat;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}