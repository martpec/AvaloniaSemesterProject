using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HeatProductionOptimization.Models;

namespace HeatProductionOptimization
{
    public class ResultDataManager
    {
        public static void SaveOptimizedData(List<OptimizedData> optimizedData, string filePath)
        {
            string appDataPath = Path.Combine(Environment.CurrentDirectory, "AppData", filePath);

            using (var writer = new StreamWriter(appDataPath))
            {
                writer.WriteLine("StartTime,EndTime,TotalProductionCost,Emissions,ActivatedBoilers");

                foreach (var data in optimizedData)
                {
                    string activatedBoilers = data.ActivatedBoilers != null ? string.Join(";", data.ActivatedBoilers.Select(b => b.Name)) : string.Empty;
                    writer.WriteLine($"{data.StartTime},{data.EndTime},{data.TotalProductionCost},{data.Emissions},{activatedBoilers}");
                }
            }
        }
    }
}