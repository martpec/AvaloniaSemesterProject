﻿using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OptimizerAvalonia.ViewModels;

public partial class SettingsViewModel: ViewModelBase
{
    
    private readonly string _summerPath = "SummerData.csv";
    private readonly string _winterPath = "WinterData.csv";
    
    [ObservableProperty]
    private bool _isWinter;

    [RelayCommand]
    private void TriggerButton()
    {
        if (IsWinter)
        {
            SourceDataPath = _winterPath;
        }
        else
        {
            SourceDataPath = _summerPath; 
        }
    }
/*--------------Emissions/Cost----------------*/
    private bool isEmissions;

    public bool IsEmissions
    {
        get => isEmissions;
        set => SetProperty(ref isEmissions, value);
    }
    
}