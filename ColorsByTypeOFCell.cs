using System.Drawing;
using System.Linq;

namespace WinTetris
{
    public static class ColorsByTypeOFCell
    {
        /// <summary>
        /// find brush for draw by type of cell
        /// </summary>
        /// <param name="cell">current type of cell</param>
        /// <returns>brush for draw</returns>
        public static Brush GetColor(TypeOfCell cell)
        {
            Figure[] figures = new Figure[] { new FigureI(0), new FigureO(0), new FigureT(0),
            new FigureS(0), new FigureZ(0), new FigureL(0), new FigureJ(0)};
            return figures.FirstOrDefault(figure => figure.TypeOfCell == cell).Color;
        }
    }
}
