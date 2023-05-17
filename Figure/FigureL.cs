using System.Drawing;

namespace BlockPuzzle
{
    public class FigureL : Figure
    {
        public FigureL(int startColumn)
        {
            Configuration = new Point[4] {
            new Point(1,1),
            new Point(1,0),
            new Point(1,2),
            new Point(2,2)
        };

            Color = Brushes.Blue;
            TypeOfCell = TypeOfCell.StaticL;
            FindStartPosition(startColumn);
        }
    }
}