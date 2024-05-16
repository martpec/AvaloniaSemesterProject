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
    private readonly ObservableCollection<DateTimePoint> _observablePoints;
    public ObservableCollection<ISeries> Series { get; set; }
    public Axis[] XAxes { get; set; } =
    {
        new DateTimeAxis(TimeSpan.FromHours(1), date => date.ToString("H:mm d/M")) //output format
        {
            LabelsRotation = 45 // Rotate the labels by 45 degrees
        }
    };
    public graphViewModel()
    {
        _observablePoints = new ObservableCollection<DateTimePoint>
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

        Series = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Values = _observablePoints,
                Fill = null
            }
        };
    }
    /*private int x = 13;
    public void AddItemCommand()
    {
        _observablePoints.Add(new(++x,1));
    } */

    [RelayCommand]
    private void Optimize()
    {
        SourceDataManager SDM = new SourceDataManager();
        // TO DO: change IBoilersList
        Optimizer O = new Optimizer(IBoilersList,SDM.LoadSourceData(SourceDataPath));
        O.CalculateOptimalHeatProduction();
    }
}