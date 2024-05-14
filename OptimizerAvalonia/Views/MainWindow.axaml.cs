using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using FluentAvalonia.UI.Windowing;
using OptimizerAvalonia.SplashScreen;

namespace OptimizerAvalonia.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {   
        InitializeComponent();
        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
        
        SplashScreen = new ComplexSplashScreen();
        
    }
}

internal class ComplexSplashScreen : IApplicationSplashScreen
{
    public ComplexSplashScreen()
    {
        SplashScreenContent = new DemoComplexSplashScreen();
    }

    public string AppName { get; }
    public IImage AppIcon { get; }
    public object SplashScreenContent { get; }

    // To avoid too quickly transitioning away from the splash screen, you can set a minimum
    // time to hold before loading the content, value is in Milliseconds
    public int MinimumShowTime { get; set; }


    // Place your loading tasks here. NOTE, this is already called on a background thread, so
    // if any UI thread work needs to be done, use Dispatcher.UIThread.Post or .InvokeAsync
    public async Task RunTasks(CancellationToken token)
    {
        await ((DemoComplexSplashScreen)SplashScreenContent).InitApp();
    }
}