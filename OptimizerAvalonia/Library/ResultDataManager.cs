using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using HeatProductionOptimization.Models;

namespace HeatProductionOptimization;

public class ResultDataManager
{
    public static void SaveOptimizedData(List<OptimizedData> optimizedData, string filePath)
    {
        try
        {
            // Get the current base directory
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Console.WriteLine($"Current base directory: {baseDirectory}");

            // Move three directories up
            DirectoryInfo directoryInfo = new DirectoryInfo(baseDirectory);
            for (int i = 0; i < 3; i++)
            {
                if (directoryInfo.Parent != null)
                {
                    directoryInfo = directoryInfo.Parent;
                }
                else
                {
                    throw new InvalidOperationException("Cannot move up three directories from the base directory.");
                }
            }

            string parentDirectory = directoryInfo.FullName;
            Console.WriteLine($"Parent directory after moving up three levels: {parentDirectory}");

            // Construct the absolute path to the file
            string appDataPath = Path.Combine(parentDirectory, "Library", "AppData", "Results", filePath);
            Console.WriteLine($"Absolute file path: {appDataPath}");

            // Extract directory path and ensure it exists
            string? directoryPath = Path.GetDirectoryName(appDataPath);

            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine($"Directory created: {directoryPath}");
            }
            else if (string.IsNullOrEmpty(directoryPath))
            {
                Console.WriteLine("Directory path is null or empty.");
            }
            else
            {
                Console.WriteLine($"Directory already exists: {directoryPath}");
            }

            // Confirm the final file path
            Console.WriteLine($"File path: {appDataPath}");

            // Check directory and file creation explicitly
            if (Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Confirmed directory exists: {directoryPath}");

                // Open a StreamWriter to the file
                using (var writer = new StreamWriter(appDataPath))
                {
                    writer.WriteLine("StartTime,EndTime,TotalProductionCost,Emissions,ActivatedBoilers");

                    // Write each data item to the file
                    foreach (var data in optimizedData)
                    {
                        string activatedBoilers = data.BoilerProductions != null
                            ? string.Join(";",
                                data.BoilerProductions.Select(b =>
                                    $"{b.BoilerName}:{b.HeatProduced.ToString(CultureInfo.InvariantCulture)}"))
                            : string.Empty;
                        writer.WriteLine(
                            $"{data.StartTime},{data.EndTime},{data.TotalProductionCost.ToString(CultureInfo.InvariantCulture)},{data.Emissions.ToString(CultureInfo.InvariantCulture)},{activatedBoilers}");
                    }
                }

                Console.WriteLine("Data successfully saved.");
            }
            else
            {
                Console.WriteLine("Failed to confirm directory creation.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data: {ex.Message}");
        }
    }
}