using System;
using System.Windows.Media;

namespace _505_GUI_Battleships.MVVM.Model;

internal class ShipModel
{
    /// <summary>
    ///     Id of the Current Ship Instance
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    ///     Length of the Ship
    /// </summary>
    public int Length { get; }

    /// <summary>
    ///     Image File for Horizontal presentation
    /// </summary>
    public ImageSource PathHorizontal { get; }

    /// <summary>
    ///     Image File for Vertical presentation
    /// </summary>
    public ImageSource PathVertival { get; }

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="length"></param>
    /// <param name="pathHorizontal"></param>
    /// <param name="pathVertival"></param>
    public ShipModel(int length, ImageSource pathHorizontal, ImageSource pathVertival)
    {
        Length = length;
        PathHorizontal = pathHorizontal;
        PathVertival = pathVertival;
        Id = Guid.NewGuid();
    }

    /// <summary>
    ///     Gives the Image to use back based on bool horizontal Input
    /// </summary>
    /// <param name="horizontal"></param>
    /// <returns></returns>
    public ImageSource UpdateImageSource(bool horizontal)
    {
        return horizontal ? PathHorizontal : PathVertival;
    }
}
