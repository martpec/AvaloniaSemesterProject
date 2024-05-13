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
namespace OptimizerAvalonia.ViewModels;

public class graphViewModel: ViewModelBase
{
    private readonly ObservableCollection<ObservablePoint> _observablePoints;
    public ObservableCollection<ISeries> Series { get; set; }
    public graphViewModel()
    {
        _observablePoints = new ObservableCollection<ObservablePoint>
        {
            new ObservablePoint(1,0),
            new(2,1.2),
            new(3,2.2),
            new(4,3.2),
            new(5,4.2),
            new(6,5.2),
            new(7,6.2),
            new(8,7.2),
            new(9,8.2),
            new(10,9.2),
            new(11,10.2),
            new(12,11.2),
            new(13,50.2)
        };

        Series = new ObservableCollection<ISeries>
        {
            new LineSeries<ObservablePoint>
            {
                Values = _observablePoints,
                Fill = null
            }
        };
    }
    private int x = 13;
    public void AddItemCommand()
    {
        _observablePoints.Add(new(++x,1));
    }   
}