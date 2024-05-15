namespace HeatProductionOptimization.Interfaces
{
    public interface IBoiler
    {   
        // Name of the boiler
        string? Name { get; set; }

        // The maximum heat production of the boiler in MW
        double MaxHeat { get; set; }

        // The production cost of the boiler in DKK/MWh
        double ProductionCost { get; set; }

        // The CO2 emissions of the boiler in kg/MWh
        double Emissions { get; set; }

        // Method to set the properties of the boiler
        public void SetAdditionalProperties(string[] values);
        
    }
}