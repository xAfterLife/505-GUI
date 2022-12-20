namespace _505_GUI_Battleships.MVVM.Model;

public class PlayerAttackModel
{
    public int EnemyId { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }

    public PlayerAttackModel(int enemyId, int posX, int posY)
    {
        EnemyId = enemyId;
        PosX = posX;
        PosY = posY;
    }
}
