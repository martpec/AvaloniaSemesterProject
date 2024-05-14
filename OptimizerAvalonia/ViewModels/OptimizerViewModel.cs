using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.Defaults;
using System;
using System.Collections.ObjectModel;

namespace OptimizerAvalonia.ViewModels;
public class OptimizerViewModel : ViewModelBase
{
    // Graph 1-> money
    private readonly ObservableCollection<DateTimePoint> _observablePoints1;
    public ObservableCollection<ISeries> Series1 { get; set; }
    public Axis[] XAxes1 { get; set; } =
    {
        new DateTimeAxis(TimeSpan.FromHours(1), date => date.ToString("H:mm d/M yyyy")) //output format
    };
    private readonly ObservableCollection<DateTimePoint> _observablePoints2;
    public ObservableCollection<ISeries> Series2 { get; set; }
    public Axis[] XAxes2 { get; set; } =
    {
        new DateTimeAxis(TimeSpan.FromHours(1), date => date.ToString("H:mm d/M yyyy")) //output format
    };

    public OptimizerViewModel()
    {
        _observablePoints1 = new ObservableCollection<DateTimePoint>
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

        Series1 = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Values = _observablePoints1,
                Fill = null
            }
        };

        _observablePoints2 = new ObservableCollection<DateTimePoint>
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

        Series2 = new ObservableCollection<ISeries>
        {
            new LineSeries<DateTimePoint>
            {
                Values = _observablePoints2,
                Fill = null
            }
        };
    }
}