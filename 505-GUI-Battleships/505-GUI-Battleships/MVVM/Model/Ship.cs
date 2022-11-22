using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace _505_GUI_Battleships.MVVM.Model;

internal class Ship
{
    int length { get; }
    bool horizontal { get; set; }
    int shipSelectionPosition { get; }
    int amount { get; }
    ImageSource path { get; }
    int xPositionOnBoard { get; set; }
    int yPositionOnBoard { get; set; }

    public Ship(int length, bool horizontal, int shipSelectionPosition, int amount, ImageSource path)
    {
        this.length = length;
        this.horizontal = horizontal;
        this.shipSelectionPosition = shipSelectionPosition;
        this.amount = amount;
        this.path = path;
    }

    public bool IsHorizontal()
    {
        return horizontal;
    }

    public void Flip()
    {
        horizontal = !horizontal;
    }

    public int GetLength()
    {
        return length;
    }

    public ImageSource GetPath()
    {
        return path;
    }
}
