using System.Reactive;
using ReactiveUI;
using HeatProductionOptimization;
using HeatProductionOptimization.Models;
using HeatProductionOptimization.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OptimizerAvalonia.ViewModels;

public class boilersViewModel : ViewModelBase
{
    private ObservableCollection<Boiler> _activeBoilers = new();
    public ObservableCollection<Boiler> ActiveBoilers
    {   
        get { return _activeBoilers; }
        set
        {
            _activeBoilers = value;
        }
    }
    private List<IBoiler> Boilers { get; set; }

    public boilersViewModel()
    {
        Boilers = new List<IBoiler>();

        var assetManager = new AssetManager();
        IBoiler gasBoiler = assetManager.LoadBoilerData<GasBoiler>("GasBoiler.csv");
        IBoiler oilBoiler = assetManager.LoadBoilerData<OilBoiler>("OilBoiler.csv");
        IBoiler gasMotor = assetManager.LoadBoilerData<GasMotor>("GasMotor.csv");
        IBoiler electricBoiler = assetManager.LoadBoilerData<ElectricBoiler>("ElectricBoiler.csv");

        Boilers.Add(gasBoiler);
        Boilers.Add(oilBoiler);
        Boilers.Add(gasMotor);
        Boilers.Add(electricBoiler);
        
        foreach (var boiler in Boilers)
        {
            ActiveBoilers.Add(new Boiler(boiler));
        }
    }
}
public class Boiler(IBoiler boiler)
{
    public string? Name { get; set; } = boiler.Name;
    public double MaxHeat { get; set; } = boiler.MaxHeat;
}

