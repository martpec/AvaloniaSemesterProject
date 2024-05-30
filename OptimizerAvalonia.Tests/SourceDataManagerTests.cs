using Xunit;
using HeatProductionOptimization;
using HeatProductionOptimization.Models;

namespace OptimizerAvalonia.Tests;

public class SourceDataManagerTests
{
    [Fact]
    public void LoadSourceData_SummerPeriod()
    {
        // Arrange
        var sourceDataManager = new SourceDataManager();
        var tempFilePath = Path.Combine(Path.GetTempPath(), "sourcedata.csv");

        var csvContent = "Summer period,,,\n" +
                         "Time from,Time to,Heat Demand,Electricity Price\n" +
                         "DKK local time,DKK local time,MWh,DKK / Mwh(el)\n" +
                         "7/8/2023 0:00,7/8/2023 1:00,1.79,752.03\n" +
                         "7/8/2023 1:00,7/8/2023 2:00,1.85,691.05\n" +
                         "7/8/2023 2:00,7/8/2023 3:00,1.76,674.78\n" +
                         "7/8/2023 3:00,7/8/2023 4:00,1.67,652.95\n" +
                         "7/8/2023 4:00,7/8/2023 5:00,1.73,666.3\n" +
                         "7/8/2023 5:00,7/8/2023 6:00,1.79,654.6";

        File.WriteAllText(tempFilePath, csvContent);

        // Act
        var sourceDataList = sourceDataManager.LoadSourceData(tempFilePath);

        // Assert
        Assert.NotNull(sourceDataList);
        Assert.Equal(6, sourceDataList.Count);

        Assert.Equal(new DateTime(2023, 7, 8, 0, 0, 0), sourceDataList[0].StartTime);
        Assert.Equal(new DateTime(2023, 7, 8, 1, 0, 0), sourceDataList[0].EndTime);
        Assert.Equal(1.79, sourceDataList[0].HeatDemand);
        Assert.Equal(752.03, sourceDataList[0].ElectricityPrice);

        Assert.Equal(new DateTime(2023, 7, 8, 5, 0, 0), sourceDataList[5].StartTime);
        Assert.Equal(new DateTime(2023, 7, 8, 6, 0, 0), sourceDataList[5].EndTime);
        Assert.Equal(1.79, sourceDataList[5].HeatDemand);
        Assert.Equal(654.6, sourceDataList[5].ElectricityPrice);

        // Clean up
        File.Delete(tempFilePath);
    }
}

