namespace HeatProductionOptimization.Interfaces
{
    public interface IEmissionProducer
    {
        // Production of emissions in kg/MWh 
        double Emissions { get; set;}
    }
}