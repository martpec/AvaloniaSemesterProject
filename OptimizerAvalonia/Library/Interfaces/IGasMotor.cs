namespace HeatProductionOptimization.Interfaces
{
    public interface IGasMotor : IBoiler, IEmissionProducer
    {   
        // Gas consumption in MWh
        double GasConsumption { get; set; }

        // Electricity production in MWh, electricity produced can be sold back to the grid
        double ElectricityProduced { get; set; }
    }
}