using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTetris
{
    public abstract class Figure
    {
        public delegate void MoveFigure();
        public event MoveFigure ChangeFigurePosition;

        public Point[] Configuration { get; protected set; }
        public TypeOfCell TypeOfCell { get; protected set; }
        public Point[] Position { get; protected set; }
        public Brush Color { get; protected set; }

        /// <summary>
        /// find start position of figure on the ggame field
        /// </summary>
        /// <param name="width">width of game field</param>
        protected void FindStartPosition(int startColumn)
        {
            Position = new Point[4]
            {
            new Point(Configuration[0].X + startColumn, Configuration[0].Y),
            new Point(Configuration[1].X + startColumn, Configuration[1].Y),
            new Point(Configuration[2].X + startColumn, Configuration[2].Y),
            new Point(Configuration[3].X + startColumn, Configuration[3].Y)
            };
        }

        /// <summary>
        /// check if the figure has correct position on the game field
        /// </summary>
        /// <param name="gameField">game field</param>
        /// <returns>true if the current position is correct</returns>
        public bool IsCorrect(TypeOfCell[,] gameField)
        {
            int deep = gameField.GetUpperBound(0) + 1;
            int width = gameField.Length / deep;

            //check for mistake of value or shape
            foreach (Point point in Position)
            {
                if ((point.X >= width) || (point.X < 0) ||
                    (point.Y >= deep) || gameField[point.Y, point.X] < TypeOfCell.Static)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Move figure down
        /// </summary>
        /// <param name="gameField">game field</param>
        /// <returns>true if figure was descendeed</returns>
        public bool MoveDown(TypeOfCell[,] gameField)
        {
            for (int index = 0; index < Position.Length; index++)
            {
                Position[index].Y++;
            }

            if (!IsCorrect(gameField))
            {
                for (int index = 0; index < Position.Length; index++)
                {
                    Position[index].Y--;
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// rotate figure by 90 degree
        /// </summary>
        /// <param name="gameField">game field</param>
        public void Rotate(TypeOfCell[,] gameField)
        {
            //check for square
            if (TypeOfCell == TypeOfCell.StaticO)
            {
                return;
            }

            //copy array
            Point[] position = new Point[4];
            Array.Copy(Position, position, Position.Length);

            //find new points
            for (int index = 1; index < 4; index++)
            {
                Point point = FindNewShift(Position[0], Position[index]);
                Position[index] = point;
            }

            //if the final figure can't be entered in game filed then reset
            if (!IsCorrect(gameField))
            {
                Array.Copy(position, Position, Position.Length);
            }
            else
            {
                ChangeFigurePosition.Invoke();
            }
        }

        /// <summary>
        /// find new point for block of figure
        /// </summary>
        /// <param name="centralPoint">point of central block</param>
        /// <param name="point">point of a current block</param>
        /// <returns>new point of the current block</returns>
        private static Point FindNewShift(Point centralPoint, Point point)
        {
            //replace by rule: d_left => d_top; d_top => -d_left; rule from rotate by 90 deg
            //find delta
            int deltaLeftShift = centralPoint.X - point.X;
            int deltaTopShift = centralPoint.Y - point.Y;

            //get new left and top shifts 
            int leftShift = centralPoint.X + deltaTopShift;
            int topShift = centralPoint.Y - deltaLeftShift;

            return new Point(leftShift, topShift);
        }

        /// <summary>
        /// Move figure left
        /// </summary>
        /// <param name="gameField">game field</param>
        public void MoveLeft(TypeOfCell[,] gameField)
        {
            for (int index = 0; index < Position.Length; index++)
            {
                Position[index].X--;
            }

            if (!IsCorrect(gameField))
            {
                for (int index = 0; index < Position.Length; index++)
                {
                    Position[index].X++;
                }
            }
            else
            {
                ChangeFigurePosition.Invoke();
            }
        }

        /// <summary>
        /// Move figure right
        /// </summary>
        /// <param name="gameField">game field</param>
        public void MoveRight(TypeOfCell[,] gameField)
        {
            for (int index = 0; index < Position.Length; index++)
            {
                Position[index].X++;
            }

            if (!IsCorrect(gameField))
            {
                for (int index = 0; index < Position.Length; index++)
                {
                    Position[index].X--;
                }
            }
            else
            {
                ChangeFigurePosition.Invoke();
            }
        }
    }
}