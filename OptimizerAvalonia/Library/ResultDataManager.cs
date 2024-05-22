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
            string appDataPath = Path.Combine(@"C:\Users\fifof\OneDrive\Počítač\AvaloniaUI\SemesterProject2\OptimizerAvalonia\Library\AppData\Results", filePath);

            using (var writer = new StreamWriter(appDataPath))
            {
                writer.WriteLine("StartTime,EndTime,TotalProductionCost,Emissions,ActivatedBoilers");

                foreach (var data in optimizedData)
                {
                    string activatedBoilers = data.BoilerProductions != null 
                        ? string.Join(";", data.BoilerProductions.Select(b => $"{b.BoilerName}:{b.HeatProduced}")) 
                        : string.Empty;
                    writer.WriteLine($"{data.StartTime},{data.EndTime},{data.TotalProductionCost},{data.Emissions},{activatedBoilers}");
                }
            }
        }
    }
}