using System.Windows.Media;

namespace _505_GUI_Battleships.MVVM.Model;

internal class Ship
{
    public int Length { get; }
    public bool Horizontal { get; set; }
    public int ShipSelectionPosition { get; }
    public int Amount { get; }
    public ImageSource Path { get; }
    public int XPositionOnBoard { get; set; }
    public int YPositionOnBoard { get; set; }

    public Ship(int length, bool horizontal, int shipSelectionPosition, int amount, ImageSource path)
    {
        Length = length;
        Horizontal = horizontal;
        ShipSelectionPosition = shipSelectionPosition;
        Amount = amount;
        Path = path;
    }

    public void Flip()
    {
        Horizontal = !Horizontal;
    }
}
