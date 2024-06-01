using System.Globalization;
using System.IO;
using HeatProductionOptimization.Interfaces;

namespace HeatProductionOptimization;

public class AssetManager
{
    // Method to load data into boiler model from CSV file
    public T LoadBoilerData<T>(string filePath) where T : IBoiler, new()
    {
        T boiler = new T();

        string appDataPath = Path.Combine(@"Library\AppData", filePath);

        using (var reader = new StreamReader(appDataPath))
        {
            // Skip the first line
            reader.ReadLine();

            var line = reader.ReadLine();

#pragma warning disable CS8602 // Dereference of a possibly null reference.

            var values = line.Split(',');

            boiler.Name = values[0].Trim();
            boiler.MaxHeat = ExtractNumber(values[1]);
            boiler.ProductionCost = ExtractNumber(values[2]);

            boiler.SetAdditionalProperties(values);
        }

        return boiler;
    }

    /// <summary>
    /// Helper method to remove any non-numeric characters from a string and parse it to a double using regex
    /// </summary>
    /// <param name="input"></param>
    /// <returns>double</returns>
    public static double ExtractNumber(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return 0.0;
        
        string numberStr = System.Text.RegularExpressions.Regex.Match(input, @"-?\d+(\.\d+)?").Value;
        return double.Parse(numberStr, CultureInfo.InvariantCulture);
    }
}