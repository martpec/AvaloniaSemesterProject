using System;
using System.Collections.Generic;
namespace HeatProductionOptimization.Interfaces

{
    public interface ISourceData
    {   
        // Start time of the data  
        public DateTime StartTime { get; set; }

        // End time of the data
        public DateTime EndTime { get; set; }

        // The heat demand douring the hour in MWh
        public double HeatDemand { get; set; }
        
        // The electricity price during the hour in DKK/MWh
        public double ElectricityPrice { get; set; }

    }
}