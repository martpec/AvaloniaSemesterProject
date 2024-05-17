using System.Reactive;
using ReactiveUI;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HeatProductionOptimization;
using HeatProductionOptimization.Interfaces;
using HeatProductionOptimization.Models;

namespace OptimizerAvalonia.ViewModels;


public partial class graphViewModel : ViewModelBase
{
    private readonly ObservableCollection<DateTimePoint> allBoilers;
    private readonly ObservableCollection<DateTimePoint> boiler1;
    private readonly ObservableCollection<DateTimePoint> boiler2;
    private readonly ObservableCollection<DateTimePoint> boiler3;
    private readonly ObservableCollection<DateTimePoint> boiler4;
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
    public graphViewModel()
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
            new LineSeries<DateTimePoint>
            {
                Name = "Total heat demand",
                Values = allBoilers,
                //Fill = null
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
        SourceDataManager sourceDataManager = new SourceDataManager();
        List<IBoiler> activeBoilers = new();

        for (int i = 0; i < IBoilersList.Count; i++)
        {
            if (BoilersList[i].IsActive)
            {
                IBoilersList[i].MaxHeat = BoilersList[i].HeatProduction;
                activeBoilers.Add(IBoilersList[i]);
            }
        }
        Optimizer optimizer = new Optimizer(activeBoilers, sourceDataManager.LoadSourceData(SourceDataPath));
        optimizer.CalculateOptimalHeatProduction(IsEmissions);

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
        foreach (OptimizedData data in OptimizedDataForGraph)
        {
            double totalHeatPerHour = 0.0;
            foreach (BoilerProduction boiler in data.BoilerProductions)
            {
                totalHeatPerHour += boiler.HeatProduced;
            }
            allBoilers.Add(new(data.StartTime, data.HeatDemand));
        }
    }

}