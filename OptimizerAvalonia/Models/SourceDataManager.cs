using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using HeatProductionOptimization.Models;

namespace HeatProductionOptimization
{
    public class SourceDataManager
    {
        public List<SourceData> LoadSourceData(string filePath)
        {
            List<SourceData> sourceDataList = new List<SourceData>();
            
            string currentDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;

            string appDataPath = Path.Combine(projectDirectory, "Models\\AppData", filePath);
            
            try
            {
                using var reader = new StreamReader(appDataPath);
                // Skip first three lines of headings
                for (int i = 0; i < 3; i++)
                {
                    reader.ReadLine();
                }

                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');

                    DateTime startTime = ParseDateTime(parts[0]);
                    DateTime endTime = ParseDateTime(parts[1]);
                    double heatDemand = double.Parse(parts[2].Trim());
                    double electricityPrice = double.Parse(parts[3].Trim());

                    sourceDataList.Add(new SourceData
                    {
                        StartTime = startTime,
                        EndTime = endTime,
                        HeatDemand = heatDemand,
                        ElectricityPrice = electricityPrice
                    });
                }

                foreach (var sourceData in sourceDataList)
                {
                    Console.WriteLine($"Start time: {sourceData.StartTime}, End time: {sourceData.EndTime}, Heat demand: {sourceData.HeatDemand}, Electricity price: {sourceData.ElectricityPrice}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading source data: {ex.Message}"); // Catching exceptions and printing error message
            }

            return sourceDataList;
        }

        /// <summary>
        /// Helper method to parse a datetime string
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <returns>DateTime</returns>
        private DateTime ParseDateTime(string dateTimeString)
        {
            // "M/d/yyyy H:mm" is the format of the datetime in the csv file
            return DateTime.ParseExact(dateTimeString.Trim(), "M/d/yyyy H:mm", CultureInfo.InvariantCulture);
        }

    }
}