using HeatProductionOptimization;
using HeatProductionOptimization.Models;
using HeatProductionOptimization.Interfaces;

namespace OptimizerAvalonia.Tests;

public class OptimizerTests
{
    private List<IBoiler> GetBoilers()
    {
        return new List<IBoiler>
        {
            new GasBoiler { Name = "Test_GasBoiler1", MaxHeat = 5, ProductionCost = 10, Emissions = 5 },
            new GasBoiler { Name = "Test_GasBoiler2", MaxHeat = 3, ProductionCost = 8, Emissions = 3 },
        };
    }

    private List<SourceData> GetDemandData()
    {
        return new List<SourceData>
        {
            new SourceData
            {
                StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1), HeatDemand = 8, ElectricityPrice = 0.5
            },
            new SourceData
            {
                StartTime = DateTime.Now.AddHours(1), EndTime = DateTime.Now.AddHours(2), HeatDemand = 5,
                ElectricityPrice = 0.6
            }
        };
    }

    [Fact]
    public void CalculateOptimalHeatProduction_OptimizesByCost()
    {
        // Arrange
        var boilers = GetBoilers();
        var demandData = GetDemandData();
        var optimizer = new Optimizer(boilers, demandData);

        // Act
        optimizer.CalculateOptimalHeatProduction(optimizeEmissions: false);

        // Assert
        Assert.Equal(2, optimizer.OptimizedData.Count);
        var firstInterval = optimizer.OptimizedData[0];
        var secondInterval = optimizer.OptimizedData[1];

        Assert.True(firstInterval.TotalProductionCost > 0);
        Assert.True(secondInterval.TotalProductionCost > 0);
        Assert.Equal(8, firstInterval.HeatDemand);
        Assert.Equal(5, secondInterval.HeatDemand);
    }

    [Fact]
    public void CalculateOptimalHeatProduction_OptimizesByEmissions()
    {
        // Arrange
        var boilers = GetBoilers();
        var demandData = GetDemandData();
        var optimizer = new Optimizer(boilers, demandData);

        // Act
        optimizer.CalculateOptimalHeatProduction(optimizeEmissions: true);

        // Assert
        Assert.Equal(2, optimizer.OptimizedData.Count);
        var firstInterval = optimizer.OptimizedData[0];
        var secondInterval = optimizer.OptimizedData[1];

        Assert.True(firstInterval.Emissions > 0);
        Assert.True(secondInterval.Emissions > 0);
        Assert.Equal(8, firstInterval.HeatDemand);
        Assert.Equal(5, secondInterval.HeatDemand);
    }

    [Fact]
    public void CalculateOptimalHeatProduction_InefficientResources()
    {
        // Arrange
        var boilers = new List<IBoiler>();
        var sourceData = new List<SourceData>();

        SourceData data = new SourceData
        {
            StartTime = DateTime.Parse("2/8/2023 0:00"),
            EndTime = DateTime.Parse("2/8/2023 1:00"),
            HeatDemand = 6.62,
            ElectricityPrice = 1190.94
        };
        sourceData.Add(data);

        var optimizer = new Optimizer(boilers, sourceData);

        // Capture the console output
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        // Act
        optimizer.CalculateOptimalHeatProduction(optimizeEmissions: false);

        // Get the console output
        var output = stringWriter.ToString();

        // Assert
        Assert.Contains(
            "Insufficient capacity to meet the demand for interval 02/08/2023 00:00:00 to 02/08/2023 01:00:00", output);

        // Reset the console output
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }

    // Two Boilers one doesn't have enough capacity to produce whole heat demand and second only needs to satisfy demand lower than 1 MW
    [Fact]
    public void CalculateOptimalHeatProduction_TwoBoilers1MW()
    {
        // Arrange
        var boilers = new List<IBoiler>();
        var gasBoiler = new GasBoiler { Name = "Test_GasBoiler1", MaxHeat = 1, ProductionCost = 10, Emissions = 5 };
        var elecBoiler = new ElectricBoiler { Name = "Test_GasBoiler2", MaxHeat = 4, ProductionCost = 3, Emissions = 25, ElectricityConsumption = 7};
        boilers.Add(gasBoiler);
        boilers.Add(elecBoiler);
        var sourceData = new List<SourceData>();

        SourceData data = new SourceData
        {
            StartTime = DateTime.Parse("2/8/2023 0:00"),
            EndTime = DateTime.Parse("2/8/2023 1:00"),
            HeatDemand = 1.4,
            ElectricityPrice = 1190.94
        };
        sourceData.Add(data);

        var optimizer = new Optimizer(boilers, sourceData);
        
        // Act
        optimizer.CalculateOptimalHeatProduction(optimizeEmissions: false);
        
        // Assert
        Assert.Equal(1, optimizer.OptimizedData[0].BoilerProductions[0].HeatProduced);
        Assert.Equal(1, optimizer.OptimizedData[0].BoilerProductions[1].HeatProduced);
    }
    
    // Two boilers one is going to make less than 1 MW 
    [Fact]
    public void CalculateOptimalHeatProduction_TwoBoilersAndDecresaFromPrevious()
    {
        // Arrange
        var boilers = new List<IBoiler>();
        var gasBoiler = new GasBoiler { Name = "Test_GasBoiler1", MaxHeat = 2, ProductionCost = 10, Emissions = 5 };
        var elecBoiler = new ElectricBoiler { Name = "Test_GasBoiler2", MaxHeat = 4, ProductionCost = 3, Emissions = 25, ElectricityConsumption = 7};
        boilers.Add(gasBoiler);
        boilers.Add(elecBoiler);
        var sourceData = new List<SourceData>();

        SourceData data = new SourceData
        {
            StartTime = DateTime.Parse("2/8/2023 0:00"),
            EndTime = DateTime.Parse("2/8/2023 1:00"),
            HeatDemand = 2.2,
            ElectricityPrice = 1190.94
        };
        sourceData.Add(data);

        var optimizer = new Optimizer(boilers, sourceData);
        
        // Act
        optimizer.CalculateOptimalHeatProduction(optimizeEmissions: false);
        
        // Assert
        Assert.Equal(1.2, optimizer.OptimizedData[0].BoilerProductions[0].HeatProduced,2);
        Assert.Equal(1, optimizer.OptimizedData[0].BoilerProductions[1].HeatProduced,2);
    }
}