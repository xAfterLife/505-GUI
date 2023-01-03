namespace _505_GUI_Battleships.MVVM.Model;

public class ShipPlacementModel
{
    public int PosX { get; set; }
    public int PosY { get; set; }
    public bool Horizontal { get; set; }

    public ShipPlacementModel(int posX, int posY, bool horizontal)
    {
        PosX = posX;
        PosY = posY;
        Horizontal = horizontal;
    }

    public void Flip()
    {
        Horizontal = !Horizontal;
    }
}
