using System;
using System.Collections.Generic;
using HeatProductionOptimization.Models;

namespace HeatProductionOptimization.Interfaces
{
    public interface IOptimizedData
    {
        // Start time of the data
        DateTime StartTime { get; set; }

        // End time of the data
        DateTime EndTime { get; set; }

        // The total production cost summed from all time slices. 
        double TotalProductionCost { get; set; }

        // Amount of emissions
        double Emissions { get; set; } 

        // List of boilers we are using
        List<BoilerProduction>? BoilerProductions { get; set; }
    }
}