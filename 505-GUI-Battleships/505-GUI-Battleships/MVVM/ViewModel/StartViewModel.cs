using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.Services;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal class StartViewModel : ObservableObject, IDisposable
{
    /// <summary>
    ///     Command to Change the CurrentView to the PlayerSelection
    /// </summary>
    public ICommand StartGameCommand => new RelayCommand(_ => ChangeViewModel.ChangeView(ChangeViewModel.ViewType.PlayerSelection, this));

    public ICommand SoundTestCommand => new RelayCommand(_ =>
    {
        SoundPlayerService.PlaySound(SoundPlayerService.SoundType.Geschoss);
        Task.Delay(1000);
        SoundPlayerService.PlaySound(SoundPlayerService.SoundType.Wassertreffer);
    });

    /// <summary>
    ///     Command to Exit the Current Application
    /// </summary>
    public static ICommand ExitCommand => new RelayCommand(_ => Application.Current.Shutdown());

    public void Dispose() {}
}
