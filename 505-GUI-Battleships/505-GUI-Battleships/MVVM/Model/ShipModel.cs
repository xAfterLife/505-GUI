using System.Windows.Media;

namespace _505_GUI_Battleships.MVVM.Model;

internal class ShipModel
{
    public int Length { get; }
    public ImageSource PathHorizontal { get; }
    public ImageSource PathVertival { get; }

    public ShipModel(int length, ImageSource pathHorizontal, ImageSource pathVertival)
    {
        Length = length;
        PathHorizontal = pathHorizontal;
        PathVertival = pathVertival;
    }

    public ImageSource UpdateImageSource(bool horizontal)
    {
        return horizontal ? PathHorizontal : PathVertival;
    }
}
