using System;
using System.Linq;
using System.Threading;

namespace WinTetris
{
    public class Game
    {
        public delegate void ShowGameElements();
        public delegate void EndOfGame();
        public delegate void ShowScoreAndLines(int score, int lines);

        public event EndOfGame OnEndOfGame;
        public event ShowGameElements ChangedGameField;
        public event ShowGameElements ChangedNextFigure;
        public event ShowScoreAndLines ChangedScoreAndLines;

        private int _startColumn;
        private int _score;
        private int _lines;

        public TypeOfCell[,] GameField { get; }
        public Figure CurrentFigure { get; private set; }
        public Figure NextFigure { get; private set; }
        public int Width { get; }
        public int Depth { get; }
        public int Interval { get; }

        public Game()
        {
            (int depth, int width, int speed) = WorkWithSettings.GetSettings();

            Depth = depth;
            Width = width;
            Interval = Convert.ToInt32(Math.Round(Constants.MilisecondsInAMinute / speed));

            GameField = new TypeOfCell[Depth, Width];
            _startColumn = Convert.ToInt32(Math.Round((double)Width / 2) - 1);
        }

        /// <summary>
        /// Create first figures couple and show them
        /// </summary>
        public void Start()
        {
            //NextFigure = FigureGenerator.Generate(_startColumn);
            Random random = new Random();
            int generatedValue = random.Next(1, Width);
            NextFigure = FigureGenerator.Generate(generatedValue);

            Thread.Sleep(25); //because computer generates two of the same figure without it
            CurrentFigure = FigureGenerator.Generate(_startColumn);

            CurrentFigure.ChangeFigurePosition += InvokeRefresh;

            //invoke 
            ChangedGameField.Invoke();
            ChangedNextFigure.Invoke();
        }

        /// <summary>
        /// move figure down generate new one and finish the game
        /// </summary>
        public void MoveFigureDown(TypeOfCell[,] gameField)
        {
            //move figure down by one
            bool isFall = CurrentFigure.MoveDown(GameField);
            if (!isFall)
            {
                //fill the field with the current figure
                foreach (Point point in CurrentFigure.Position)
                {
                    GameField[point.Y, point.X] = CurrentFigure.TypeOfCell;
                }

                //check for fill rows
                CheckForFilledRows();

                //change next and current figures, show it
                CurrentFigure = NextFigure;
                CurrentFigure.ChangeFigurePosition += InvokeRefresh;
                NextFigure = FigureGenerator.Generate(_startColumn);

                ChangedNextFigure.Invoke();
                ChangedGameField.Invoke();

                //check for end of game(can't insert figure in game field)
                bool isEnd = CurrentFigure.IsCorrect(GameField);
                if (!isEnd)
                {
                    OnEndOfGame.Invoke();
                }
            }

            ChangedGameField.Invoke();
        }

        /// <summary>
        /// check on filled rows and refill game field
        /// </summary>
        private void CheckForFilledRows()
        {
            int[] filledRows = FindFilledRows();

            //find count of filled rows and exit if not any row was filled
            int countOfFilledRow = filledRows.Count(row => row != -1);
            if (countOfFilledRow == 0)
            {
                return;
            }
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.chimes);
            player.Play();

            RefreshGameFieldAfterFilledRows(filledRows, countOfFilledRow);
            AddScoreAndLines(countOfFilledRow);
        }

        /// <summary>
        /// Add score and lines count
        /// </summary>
        /// <param name="countOfFilledRow">count of filled row in the game filed</param>
        private void AddScoreAndLines(int countOfFilledRow)
        {
            _lines += countOfFilledRow;
            _score += countOfFilledRow == 4 ? Constants.PointsForTetris : countOfFilledRow * Constants.PointsForARow;
            ChangedScoreAndLines.Invoke(_score, _lines);
        }

        /// <summary>
        /// find indexes of filled rows
        /// </summary>
        /// <returns>array of filled rows indexes</returns>
        private int[] FindFilledRows()
        {
            int[] filledRow = new int[4] { -1, -1, -1, -1 };
            int index = 0;

            for (int row = Depth - 1; row > 0; row--)
            {
                bool isFilled = true;
                bool isEmpty = true;

                for (int column = 0; column < Width; column++)
                {
                    //check for filled row
                    if (GameField[row, column] == TypeOfCell.Empty)
                    {
                        isFilled = false;
                    }

                    //check for empty row
                    if (GameField[row, column] < TypeOfCell.Static)
                    {
                        isEmpty = false;
                    }
                }

                //reset data aboute filled rows
                if (isFilled)
                {
                    filledRow[index++] = row;
                }

                //exit if we will not can meet filled row
                if (isEmpty || (filledRow[0] - 3 >= row && filledRow[0] >= 0))
                {
                    return filledRow;
                }
            }

            return filledRow;
        }

        /// <summary>
        /// refill game field after filled some rows
        /// </summary>
        /// <param name="filledRows">indexes of filled rows</param>
        /// <param name="countOfFilledRow">count of filled rows</param>
        private void RefreshGameFieldAfterFilledRows(int[] filledRows, int countOfFilledRow)
        {
            int shift = 1;
            int index = 0;

            //shift all rows from the first filled to top end of fields
            for (int row = filledRows[index]; row >= countOfFilledRow; row--)
            {
                //check for skip if the next row is filled
                while (index != 3 && filledRows[index + 1] == row - shift)
                {
                    //inc shift 
                    if (filledRows[index + 1] == row - shift)
                    {
                        index++;
                        shift++;
                    }
                }

                //shift
                for (int column = 0; column < Width; column++)
                {
                    GameField[row, column] = GameField[row - shift, column];
                }
            }

            AddEmptyRowToTheTop(countOfFilledRow);
        }

        /// <summary>
        /// fill top of game field with empty rows
        /// </summary>
        /// <param name="countOfFilledRow">count of needed rows</param>
        private void AddEmptyRowToTheTop(int countOfFilledRow)
        {
            for (int row = 0; row < countOfFilledRow; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    GameField[row, column] = TypeOfCell.Empty;
                }
            }
        }

        public void InvokeRefresh() => ChangedGameField.Invoke();
    }
}