using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.Defaults;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using Avalonia.Media;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace OptimizerAvalonia.ViewModels;

public class OptimizerViewModel : ViewModelBase
{
    // Graph 1-> money
    public ObservableCollection<ISeries> Series1 { get; set; }

    public Axis[] XAxes1 { get; set; } =
    {
        new DateTimeAxis(TimeSpan.FromHours(1), date => date.ToString("H:mm d/M")) //output format
        {
            LabelsRotation = 0, // Rotate the labels by 45 degrees
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

    public ObservableCollection<ISeries> Series2 { get; set; }

    public Axis[] XAxes2 { get; set; } =
    {
        new DateTimeAxis(TimeSpan.FromHours(1), date => date.ToString("H:mm d/M yyyy")) //output format
        {
            LabelsPaint = new SolidColorPaint(SKColors.White)
        }
    };

    public OptimizerViewModel()
    {
        
        Series1 = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Values = ObservablePoints3,
                //Fill = null
            },
        };
        
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