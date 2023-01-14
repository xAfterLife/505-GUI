using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace _505_GUI_Battleships.MVVM.Model;

public class ShipPlacementModel
{
    /// <summary>
    ///     Position of the placed Ship
    /// </summary>
    public Point Position { get; set; }

    /// <summary>
    ///     Length of the placed Ship (needed Duplicate of ShipModel.Length)
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    ///     Is the Ship already destroyed? Used for Visibility
    /// </summary>
    public bool Destroyed { get; set; }

    /// <summary>
    ///     bool Ship Horizontal position
    /// </summary>
    public bool Horizontal { get; set; }

    /// <summary>
    ///     ShipModelId for easier selection
    /// </summary>
    public Guid ShipModelId { get; }

    /// <summary>
    ///     Constructor of ShipPlacementModel
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="horizontal"></param>
    /// <param name="shipModelId"></param>
    /// <param name="length"></param>
    public ShipPlacementModel(int posX, int posY, bool horizontal, Guid shipModelId, int length)
    {
        Position = new Point(posX, posY);
        Horizontal = horizontal;
        ShipModelId = shipModelId;
        Length = length;
        Destroyed = false;
    }

    /// <summary>
    ///     Get the Positions of the Ship (Position and Length in the current Direction)
    /// </summary>
    /// <returns></returns>
    public Collection<Point> GetPoisitionList()
    {
        var positionList = new Collection<Point>();
        for ( var i = 0; i < Length; i++ )
            //With Keyword takes the Position and extends/alters it
            positionList.Add(Horizontal ? Position with { X = Position.X + i } : Position with { Y = Position.Y + 1 });
        return positionList;
    }

    /// <summary>
    ///     Did the Attack hit the Ship?
    /// </summary>
    /// <param name="shotPosition">Attacks Position</param>
    /// <returns></returns>
    public bool IsShipHit(Point shotPosition)
    {
        return GetPoisitionList().Contains(shotPosition);
    }

    /// <summary>
    ///     Negates the Horizontal bool
    /// </summary>
    public void Flip()
    {
        Horizontal = !Horizontal;
    }
}
