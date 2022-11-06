using System.Collections.ObjectModel;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal class PlayerConfigurationViewModel : ObservableObject
{
    public ObservableCollection<PlayerModel> Players { get; set; }

    public PlayerConfigurationViewModel()
    {
        Players = new ObservableCollection<PlayerModel> { new("Test 123", 69), new("Teeees", 420), new("Mustermann", 1337) };
    }
}
