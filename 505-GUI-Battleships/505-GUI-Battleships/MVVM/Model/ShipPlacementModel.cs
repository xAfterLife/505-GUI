using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using Point = System.Windows.Point;

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

    public bool IsShipHit(Point shotPosition)
    {
        Point positionChecker = Position;
        for (int i = 0; i < Length; i++)
        {
            if (shotPosition == positionChecker)
                return true;

            if (Horizontal == true)
            {
                positionChecker.X += 1;
            } else
            {
                positionChecker.Y += 1;
            }
        }
        return false;
    }

    public void Flip()
    {
        Horizontal = !Horizontal;
    }
}
