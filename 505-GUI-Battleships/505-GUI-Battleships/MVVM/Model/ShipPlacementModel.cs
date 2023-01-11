using System;

namespace _505_GUI_Battleships.MVVM.Model;

public class ShipPlacementModel
{
    public int PosX { get; set; }
    public int PosY { get; set; }
    public bool Horizontal { get; set; }
    public Guid ShipModelId { get; }

    public ShipPlacementModel(int posX, int posY, bool horizontal, Guid shipModelId)
    {
        PosX = posX;
        PosY = posY;
        Horizontal = horizontal;
        ShipModelId = shipModelId;
    }

    public void Flip()
    {
        Horizontal = !Horizontal;
    }
}
