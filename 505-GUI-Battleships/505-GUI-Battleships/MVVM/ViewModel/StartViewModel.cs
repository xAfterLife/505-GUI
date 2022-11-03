using System.Windows.Input;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal class StartViewModel : ObservableObject
{
    public static ICommand DummyViewModelCommand => new RelayCommand(_ => ChangeViewModel.ChangeView(typeof(DummyViewModel)));
}
