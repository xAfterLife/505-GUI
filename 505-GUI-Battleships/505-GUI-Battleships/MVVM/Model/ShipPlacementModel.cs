namespace _505_GUI_Battleships.MVVM.Model;

public class ShipPlacementModel
{
    public int PosX { get; set; }
    public int PosY { get; set; }
    public bool Flipped { get; set; }

    public ShipPlacementModel(int posX, int posY, bool flipped)
    {
        PosX = posX;
        PosY = posY;
        Flipped = flipped;
    }
}
