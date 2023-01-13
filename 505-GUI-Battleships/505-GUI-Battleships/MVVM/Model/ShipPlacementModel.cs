using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace _505_GUI_Battleships.MVVM.Model;

public class ShipPlacementModel
{
    public Point Position { get; set; }
    public int Length { get; set; }
    public bool Destroyed { get; set; }
    public bool Horizontal { get; set; }
    public Guid ShipModelId { get; }

    public ShipPlacementModel(int posX, int posY, bool horizontal, Guid shipModelId, int length)
    {
        Position = new Point(posX, posY);
        Horizontal = horizontal;
        ShipModelId = shipModelId;
        Length = length;
        Destroyed = false;
    }

    public Collection<Point> GetPoisitionList()
    {
        var positionList = new Collection<Point>();
        if ( Horizontal )
            for ( var i = 0; i < Length; i++ )
                positionList.Add(new Point(Position.X + i, Position.Y));
        else
            for ( var i = 0; i < Length; i++ )
                positionList.Add(new Point(Position.X, Position.Y + 1));
        return positionList;
    }

    public bool IsShipHit(Point shotPosition)
    {
        var positionChecker = Position;

        for ( var i = 0; i < Length; i++ )
        {
            if ( shotPosition == positionChecker )
                return true;

            if ( Horizontal )
                positionChecker.X += 1;
            else
                positionChecker.Y += 1;
        }

        return false;
    }

    public void Flip()
    {
        Horizontal = !Horizontal;
    }
}
