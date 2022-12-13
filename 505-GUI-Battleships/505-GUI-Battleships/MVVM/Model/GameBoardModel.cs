using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _505_GUI_Battleships.MVVM.Model;

public class GameBoardModel
{
    public int Height { get; }
    public int Width { get; }
    public Canvas Board { get; }

    public GameBoardModel(int height = 10, int width = 10)
    {
        Height = height;
        Width = width;

        Board = new Canvas
        {
            Width = width,
            Height = height,
            ClipToBounds = true,
            AllowDrop = true,
            LayoutTransform = new ScaleTransform(1, -1)
        };

        // https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.tilebrush.viewbox?view=windowsdesktop-7.0
        DrawingBrush brush = new()
        {
            TileMode = TileMode.Tile,
            Viewbox = new Rect(0, 0, 1, 1),
            Viewport = new Rect(0, 0, 1, 1),
            ViewboxUnits = BrushMappingMode.Absolute,
            ViewportUnits = BrushMappingMode.Absolute,
            Drawing = new GeometryDrawing { Pen = new Pen { Thickness = 0.1, Brush = Brushes.DarkGreen }, Geometry = new RectangleGeometry(new Rect(0, 0, 1, 1)) }
        };

        Board.Background = brush;
    }

    private void DrawBoard() {}
}
