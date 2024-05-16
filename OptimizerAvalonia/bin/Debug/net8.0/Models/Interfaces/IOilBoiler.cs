namespace HeatProductionOptimization.Interfaces
{
    public interface IOilBoiler : IBoiler, IEmissionProducer
    {   
        // Oil consumption in MWh
        double OilConsumption { get; set; }
    }
}