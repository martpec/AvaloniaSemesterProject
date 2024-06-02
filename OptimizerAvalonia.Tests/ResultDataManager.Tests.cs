using HeatProductionOptimization.Models;
using HeatProductionOptimization;

namespace OptimizerAvalonia.Tests;

public class ResultDataManagerTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly List<OptimizedData> _optimizedDataSample;

    public ResultDataManagerTests()
    {
        // Setup a test directory
        _testDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestResults");

        // Sample data
        _optimizedDataSample = new List<OptimizedData>
        {
            new OptimizedData
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                TotalProductionCost = 100.0,
                Emissions = 200.0,
                BoilerProductions = new List<BoilerProduction>
                {
                    new BoilerProduction { BoilerName = "Boiler1", HeatProduced = 50.0 },
                    new BoilerProduction { BoilerName = "Boiler2", HeatProduced = 50.0 }
                }
            }
        };

        // Ensure test directory exists
        if (!Directory.Exists(_testDirectory))
        {
            Directory.CreateDirectory(_testDirectory);
        }
    }

    [Fact]
    public void SaveOptimizedData_ShouldSaveFileSuccessfully()
    {
        // Arrange
        string fileName = "test_output.csv";
        string filePath = Path.Combine(_testDirectory, fileName);

        // Act
        ResultDataManager.SaveOptimizedData(_optimizedDataSample, filePath);

        // Assert
        Assert.True(File.Exists(filePath), "File was not created successfully.");

        // Cleanup
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public void SaveOptimizedData_DirectoryDoesNotExist_ShouldCreateDirectory()
    {
        // Arrange
        string fileName = "nonexistent_directory/test_output.csv";
        string filePath = Path.Combine(_testDirectory, fileName);

        // Act
        ResultDataManager.SaveOptimizedData(_optimizedDataSample, filePath);

        // Assert
        Assert.True(File.Exists(filePath), "File was not created in a nonexistent directory.");

        // Cleanup
        string? directoryPath = Path.GetDirectoryName(filePath);
        if (directoryPath != null && Directory.Exists(directoryPath))
        {
            Directory.Delete(directoryPath, true);
        }
    }
    
    public void Dispose()
    {
        // Cleanup test directory
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }
}