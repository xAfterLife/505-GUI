using System.Windows.Controls;
using _505_GUI_Battleships.Core;

namespace _505_GUI_Battleships.MVVM.Model;

public sealed class SelectTargetPlayerCardModel : ObservableObject
{
    private PlayerModel _player;

    private Canvas _playerBoard;

    private string _playerColor;

    private string _playerName;

    public PlayerModel Player { get; set; }

    public string PlayerColor { get; set; }

    public string PlayerName { get; set; }

    public Canvas PlayerBoard { get; set; }

    public SelectTargetPlayerCardModel(PlayerModel player, Canvas playerboard)
    {
        _player = player;
        _playerBoard = playerboard;
    }
}
