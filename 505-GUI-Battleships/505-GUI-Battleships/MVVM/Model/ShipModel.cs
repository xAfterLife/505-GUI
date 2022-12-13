using System.Windows.Media;

namespace _505_GUI_Battleships.MVVM.Model;

internal class ShipModel
{
    public int Length { get; }
    public bool Horizontal { get; set; }
    public int ShipSelectionPosition { get; }
    public int Amount { get; }
    public ImageSource PathHorizontal { get; }
    public ImageSource PathVertival { get; }
    public int XPositionOnBoard { get; set; }
    public int YPositionOnBoard { get; set; }

    public ShipModel(int length, bool horizontal, int shipSelectionPosition, int amount, ImageSource pathHorizontal, ImageSource pathVertival)
    {
        Length = length;
        Horizontal = horizontal;
        ShipSelectionPosition = shipSelectionPosition;
        Amount = amount;
        PathHorizontal = pathHorizontal;
        PathVertival = pathVertival;
    }

    public void Flip()
    {
        Horizontal = !Horizontal;
    }

    public ImageSource UpdateImageSource()
    {
        if (Horizontal)
        {
            return PathHorizontal;
        } 
        else
        {
            return PathVertival;
        }
    }
}
