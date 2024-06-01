using HeatProductionOptimization;
using HeatProductionOptimization.Models;

namespace OptimizerAvalonia.Tests;

public class AssetManagerTests
{
    //---------------LoadBoilerDataTests-------------
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
        Assert.Equal(215, boiler.Emissions);
        Assert.Equal(1.1, boiler.GasConsumption);

        // Clean up
        File.Delete(tempFilePath);
    }

    [Fact]
    public void LoadBoilerData_ElectricBoilerTest()
    {
        // Arrange
        var assetManager = new AssetManager();
        var tempFilePath = Path.Combine(Path.GetTempPath(), "boilerdata_electricboiler.csv");

        var csvContent = "Name:,MaxHeat:,ProductionCost:,MaxElectricity:\n" +
                         "EK,8.0 MW,50 DKK / MWh(th),-8.0 MW";

        File.WriteAllText(tempFilePath, csvContent);

        // Act
        var boiler = assetManager.LoadBoilerData<ElectricBoiler>(tempFilePath);

        // Assert
        Assert.NotNull(boiler);
        Assert.Equal("EK", boiler.Name);
        Assert.Equal(8.0, boiler.MaxHeat);
        Assert.Equal(50, boiler.ProductionCost);
        Assert.Equal(-8.0, boiler.ElectricityConsumption);

        // Clean up
        File.Delete(tempFilePath);
    }

    [Fact]
    public void LoadBoilerData_GasMotorTest()
    {
        // Arrange
        var assetManager = new AssetManager();
        var tempFilePath = Path.Combine(Path.GetTempPath(), "boilerdata_gasmotor.csv");

        var csvContent = "Name:,MaxHeat:,ProductionCost:,CO2 Emissions:,GasConsumption:,MaxElectricity:\n" +
                         "GM,3.6 MW,1100 DKK / MWh (th),640 kg / MWh (th),1.9 MWh(gas) / MWh (th),2.7 MW";

        File.WriteAllText(tempFilePath, csvContent);

        // Act
        var motor = assetManager.LoadBoilerData<GasMotor>(tempFilePath);

        // Assert
        Assert.NotNull(motor);
        Assert.Equal("GM", motor.Name);
        Assert.Equal(3.6, motor.MaxHeat);
        Assert.Equal(1100, motor.ProductionCost);
        Assert.Equal(640, motor.Emissions);
        Assert.Equal(1.9, motor.GasConsumption);
        Assert.Equal(2.7, motor.ElectricityProduced);

        // Clean up
        File.Delete(tempFilePath);
    }

    [Fact]
    public void LoadBoilerData_OilBoilerTest()
    {
        // Arrange
        var assetManager = new AssetManager();
        var tempFilePath = Path.Combine(Path.GetTempPath(), "boilerdata_oilboiler.csv");

        var csvContent = "Name:,MaxHeat:,ProductionCost:,CO2 Emissions:,OilConsumption:\n" +
                         "OB,4.0 MW,700 DKK / MWh (th),265 kg / MWh (th),1.2 MWh(oil) / MWh (th)";

        File.WriteAllText(tempFilePath, csvContent);

        // Act
        var boiler = assetManager.LoadBoilerData<OilBoiler>(tempFilePath);

        // Assert
        Assert.NotNull(boiler);
        Assert.Equal("OB", boiler.Name);
        Assert.Equal(4.0, boiler.MaxHeat);
        Assert.Equal(700, boiler.ProductionCost);
        Assert.Equal(265, boiler.Emissions);
        Assert.Equal(1.2, boiler.OilConsumption);

        // Clean up
        File.Delete(tempFilePath);
    }

    [Fact]
    public void LoadBoilerData_MissingFieldsTest()
    {
        // Arrange
        var assetManager = new AssetManager();
        var tempFilePath = Path.Combine(Path.GetTempPath(), "boilerdata_missing_fields.csv");

        var csvContent = "Name:,MaxHeat:,ProductionCost:,CO2 Emissions:,GasConsumption:\n" +
                         "GB,,500 DKK / MWh (th),,";

        File.WriteAllText(tempFilePath, csvContent);

        // Act
        var boiler = assetManager.LoadBoilerData<GasBoiler>(tempFilePath);

        // Assert
        Assert.NotNull(boiler);
        Assert.Equal("GB", boiler.Name);
        Assert.Equal(0.0, boiler.MaxHeat); // Default value for missing MaxHeat is 0.0
        Assert.Equal(500, boiler.ProductionCost);

        // Clean up
        File.Delete(tempFilePath);
    }

    [Fact]
    public void LoadBoilerData_InvalidNumberFormatTest()
    {
        // Arrange
        var assetManager = new AssetManager();
        var tempFilePath = Path.Combine(Path.GetTempPath(), "boilerdata_invalid_number_format.csv");

        var csvContent = "Name:,MaxHeat:,ProductionCost:,CO2 Emissions:,GasConsumption:\n" +
                         "GB,\"invalid number\",500 DKK / MWh (th),215 kg / MWh (th),\"1.1 MWh(gas) / MWh (th)\"";

        File.WriteAllText(tempFilePath, csvContent);

        // Act
        void Act() => assetManager.LoadBoilerData<GasBoiler>(tempFilePath);

        // Assert
        Assert.Throws<FormatException>(Act);

        // Clean up
        File.Delete(tempFilePath);
    }

    [Fact]
    public void LoadBoilerData_EmptyFileTest()
    {
        // Arrange
        var assetManager = new AssetManager();
        var tempFilePath = Path.Combine(Path.GetTempPath(), "boilerdata_empty.csv");

        File.WriteAllText(tempFilePath, "");

        // Act
        void Act() => assetManager.LoadBoilerData<GasBoiler>(tempFilePath);

        // Assert
        Assert.Throws<NullReferenceException>(Act);

        // Clean up
        File.Delete(tempFilePath);
    }

    [Fact]
    public void LoadBoilerData_FileNotFoundTest()
    {
        // Arrange
        var assetManager = new AssetManager();
        var nonExistentFilePath = Path.Combine(Path.GetTempPath(), "nonexistent.csv");

        // Act
        void Act() => assetManager.LoadBoilerData<GasBoiler>(nonExistentFilePath);

        // Assert
        Assert.Throws<FileNotFoundException>(Act);
    }

    //-------------ExtractNumberTests-------------
    [Theory]
    [InlineData("5.0", 5.0)]
    [InlineData("500", 500.0)]
    [InlineData("-123.45", -123.45)]
    [InlineData("text 789.0 more text", 789.0)]
    [InlineData(" 123.45 ", 123.45)]
    [InlineData("", 0.0)]
    [InlineData(null, 0.0)]
    public void ExtractNumber_TestVariousInputs(string input, double expected)
    {
        // Act
        var result = AssetManager.ExtractNumber(input);

        // Assert
        Assert.Equal(expected, result);
    }
}