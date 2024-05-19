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

    public Axis[] YAxes { get; set; } =
    {
        new Axis
        {
            LabelsPaint = new SolidColorPaint(SKColors.White)
        }
    };

    public ObservableCollection<ISeries> Series2 { get; set; }

    public Axis[] XAxes2 { get; set; } =
    {
        new DateTimeAxis(TimeSpan.FromHours(1), date => date.ToString("H:mm d/M yyyy")) //output format
        {
            LabelsPaint = new SolidColorPaint(SKColors.White)
        }
    };

    public CostsViewModel()
    {
        
        Series2 = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Values = ObservablePoints2,
                Fill = null
            }
        };
    }
}