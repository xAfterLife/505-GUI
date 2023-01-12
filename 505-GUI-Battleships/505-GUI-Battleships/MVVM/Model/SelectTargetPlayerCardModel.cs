using _505_GUI_Battleships.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace _505_GUI_Battleships.MVVM.Model;

public sealed class SelectTargetPlayerCardModel : ObservableObject
{
    private PlayerModel _player;

    public PlayerModel Player
    {
        get; set;
    }
   
    private string _playerColor;
    public string PlayerColor
    {
        get; set;
    }

    private string _playerName;
    public string PlayerName
    {
        get; set;
    }


    private Canvas _playerBoard;
    public Canvas PlayerBoard
    {
        get; set;
    }

    public SelectTargetPlayerCardModel(PlayerModel player, Canvas playerboard)
    {
        _player = player;
        _playerBoard = playerboard;
    }
}
