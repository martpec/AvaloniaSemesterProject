using HeatProductionOptimization.Interfaces;
using static HeatProductionOptimization.AssetManager;
using System;
using System.Collections.Generic;

namespace HeatProductionOptimization.Models
{
    // Gas motor model class
    public class GasMotor : IGasMotor
    {
        public string? Name { get; set; }
        public double MaxHeat { get; set; }
        public double ElectricityProduced { get; set; }
        public double ProductionCost { get; set; }
        public double Emissions { get; set; }
        public double GasConsumption { get; set; }
        
        public void SetAdditionalProperties(string[] values)
        {
            Emissions = ExtractNumber(values[3]);
            GasConsumption = ExtractNumber(values[4]);
            ElectricityProduced = ExtractNumber(values[5]);
            //System.Console.WriteLine($"Name: {Name}\nMaxHeat: {MaxHeat}\nProductionCost: {ProductionCost}\nEmissions: {Emissions}\nGasConsumption: {GasConsumption}\nElectricityProduced: {ElectricityProduced}\n");
        }
    }
}



// Name: GM

// Max heat: 3,6 MW

// Max electricity: 2,7 MW

// Production costs: 1100 DKK / MWh(th)

// CO2 emissions: 640 kg / MWh(th)

// Gas consumption: 1,9 MWh(gas) / MWh(th)