using HeatProductionOptimization.Interfaces;
using System;
using System.Collections.Generic;

namespace HeatProductionOptimization.Models
{
    public class OptimizedData : IOptimizedData
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double TotalProductionCost { get; set; }
        public double Emissions { get; set; } = 0; // TODO: Decide whether the emissions will be calculated
        public List<BoilerProduction>? BoilerProductions { get; set; }
    }
}