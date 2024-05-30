using System;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace OptimizerAvalonia.ViewModels;

public class ElectricityViewModel : ViewModelBase
{
    public ObservableCollection<ISeries> ElectricitySeries { get; set; }

    public Axis[] XAxes1 { get; set; } =
    [
        new DateTimeAxis(TimeSpan.FromHours(1), date => date.ToString("H:mm d/M")) //output format
        {
            LabelsRotation = 0, // Rotate the labels by 45 degrees
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

    public ElectricityViewModel()
    {
        ElectricitySeries =
        [
            new LineSeries<DateTimePoint>
            {
                Values = ElectricityPoints,
                LineSmoothness = 0, // 0/1 change if line is smooth or not (UUUUUUUUU or VVVVVV) 
                //Fill = new SolidColorPaint(SKColors.Red),
                //Stroke = new SolidColorPaint(SKColors.Red),
            }
        ];
    }
}