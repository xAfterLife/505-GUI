using System.Windows;

namespace _505_GUI_Battleships;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const double AspectRatio = 1.66666666667;

    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);

        if ( sizeInfo.HeightChanged )
            Width = sizeInfo.NewSize.Height * AspectRatio;

        if ( sizeInfo.WidthChanged )
            Height = sizeInfo.NewSize.Width / AspectRatio;
    }
}
