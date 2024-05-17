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
            Color = new SKColor(255,255,255), 
            //SKTypeface = SKTypeface.FromFamilyName("Courier New") 
        }; 
    public graphViewModel()
    {
        allBoilers = new ObservableCollection<DateTimePoint>
        {   // year, month, day, hour, minute, second
            new DateTimePoint(new DateTime(2021, 1, 1, 1, 0, 0), 3.3),
            new DateTimePoint(new DateTime(2021, 1, 1, 2, 0, 0), 13.4),
            new DateTimePoint(new DateTime(2021, 1, 1, 3, 0, 0), 6.6),
            new DateTimePoint(new DateTime(2021, 1, 1, 4, 0, 0), 8.1),
            new DateTimePoint(new DateTime(2021, 1, 1, 6, 0, 0), 3.9),
            new DateTimePoint(new DateTime(2021, 1, 1, 8, 0, 0), 10.7),
            new DateTimePoint(new DateTime(2021, 1, 1, 9, 0, 0), 15.3),
            new DateTimePoint(new DateTime(2021, 1, 1, 10, 0, 0), 9.2),
        };
        boiler1 = new ObservableCollection<DateTimePoint>
        {
            new DateTimePoint(new DateTime(2021, 1, 1, 1, 0, 0), 4.1),
            new DateTimePoint(new DateTime(2021, 1, 1, 2, 0, 0), 8.1),
            new DateTimePoint(new DateTime(2021, 1, 1, 3, 0, 0), 2.2),
            new DateTimePoint(new DateTime(2021, 1, 1, 4, 0, 0), 8.2),
            new DateTimePoint(new DateTime(2021, 1, 1, 6, 0, 0), 9.2),
            new DateTimePoint(new DateTime(2021, 1, 1, 8, 0, 0), 5.2),
            new DateTimePoint(new DateTime(2021, 1, 1, 9, 0, 0), 1.2)
        };
        boiler2 = new ObservableCollection<DateTimePoint>
        {
            new DateTimePoint(new DateTime(2021, 1, 1, 3, 0, 0), 2.2),
            new DateTimePoint(new DateTime(2021, 1, 1, 4, 0, 0), 2.2),
            new DateTimePoint(new DateTime(2021, 1, 1, 9, 0, 0), 1.2)
        };
        boiler3 = new ObservableCollection<DateTimePoint>
        {
            new DateTimePoint(new DateTime(2021, 1, 1, 9, 0, 0), 1.2)
        };
        boiler4 = new ObservableCollection<DateTimePoint>
        {
            new DateTimePoint(new DateTime(2021, 1, 1, 9, 0, 0), 1.2)
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
                Name = "Total heat producet",
                Values = allBoilers,
                //Fill = null
            },
        };
    }
    /*private int x = 13;
    public void AddItemCommand()
    {
        allBoilers.Add(new(++x,1));
    } */

    [RelayCommand]
    private void Optimize()
    {
        SourceDataManager SDM = new SourceDataManager();
        // TO DO: change IBoilersList
        Optimizer O = new Optimizer(IBoilersList, SDM.LoadSourceData(SourceDataPath));
        O.CalculateOptimalHeatProduction();
    }
}