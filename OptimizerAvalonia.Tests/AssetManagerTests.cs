using Xunit;
using HeatProductionOptimization;
using HeatProductionOptimization.Models;

namespace OptimizerAvalonia.Tests;

public class AssetManagerTests
{
    [Fact]
    public void LoadBoilerData_GasBoilerTest()
    {
        // Arrange
        var assetManager = new AssetManager();
        var tempFilePath = Path.Combine(Path.GetTempPath(), "boilerdata.csv");

        var csvContent = "Name:,MaxHeat:,ProductionCost:,CO2 Emissions:,GasConsumption:\n" +
                         "GB,\"5.0 MW\",500 DKK / MWh (th),215 kg / MWh (th),\"1.1 MWh(gas) / MWh (th)\"";

        File.WriteAllText(tempFilePath, csvContent);

        // Act
        var boiler = assetManager.LoadBoilerData<GasBoiler>(tempFilePath);

        // Assert
        Assert.NotNull(boiler);
        Assert.Equal("GB", boiler.Name);
        Assert.Equal(5.0, boiler.MaxHeat);
        Assert.Equal(500, boiler.ProductionCost);

        // Clean up
        File.Delete(tempFilePath);
    }
}

