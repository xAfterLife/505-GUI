using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _505_GUI_Battleships.MVVM.Model
{
    internal class Mode
    {
        public int PlayerCount;
        public int[][] BoardDimensions;
        public int? Rounds;
        public int ShipSquareCount;
        public int GameMode;

        public Mode(int playerCount, int[][] boardDimensions, int? rounds, int shipSquareCount, int gameMode)
        {
            PlayerCount = playerCount;
            BoardDimensions = boardDimensions;
            Rounds = rounds;
            ShipSquareCount = shipSquareCount;
            GameMode = gameMode;
        }
    }
}