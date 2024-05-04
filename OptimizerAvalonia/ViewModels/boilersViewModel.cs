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
    private ObservableCollection<IBoiler> _activeBoilers = new();
    public ObservableCollection<IBoiler> ActiveBoilers
    {   
        get { return _activeBoilers; }
        set
        {
            _activeBoilers = value;
        }
    }

    private ObservableCollection<IBoiler> _deactivatedBoilers = new();
    public ObservableCollection<IBoiler> DeactivatedBoilers
    {   
        get { return _deactivatedBoilers; }
        set
        {
            _deactivatedBoilers = value;
        }
    }


    
    

    public boilersViewModel()
    {
        ActiveBoilers = new();

        var assetManager = new AssetManager();
        IBoiler gasBoiler = assetManager.LoadBoilerData<GasBoiler>("GasBoiler.csv");
        IBoiler oilBoiler = assetManager.LoadBoilerData<OilBoiler>("OilBoiler.csv");
        IBoiler gasMotor = assetManager.LoadBoilerData<GasMotor>("GasMotor.csv");
        IBoiler electricBoiler = assetManager.LoadBoilerData<ElectricBoiler>("ElectricBoiler.csv");

        ActiveBoilers.Add(gasBoiler);
        ActiveBoilers.Add(oilBoiler);
        ActiveBoilers.Add(gasMotor);
        ActiveBoilers.Add(electricBoiler);
    }
}


