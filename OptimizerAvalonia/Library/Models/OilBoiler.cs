using HeatProductionOptimization.Interfaces;
using static HeatProductionOptimization.AssetManager;
using System;
using System.Collections.Generic;

namespace HeatProductionOptimization.Models
{
    // Oil boiler model class
    public class OilBoiler : IOilBoiler
    {
        public string? Name { get; set; }
        public double MaxHeat { get; set; }
        public double ProductionCost { get; set; }
        public double Emissions { get; set; }
        public double OilConsumption { get; set; }
        public void SetAdditionalProperties(string[] values)
        {
            Emissions = ExtractNumber(values[3]);
            OilConsumption = ExtractNumber(values[4]);
            //System.Console.WriteLine($"Name: {Name}\nMaxHeat: {MaxHeat}\nProductionCost: {ProductionCost}\nEmissions: {Emissions}\nOilConsumption: {OilConsumption}\n");
        }
    }
}

// Name: OB

// Max heat: 4,0 MW

// Production costs: 700 DKK / MWh(th)

// CO2 emissions: 265 kg / MWh(th)

// Gas consumption: 1,2 MWh(oil) / MWh(th)