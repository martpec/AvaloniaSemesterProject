namespace HeatProductionOptimization.Interfaces;

public interface IElectricBoiler : IBoiler
{
    // Electric consumption in MWh
    double ElectricityConsumption { get; set; }
}