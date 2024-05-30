namespace HeatProductionOptimization.Interfaces;

public interface IGasBoiler : IBoiler, IEmissionProducer
{
    // Gas consumption in MWh
    double GasConsumption { get; set; }
}