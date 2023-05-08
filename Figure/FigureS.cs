using System.Drawing;

namespace WinTetris
{
    public class FigureS : Figure
    {
        public FigureS(int startColumn)
        {
            Configuration = new Point[4] {
            new Point(1,1),
            new Point(0,1),
            new Point(2,0),
            new Point(1,0)
        };

            Color = Brushes.Pink;
            TypeOfCell = TypeOfCell.StaticS;
            FindStartPosition(startColumn);
        }
    }
}