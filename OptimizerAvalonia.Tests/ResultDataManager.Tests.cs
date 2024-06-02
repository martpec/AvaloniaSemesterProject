using HeatProductionOptimization.Models;
using HeatProductionOptimization;

namespace OptimizerAvalonia.Tests
{
    public class ResultDataManagerTests
    {
        [Fact]
        public void SaveOptimizedData_GeneralTest()
        {
            // Arrange
            var optimizedData = new List<OptimizedData>
            {
                new OptimizedData
                {
                    StartTime = DateTime.Parse("2023-01-01T00:00:00"),
                    EndTime = DateTime.Parse("2023-01-01T01:00:00"),
                    TotalProductionCost = 100.0,
                    Emissions = 50.0,
                    BoilerProductions = new List<BoilerProduction>
                    {
                        new BoilerProduction { BoilerName = "Boiler1", HeatProduced = 100.0 }
                    }
                }
            };

            string tempDirectory = Path.GetTempPath();
            string filePath = Path.Combine(tempDirectory, "test_results.csv");
            var expectedContent = "StartTime,EndTime,TotalProductionCost,Emissions,ActivatedBoilers\r" +
                                  "\n01/01/2023 00:00:00,01/01/2023 01:00:00,100,50,Boiler1:100\r\n";

            // Act
            ResultDataManager.SaveOptimizedData(optimizedData, filePath);

            // Assert
            var actualContent = File.ReadAllText(filePath);
            Assert.Equal(expectedContent, actualContent);

            // Cleanup
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public void SaveOptimizedData_EmptyList()
        {
            // Arrange
            var optimizedData = new List<OptimizedData>();

            string tempDirectory = Path.GetTempPath();
            string filePath = Path.Combine(tempDirectory, "test_empty.csv");
            var expectedContent = "StartTime,EndTime,TotalProductionCost,Emissions,ActivatedBoilers\r\n";

            // Act
            ResultDataManager.SaveOptimizedData(optimizedData, filePath);

            // Assert
            var actualContent = File.ReadAllText(filePath);
            Assert.Equal(expectedContent, actualContent);

            // Cleanup
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public void SaveOptimizedData_NullFilePath()
        {
            // Arrange
            var optimizedData = new List<OptimizedData>
            {
                new OptimizedData
                {
                    StartTime = DateTime.Parse("2023-01-01T00:00:00"),
                    EndTime = DateTime.Parse("2023-01-01T01:00:00"),
                    TotalProductionCost = 100.0,
                    Emissions = 50.0,
                    BoilerProductions = new List<BoilerProduction>
                    {
                        new BoilerProduction { BoilerName = "Boiler1", HeatProduced = 100.0 }
                    }
                }
            };

            // Act and Assert
            var exception = Record.Exception(() => ResultDataManager.SaveOptimizedData(optimizedData, null));

            // We expect the method not to throw an exception because it doesn't currently handle this case
            Assert.Null(exception);
        }
    }
}