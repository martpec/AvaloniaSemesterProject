using HeatProductionOptimization.Interfaces;
using static HeatProductionOptimization.AssetManager;

namespace HeatProductionOptimization.Models;

// Gas boiler model class
public class GasBoiler : IGasBoiler
{
    public string? Name { get; set; }
    public double MaxHeat { get; set; }
    public double ProductionCost { get; set; }
    public double Emissions { get; set; }
    public double GasConsumption { get; set; }

    public void SetAdditionalProperties(string[] values)
    {
        Emissions = ExtractNumber(values[3]);
        GasConsumption = ExtractNumber(values[4]);

        //System.Console.WriteLine($"Name: {Name}\nMaxHeat: {MaxHeat}\nProductionCost: {ProductionCost}\nEmissions: {Emissions}\nGasConsumption: {GasConsumption}\n");
    }
}


// Name: GB 

// Max heat: 5,0 MW 

// Production costs: 500 DKK / MWh(th) 

// CO2 emissions: 215 kg / MWh(th) 

// Gas consumption: 1,1 MWh(gas) / MWh(th)