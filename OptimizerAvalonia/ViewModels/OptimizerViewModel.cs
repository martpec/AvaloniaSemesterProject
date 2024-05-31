using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HeatProductionOptimization;
using HeatProductionOptimization.Interfaces;
using HeatProductionOptimization.Models;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using SkiaSharp;

namespace OptimizerAvalonia.ViewModels;

public partial class OptimizerViewModel : ViewModelBase
{
    [ObservableProperty] private bool _errorMessage;

    private readonly ObservableCollection<DateTimePoint> _allBoilers;
    private readonly ObservableCollection<DateTimePoint> _gasBoiler;
    private readonly ObservableCollection<DateTimePoint> _oilBoiler;
    private readonly ObservableCollection<DateTimePoint> _gasMotor;
    private readonly ObservableCollection<DateTimePoint> _electricBoiler;
    private readonly ObservableCollection<DateTimePoint> _notEnough;


    public ObservableCollection<ISeries> MainGraphSeries { get; set; }

    public Axis[] XAxes { get; set; } =
    [
        new DateTimeAxis(TimeSpan.FromHours(1), date => date.ToString("H:mm d/M")) //output format
        {
            LabelsRotation = 45, // Rotate the labels by 45 degrees
            LabelsPaint = new SolidColorPaint(SKColors.White)
        }
    ];

    public Axis[] YAxes { get; set; } =
    [
        new Axis
        {
            LabelsPaint = new SolidColorPaint(SKColors.White)
        }
    ];

    // Color font for the legend
    public SolidColorPaint LegendColor { get; set; } =
        new SolidColorPaint
        {
            Color = new SKColor(255, 255, 255),
            //SKTypeface = SKTypeface.FromFamilyName("Courier New") 
        };

    // exported file name
    private string? _fileName;

    // list that holds optimized data in OptimizerViewModel 
    private List<OptimizedData>? OptimizedData { get; set; }

    public OptimizerViewModel()
    {
        _allBoilers = [];
        _gasBoiler = [];
        _oilBoiler = [];
        _gasMotor = [];
        _electricBoiler = [];
        _notEnough = [];
        MainGraphSeries =
        [
            new StackedColumnSeries<DateTimePoint>
            {
                Name = "GB",
                Values = _gasBoiler,
            },

            new StackedColumnSeries<DateTimePoint>
            {
                Name = "OB",
                Values = _oilBoiler
            },

            new StackedColumnSeries<DateTimePoint>
            {
                Name = "GM",
                Values = _gasMotor
            },

            new StackedColumnSeries<DateTimePoint>
            {
                Name = "EK",
                Values = _electricBoiler
            },

            new StackedColumnSeries<DateTimePoint>
            {
                Name = "Missing heat",
                Values = _notEnough
            },

            new LineSeries<DateTimePoint>
            {
                Name = "Total heat demand",
                Values = _allBoilers,
                LineSmoothness = 0,
                Fill = null
            }

        ];

        LoadDataMainGraph();
    }

    [RelayCommand]
    private void Optimize()
    {
        _fileName = string.Empty;
        OptimizedData = [];

        SourceDataManager sourceDataManager = new SourceDataManager();
        List<IBoiler> activeBoilers = [];

        // loads the source data for optimization
        var sourceDataForOptimization = sourceDataManager.LoadSourceData(SourceDataPath);

        // dates for the name of the exported file
        DateTime firstDate = sourceDataForOptimization.Min(data => data.StartTime);
        DateTime lastDate = sourceDataForOptimization.Max(data => data.EndTime);

        // change the exported file name according to emissions or cost
        _fileName = IsEmissions
            ? $"OptimizedData_Emissions_{firstDate:yyyyMMdd}_{lastDate:yyyyMMdd}.csv"
            : $"OptimizedData_Cost_{firstDate:yyyyMMdd}_{lastDate:yyyyMMdd}.csv";

        // Loading the data of the boilers
        for (int i = 0; i < ListOfIBoilers.Count; i++)
        {
            if (BoilersList[i].IsActive)
            {
                ListOfIBoilers[i].MaxHeat = BoilersList[i].HeatProduction;
                activeBoilers.Add(ListOfIBoilers[i]);
            }
        }
        // Calling constructor of optimizer & optimize data
        Optimizer optimizer = new Optimizer(activeBoilers, sourceDataForOptimization);
        optimizer.CalculateOptimalHeatProduction(IsEmissions);

        OptimizedData = optimizer.OptimizedData;
        OptimizedDataForGraph = optimizer.OptimizedData;
        
        // Clear every graph before filling them in
        _allBoilers.Clear();
        _gasBoiler.Clear();
        _oilBoiler.Clear();
        _gasMotor.Clear();
        _electricBoiler.Clear();

        // Fill the graphs with data
        LoadDataMainGraph();
    }

    private void LoadDataMainGraph()
    {
        ErrorMessage = false; // Resets error message showing
        CostsPoints.Clear();
        EmissionsPoints.Clear();
        ElectricityPoints.Clear();
        _notEnough.Clear();
        double totalDifference = 0;
        foreach (OptimizedData data in OptimizedDataForGraph)
        {
            totalDifference = data.HeatDemand;
            if (data.BoilerProductions != null)
                // Adds data about each boiler to the graph
                foreach (BoilerProduction boiler in data.BoilerProductions)
                {
                    totalDifference -= boiler.HeatProduced;

                    if (boiler.BoilerName == "GB")
                        _gasBoiler.Add(new(data.StartTime, boiler.HeatProduced));

                    if (boiler.BoilerName == "OB")
                        _oilBoiler.Add(new(data.StartTime, boiler.HeatProduced));

                    if (boiler.BoilerName == "GM")
                        _gasMotor.Add(new(data.StartTime, boiler.HeatProduced));

                    if (boiler.BoilerName == "EK")
                        _electricBoiler.Add(new(data.StartTime, boiler.HeatProduced));
                }
            // If there was not enough heat produced, this will display on the graph how much heat was missing
            if (totalDifference > 0)
            {
                _notEnough.Add(new(data.StartTime, totalDifference));
            }

            // Fill in other graphs
            ElectricityPoints.Add(new(data.StartTime, data.ElectricityPrice));
            _allBoilers.Add(new(data.StartTime, data.HeatDemand));
            CostsPoints.Add(new(data.StartTime, data.TotalProductionCost));
            EmissionsPoints.Add(new(data.StartTime, data.Emissions));
        }
        // Show error message if there was not enough heat produced
        if (totalDifference > 0)
        {
            ErrorMessage = true;
        }
    }


    [RelayCommand]
    private async Task SaveOptimizedDataToFile()
    {
        // Check if OptimizedData is null or empty
        if (OptimizedData == null || !OptimizedData.Any())
        {
            Console.WriteLine("No optimized data to save.");
            return;
        }

        // Check if _fileName is null or empty
        if (string.IsNullOrEmpty(_fileName))
        {
            Console.WriteLine("File name is not set.");
            return;
        }

        try
        {
            // Saves the optimized data to a file
            ResultDataManager.SaveOptimizedData(OptimizedData, _fileName);
            Console.WriteLine("Optimized data saved successfully.");
            // Lets user know it saved the data correctly and successfully
            var box = MessageBoxManager
                .GetMessageBoxStandard("Optimized Data Extracted", "Data has been saved to AppData/Results.",
                    ButtonEnum.Ok,
                    Icon.Folder
                );

            await box.ShowWindowAsync();
        }
        catch (Exception ex) // Catches any errors
        {
            Console.WriteLine($"An error occurred while saving optimized data: {ex.Message}");
        }
    }
}