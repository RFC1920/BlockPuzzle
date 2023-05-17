using System.Drawing;

namespace BlockPuzzle
{
    public class FigureI : Figure
    {
        public FigureI(int startColumn)
        {
            Configuration = new Point[4] {
            new Point(1,1),
            new Point(1,0),
            new Point(1,2),
            new Point(1,3)
        };

            Color = Brushes.Yellow;
            TypeOfCell = TypeOfCell.StaticI;
            FindStartPosition(startColumn);
        }
    }
}
