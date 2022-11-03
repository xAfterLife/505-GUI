using System.Windows;
using _505_GUI_Battleships.MVVM.ViewModel;

namespace _505_GUI_Battleships;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}
