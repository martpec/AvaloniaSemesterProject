using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.Defaults;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HeatProductionOptimization;
using HeatProductionOptimization.Interfaces;
using HeatProductionOptimization.Models;
using LiveChartsCore.SkiaSharpView.Painting;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using SkiaSharp;

namespace OptimizerAvalonia.ViewModels;

public partial class OptimizerViewModel : ViewModelBase
{
    [ObservableProperty] private bool _errorMessage = false;
    
    private readonly ObservableCollection<DateTimePoint> allBoilers;
    private readonly ObservableCollection<DateTimePoint> boiler1;
    private readonly ObservableCollection<DateTimePoint> boiler2;
    private readonly ObservableCollection<DateTimePoint> boiler3;
    private readonly ObservableCollection<DateTimePoint> boiler4;
    private readonly ObservableCollection<DateTimePoint> notEnough;
    
    
    public ObservableCollection<ISeries> Series { get; set; }
    public Axis[] XAxes { get; set; } =
    {
        new DateTimeAxis(TimeSpan.FromHours(1), date => date.ToString("H:mm d/M")) //output format
        {
            LabelsRotation = 45, // Rotate the labels by 45 degrees
            LabelsPaint = new SolidColorPaint(SKColors.White)
        }
    };

    public Axis[] YAxes { get; set; } =
    {
        new Axis
            {
                LabelsPaint = new SolidColorPaint(SKColors.White)
            }
    };

    public SolidColorPaint LegendColor { get; set; } =
        new SolidColorPaint
        {
            Color = new SKColor(255, 255, 255),
            //SKTypeface = SKTypeface.FromFamilyName("Courier New") 
        };
    public OptimizerViewModel()
    {

        allBoilers = new ObservableCollection<DateTimePoint>
        {   // year, month, day, hour, minute, second
            
        };
        boiler1 = new ObservableCollection<DateTimePoint>
        {

        };
        boiler2 = new ObservableCollection<DateTimePoint>
        {

        };
        boiler3 = new ObservableCollection<DateTimePoint>
        {

        };
        boiler4 = new ObservableCollection<DateTimePoint>
        {

        };
        notEnough = new ObservableCollection<DateTimePoint>
        {

        };
        Series = new ObservableCollection<ISeries>
        {
            
            new StackedColumnSeries<DateTimePoint>
            {
                Name = "Boiler 1",
                Values = boiler1,
                //Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0)) // Red color
                //Color = new SKColor(50, 50, 50),
            },
            new StackedColumnSeries<DateTimePoint>
            {
                Name = "Boiler 2",
                Values = boiler2
            },
            new StackedColumnSeries<DateTimePoint>
            {
                Name = "Boiler 3",
                Values = boiler3
            },
            new StackedColumnSeries<DateTimePoint>
            {
                Name = "Boiler 4",
                Values = boiler4
            },
            new StackedColumnSeries<DateTimePoint>
            {
                Name = "Missing heat",
                Values = notEnough
            },
            new LineSeries<DateTimePoint>
            {
                Name = "Total heat demand",
                Values = allBoilers,
                LineSmoothness = 0, // 0/1 change if line is smooth or not (UUUUUUUUU or VVVVVV) xd
                Fill = null
            },
        };

        loadDataMainGraph();
    }
    /*private int x = 13;
    public void AddItemCommand()
    {
        allBoilers.Add(new(++x,1));
    } */

    [RelayCommand]
    private void Optimize()
    {   
        fileName = string.Empty;
        _optimizedData = new List<OptimizedData>();
        
        SourceDataManager sourceDataManager = new SourceDataManager();
        List<IBoiler> activeBoilers = new();
        
        // loads the source data for optimization
        var dataForOptimization = sourceDataManager.LoadSourceData(SourceDataPath);
        
        // dates for the name of the exported file
        DateTime firstDate = dataForOptimization.Min(data => data.StartTime);
        DateTime lastDate = dataForOptimization.Max(data => data.EndTime);
        
        // change the exported file name according to emissons or cost
        fileName = IsEmissions ? $"OptimizedData_Emissions_{firstDate:yyyyMMdd}_{lastDate:yyyyMMdd}.csv" 
                                : $"OptimizedData_Cost_{firstDate:yyyyMMdd}_{lastDate:yyyyMMdd}.csv";
        
        
        for (int i = 0; i < IBoilersList.Count; i++)
        {
            if (BoilersList[i].IsActive)
            {
                IBoilersList[i].MaxHeat = BoilersList[i].HeatProduction;
                activeBoilers.Add(IBoilersList[i]);
            }
        }
        
        Optimizer optimizer = new Optimizer(activeBoilers, dataForOptimization);
        optimizer.CalculateOptimalHeatProduction(IsEmissions);
        
        _optimizedData =  optimizer.OptimizedData;
        OptimizedDataForGraph = optimizer.OptimizedData;

        allBoilers.Clear();
        boiler1.Clear();
        boiler2.Clear();
        boiler3.Clear();
        boiler4.Clear();
        loadDataMainGraph();
    }

    public void loadDataMainGraph()
    {
        ErrorMessage = false;
        ObservablePoints2.Clear();
        ObservablePoints3.Clear();
        notEnough.Clear();
        double totalDifference = 0;
        foreach (OptimizedData data in OptimizedDataForGraph)
        {
            double totalHeatPerHour = 0.0;
            totalDifference = data.HeatDemand;
            foreach (BoilerProduction boiler in data.BoilerProductions)
            {
                totalHeatPerHour += boiler.HeatProduced;
                totalDifference -= boiler.HeatProduced;
                
                if(boiler.BoilerName == "GB")
                    boiler1.Add(new(data.StartTime, boiler.HeatProduced));
                
                if(boiler.BoilerName == "OB")
                    boiler2.Add(new(data.StartTime, boiler.HeatProduced));

                if(boiler.BoilerName == "GM")
                    boiler3.Add(new(data.StartTime, boiler.HeatProduced));
                    
                if(boiler.BoilerName == "EK")
                    boiler4.Add(new(data.StartTime, boiler.HeatProduced));
                
            }
            if(totalDifference > 0)
            {
                notEnough.Add(new(data.StartTime, totalDifference));
            }
            
            ElectricityPoints.Add(new(data.StartTime, data.ElectricityPrice));
            allBoilers.Add(new(data.StartTime, data.HeatDemand));
            ObservablePoints2.Add(new (data.StartTime, data.TotalProductionCost));
            ObservablePoints3.Add(new (data.StartTime, data.Emissions));
        }
        if(totalDifference > 0)
        {   
            ErrorMessage = true;
        }
    }

    // exported file name
    private string fileName; 
    
    // list that holds optimized data in OptimizerViewModel 
    private List<OptimizedData> _optimizedData { get; set;  }
    
    [RelayCommand]
    private async Task SaveOptimizedDataToFile()
    {   
        // Check if _optimizedData is null or empty
        if (_optimizedData == null || !_optimizedData.Any())
        {
            Console.WriteLine("No optimized data to save.");
            return;
        }

        // Check if fileName is null or empty
        if (string.IsNullOrEmpty(fileName))
        {
            Console.WriteLine("File name is not set.");
            return;
        }

        try
        {
            ResultDataManager.SaveOptimizedData(_optimizedData, fileName);
            Console.WriteLine("Optimized data saved successfully.");
            
            var box = MessageBoxManager
                .GetMessageBoxStandard("Optimized Data Extracted", "Data has been saved to AppData/Results.",
                    ButtonEnum.Ok,
                    Icon.Folder
                    );
            
            var result = await box.ShowWindowAsync();
           
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving optimized data: {ex.Message}");
        }
    }
}