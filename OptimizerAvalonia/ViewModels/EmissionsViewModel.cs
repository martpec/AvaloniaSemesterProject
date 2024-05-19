using System;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace OptimizerAvalonia.ViewModels;

public class EmissionsViewModel : ViewModelBase
{
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
    
    public EmissionsViewModel()
    {
        Series1 = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Values = ObservablePoints3,
                LineSmoothness = 0 // 0/1 change if line is smooth or not (UUUUUUUUU or VVVVVV) xd
                //Fill = null
            }
        };
    }
}