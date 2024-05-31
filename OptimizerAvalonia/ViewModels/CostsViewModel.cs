using System;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace OptimizerAvalonia.ViewModels;

public class CostsViewModel : ViewModelBase
{
    // Graph 1-> money

    // DataSeries for Graph object
    public ObservableCollection<ISeries> CostsSeries { get; set; }

    // x axis properties
    public Axis[] XAxes { get; set; } =
    [
        new DateTimeAxis(TimeSpan.FromHours(1), date => date.ToString("H:mm d/M yyyy")) //output format
        {
            LabelsPaint = new SolidColorPaint(SKColors.White)
        }
    ];

    // y axis properties
    public Axis[] YAxes { get; set; } =
    [
        new Axis
        {
            LabelsPaint = new SolidColorPaint(SKColors.White)
        }
    ];



    public CostsViewModel()
    {
        // Creates the graph
        CostsSeries = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Values = CostsPoints,
                LineSmoothness = 0 // 0/1 change if line is smooth or not (UUUUUUUUU or VVVVVV) 
                //Fill = null
            }
        };
    }
}