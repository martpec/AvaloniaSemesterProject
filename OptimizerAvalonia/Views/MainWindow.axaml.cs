using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media;
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

        //SplashScreen = new ComplexSplashScreen();
    }
}

internal class ComplexSplashScreen(string? appName, IImage? appIcon, int minimumShowTime) : IApplicationSplashScreen
{
    public string? AppName { get; } = appName;
    public IImage? AppIcon { get; } = appIcon;
    public object SplashScreenContent { get; } = new DemoComplexSplashScreen();

    // To avoid too quickly transitioning away from the splash screen, you can set a minimum
    // time to hold before loading the content, value is in Milliseconds
    public int MinimumShowTime { get; set; } = minimumShowTime;


    // Place your loading tasks here. NOTE, this is already called on a background thread, so
    // if any UI thread work needs to be done, use Dispatcher.UIThread.Post or .InvokeAsync
    public async Task RunTasks(CancellationToken token)
    {
        await ((DemoComplexSplashScreen)SplashScreenContent).InitApp();
    }
}