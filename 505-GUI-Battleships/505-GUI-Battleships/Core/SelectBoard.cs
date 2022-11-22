using _505_GUI_Battleships.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace _505_GUI_Battleships.Core
{
    public sealed class SelectBoard
    {
        public void DragBoardDrop(object sender, DragEventArgs e, Canvas playerBoard)
        {
            //object data = e.Data.GetData(image);
            Image element = (Image)e.Data.GetData(typeof(Image));

            //if (data is Canvas element)
            {
                Point dropPosition = e.GetPosition(playerBoard);
                dropPosition.X = Math.Round(dropPosition.X - 0.5);
                dropPosition.Y = Math.Round(dropPosition.Y - 0.5);
                if (dropPosition.X + element.Width > 10)
                {
                    Canvas.SetLeft(element, 10 - element.Width);
                }
                else
                {
                    Canvas.SetLeft(element, dropPosition.X);
                }
                if (dropPosition.Y + element.Height > 10)
                {
                    Canvas.SetTop(element, 10 - element.Height);
                }
                else
                {
                    Canvas.SetTop(element, dropPosition.Y);
                }

                Trace.WriteLine(dropPosition);
            }
        }

        public void ShipMouseMove(object sender, MouseEventArgs e, Image ship)
        {
            //object data = e.Data

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                System.Windows.DragDrop.DoDragDrop(ship, ship, DragDropEffects.Move);
            }
        }

        /*public void RightClickFlip(object sender, MouseButtonEventArgs e, Image ship, Ship shipData, Canvas playerBoard)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                Trace.WriteLine("Rightclick Pressed");
                int length = shipData.GetLength();
                if (!shipData.IsHorizontal())
                {
                    shipData.Flip();
                    ship.Width = length;
                    ship.Height = 1;
                    double xPos = e.GetPosition(playerBoard).X;
                    if (xPos + length > 10)
                    {
                        Canvas.SetLeft(ship, 10 - length);
                    }
                }
                else
                {
                    shipData.Flip();
                    ship.Width = 1;
                    ship.Height = length;
                    double yPos = e.GetPosition(playerBoard).Y;
                    if (yPos + length > 10)
                    {
                        Canvas.SetTop(ship, 10 - length);
                    }
                }
            }

        }*/
    }
}
