using System.Windows.Media;

namespace _505_GUI_Battleships.MVVM.Model;

internal class ShipModel
{
    public int Length { get; }
    public bool Horizontal { get; set; }
    public ImageSource PathHorizontal { get; }
    public ImageSource PathVertival { get; }

    public ShipModel(int length, bool horizontal, ImageSource pathHorizontal, ImageSource pathVertival)
    {
        Length = length;
        Horizontal = horizontal;
        PathHorizontal = pathHorizontal;
        PathVertival = pathVertival;
    }

    public void Flip()
    {
        Horizontal = !Horizontal;
    }

    public ImageSource UpdateImageSource()
    {
        return Horizontal ? PathHorizontal : PathVertival;
    }
}
