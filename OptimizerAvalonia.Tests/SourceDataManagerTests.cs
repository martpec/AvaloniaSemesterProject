using HeatProductionOptimization;

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

    [Fact]
    public void LoadSourceData_WinterPeriod()
    {
        // Arrange
        var sourceDataManager = new SourceDataManager();
        var tempFilePath = Path.Combine(Path.GetTempPath(), "sourcedata.csv");

        var csvContent = "Winter period,,,\n" +
                         "Time from,Time to,Heat Demand,Electricity Price\n" +
                         "DKK local time,DKK local time,MWh,DKK / Mwh(el)\n" +
                         "2/8/2023 0:00,2/8/2023 1:00,6.62,1190.94\n" +
                         "2/8/2023 1:00,2/8/2023 2:00,6.85,1154.55\n" +
                         "2/8/2023 2:00,2/8/2023 3:00,6.98,1116.22\n" +
                         "2/8/2023 3:00,2/8/2023 4:00,7.04,1101.12\n" +
                         "2/8/2023 4:00,2/8/2023 5:00,7.72,1086.24\n" +
                         "2/8/2023 5:00,2/8/2023 6:00,7.85,1109.53";

        File.WriteAllText(tempFilePath, csvContent);

        // Act
        var sourceDataList = sourceDataManager.LoadSourceData(tempFilePath);

        // Assert
        Assert.NotNull(sourceDataList);
        Assert.Equal(6, sourceDataList.Count);

        Assert.Equal(new DateTime(2023, 2, 8, 0, 0, 0), sourceDataList[0].StartTime);
        Assert.Equal(new DateTime(2023, 2, 8, 1, 0, 0), sourceDataList[0].EndTime);
        Assert.Equal(6.62, sourceDataList[0].HeatDemand);
        Assert.Equal(1190.94, sourceDataList[0].ElectricityPrice);

        Assert.Equal(new DateTime(2023, 2, 8, 5, 0, 0), sourceDataList[5].StartTime);
        Assert.Equal(new DateTime(2023, 2, 8, 6, 0, 0), sourceDataList[5].EndTime);
        Assert.Equal(7.85, sourceDataList[5].HeatDemand);
        Assert.Equal(1109.53, sourceDataList[5].ElectricityPrice);

        // Clean up
        File.Delete(tempFilePath);
    }

    [Fact]
    public void LoadSourceData_FileNotFoundTest()
    {
        // Arrange
        var sourceDataManager = new SourceDataManager();
        var nonExistentFilePath = Path.Combine(Path.GetTempPath(), "nonexistent.csv");

        // Act
        void Act() => sourceDataManager.LoadSourceData(nonExistentFilePath);


        // Assert
        Assert.Throws<FileNotFoundException>(Act);
    }

    [Fact]
    public void LoadSourceData_EmptyFile()
    {
        // Arrange
        var sourceDataManager = new SourceDataManager();
        var tempFilePath = Path.Combine(Path.GetTempPath(), "sourcedata_empty.csv");

        var csvContent = "";

        File.WriteAllText(tempFilePath, csvContent);

        // Act
        var sourceDataList = sourceDataManager.LoadSourceData(tempFilePath);

        // Assert
        Assert.NotNull(sourceDataList);
        Assert.Empty(sourceDataList);

        // Clean up
        File.Delete(tempFilePath);
    }

    [Fact]
    public void LoadSourceData_IncorrectDateFormat()
    {
        // Arrange
        var sourceDataManager = new SourceDataManager();
        var tempFilePath = Path.Combine(Path.GetTempPath(), "sourcedata_incorrect_date_format.csv");

        var csvContent = "Incorrect Date Format,,,\n" +
                         "Time from,Time to,Heat Demand,Electricity Price\n" +
                         "DKK local time,DKK local time,MWh,DKK / Mwh(el)\n" +
                         "01-01-2023 00:00,01-01-2023 01:00,5.0,1000.0\n";

        File.WriteAllText(tempFilePath, csvContent);

        // Capture the console output
        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        // Act
        sourceDataManager.LoadSourceData(tempFilePath);

        // Get the console output
        var output = stringWriter.ToString();

        // Assert
        Assert.Contains("An error occurred while loading source data", output);

        // Clean up
        File.Delete(tempFilePath);
        // Reset the console output
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }

    [Fact]
    public void LoadSourceData_ExtraColumns()
    {
        // Arrange
        var sourceDataManager = new SourceDataManager();
        var tempFilePath = Path.Combine(Path.GetTempPath(), "sourcedata_extra_columns.csv");

        var csvContent = "Extra Columns,,,\n" +
                         "Time from,Time to,Heat Demand,Electricity Price,Extra Column\n" +
                         "DKK local time,DKK local time,MWh,DKK / Mwh(el),Extra\n" +
                         "1/1/2023 0:00,1/1/2023 1:00,5.0,1000.0,ExtraData\n";

        File.WriteAllText(tempFilePath, csvContent);

        // Act
        var sourceDataList = sourceDataManager.LoadSourceData(tempFilePath);

        // Assert
        Assert.NotNull(sourceDataList);
        // checks if it contains only SourceData objects
        Assert.Single(sourceDataList);

        Assert.Equal(new DateTime(2023, 1, 1, 0, 0, 0), sourceDataList[0].StartTime);
        Assert.Equal(new DateTime(2023, 1, 1, 1, 0, 0), sourceDataList[0].EndTime);
        Assert.Equal(5.0, sourceDataList[0].HeatDemand);
        Assert.Equal(1000.0, sourceDataList[0].ElectricityPrice);

        // Clean up
        File.Delete(tempFilePath);
    }
}