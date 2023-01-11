using System;
using System.Windows.Media;

namespace _505_GUI_Battleships.MVVM.Model;

internal class ShipModel
{
    public Guid Id { get; }
    public int Length { get; }
    public ImageSource PathHorizontal { get; }
    public ImageSource PathVertival { get; }

    public ShipModel(int length, ImageSource pathHorizontal, ImageSource pathVertival)
    {
        Length = length;
        PathHorizontal = pathHorizontal;
        PathVertival = pathVertival;
        Id = Guid.NewGuid();
    }

    public ImageSource UpdateImageSource(bool horizontal)
    {
        return horizontal ? PathHorizontal : PathVertival;
    }
}
