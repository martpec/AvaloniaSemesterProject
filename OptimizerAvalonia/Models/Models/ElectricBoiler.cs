using System;
using System.Collections.Generic;
using HeatProductionOptimization.Interfaces;
using static HeatProductionOptimization.AssetManager;

namespace HeatProductionOptimization.Models
{
    // Electric boiler model class
    public class ElectricBoiler : IElectricBoiler
    {
        public string? Name { get; set; }
        public double MaxHeat { get; set; }
        public double ProductionCost { get; set; }
        public double Emissions { get; set; } = 0;
        public double ElectricityConsumption { get; set; }

        public void SetAdditionalProperties(string[] values)
        {
            ElectricityConsumption = ExtractNumber(values[3]);
            //System.Console.WriteLine($"Name: {Name}\nMaxHeat: {MaxHeat}\nProductionCost: {ProductionCost}\nElectricConsumption: {ElectricityConsumption}\n");
        }

    }


}

// Name: EK

// Max heat: 8,0 MW

// Max electricity(ElectricityConsumption): -8,0 MW

// Production costs: 50 DKK / MWh(th)