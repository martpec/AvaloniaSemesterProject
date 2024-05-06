using HeatProductionOptimization.Interfaces;
using System;
using System.Collections.Generic;
namespace HeatProductionOptimization.Models
{
    public class SourceData : ISourceData
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public double HeatDemand { get; set; }
        
        public double ElectricityPrice { get; set; }
     
    }
}