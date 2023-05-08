﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinTetris
{
    public partial class Form1 : Form
    {
        private Game _game;

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int key);

        bool spaceIsPressed = false;

        private Bitmap _bitField;
        private Bitmap _bitFigure;
        private Graphics _graphicsForField;
        private Graphics _graphicsForFigure;
        private Color _backColorForField = Color.Black;
        private Color _backColorForFigure = Color.White;

        public Form1()
        {
            InitializeComponent();

            //for fast next connections
            FirstConnectionToDataBase();
        }

        private void FirstConnectionToDataBase()
        {
            using (PlayersContext db = new PlayersContext())
            {
                db.Players.Load();
            }
        }

        #region Settings and records
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            //work with settings
            (int deep, int width, int speed) settings = WorkWithSettings.GetSettings();

            nudDeep.Value = settings.deep;
            nudWidth.Value = settings.width;
            nudSpeed.Value = settings.speed;

            //work with records
            List<Player> players = WorkWithRecords.GetPlayers();
            string records = RecordHandler.GetRecords(players);
            lblRecords.Text = records;
        }

        private void btnEditSettings_Click(object sender, EventArgs e)
        {
            int deep = Convert.ToInt32(nudDeep.Value);
            int width = Convert.ToInt32(nudWidth.Value);
            int speed = Convert.ToInt32(nudSpeed.Value);

            WorkWithSettings.SaveSettings(deep, width, speed);
        }
        #endregion 

        #region game
        private void btnChangeGameStatus_Click(object sender, EventArgs e)
        {
            string status = btnChangeGameStatus.Text;
            if (status.Equals(Constants.StatusGamePlay))
            {
                StartGame();
            }
            else
            {
                PauseGame();
            }
        }

        private void StopGame()
        {
            //stop game and 
            timer.Stop();
            KeyDown -= Form1_KeyDown;
            btnChangeGameStatus.Text = Constants.StatusGamePlay;
            btnStop.Enabled = false;

            //create new score and 
            AddNewRecordIfNeccessary();

            //reset images
            pctGameField.Image = null;
            pctNext.Image = null;
        }

        private void PauseGame()
        {
            timer.Stop();
            MessageBox.Show(Constants.TextForContinueGame, Constants.StatusGamePause);
            timer.Start();
        }

        private void StartGame()
        {
            _game = new Game();

            //general settings
            _bitField = new Bitmap(Constants.SizeInPixeles * _game.Width + 1, Constants.SizeInPixeles * _game.Deep + 1);
            _bitFigure = new Bitmap(Constants.SizeInPixeles * 3 + 1, Constants.SizeInPixeles * 4 + 1);
            _graphicsForField = Graphics.FromImage(_bitField);
            _graphicsForFigure = Graphics.FromImage(_bitFigure);

            //set events
            _game.ChangedGameField += ShowField;
            _game.ChangedNextFigure += ShowNextFigure;
            _game.ChangedScoreAndLines += ShowScoreAndLines;
            _game.OnEndOfGame += StopGame;
            KeyDown += Form1_KeyDown;

            _game.Start();
            timer.Interval = _game.Interval;
            timer.Start();

            btnChangeGameStatus.Text = Constants.StatusGamePause;
            btnStop.Enabled = true;
        }

        private void AddNewRecordIfNeccessary()
        {
            int score = Convert.ToInt32(lblRecord.Text);
            RecordHandler.AddRecord(score);
        }

        private void ShowScoreAndLines(int score, int lines)
        {
            lblLines.Text = lines.ToString();
            lblRecord.Text = score.ToString();
        }

        public void ShowNextFigure()
        {
            _graphicsForFigure.Clear(_backColorForFigure);
            int size = Constants.SizeInPixeles;

            //draw next figure
            Point[] figure = _game.NextFigure.Configuration;
            Brush color = _game.NextFigure.Color;

            for (int i = 0; i < 4; i++)
            {
                _graphicsForFigure.FillRectangle(color, figure[i].X * size, figure[i].Y * size, size, size);
                _graphicsForFigure.DrawRectangle(Pens.White, figure[i].X * size, figure[i].Y * size, size, size);
            }

            //show result
            pctNext.Image = _bitFigure;
        }

        public void ShowField()
        {
            _graphicsForField.Clear(_backColorForField);
            const int size = Constants.SizeInPixeles;
            Pen pensBackColor = Pens.Black;

            //draw game field
            for (int i = 0; i < _game.Deep; i++)
            {
                for (int j = 0; j < _game.Width; j++)
                {
                    TypeOfCell value = _game.GameField[i, j];
                    if (value < TypeOfCell.Static)
                    {
                        Brush colorr = ColorsByTypeOFCell.GetColor(value);
                        _graphicsForField.FillRectangle(colorr, j * size, i * size, size, size); 
                        _graphicsForField.DrawRectangle(pensBackColor, j * size, i * size, size, size);
                    }
                }
            }

            //draw falling figure
            Point[] figure = _game.CurrentFigure.Position;
            Brush color = _game.CurrentFigure.Color;

            for (int i = 0; i < 4; i++)
            {
                _graphicsForField.FillRectangle(color, figure[i].X * size, figure[i].Y * size, size, size);
                _graphicsForField.DrawRectangle(pensBackColor, figure[i].X * size, figure[i].Y * size, size, size);
            }

            //show result
            pctGameField.Image = _bitField;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                _game.CurrentFigure.Rotate(_game.GameField);
            }
            else if (e.KeyCode == Keys.A)
            {
                _game.CurrentFigure.MoveLeft(_game.GameField);
            }
            else if (e.KeyCode == Keys.S)
            {
                _game.MoveFigureDown(_game.GameField);
            }
            else if (e.KeyCode == Keys.D)
            {
                _game.CurrentFigure.MoveRight(_game.GameField);
            }
            else if (e.KeyCode == Keys.Space)
            {
                _game.MoveFigureDown(_game.GameField, true);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnChangeGameStatus.PerformClick();
            }
        }

        // Handle arrow keys (command keys)
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Up)
            {
                _game.CurrentFigure.Rotate(_game.GameField);
            }
            else if (keyData == Keys.Left)
            {
                _game.CurrentFigure.MoveLeft(_game.GameField);
            }
            else if (keyData == Keys.Down)
            {
                _game.MoveFigureDown(_game.GameField);
            }
            else if (keyData == Keys.Right)
            {
                _game.CurrentFigure.MoveRight(_game.GameField);
            }

            return true;
        }

        public void CheckKey()
        {
            try
            {
                while (true)
                {
                    if ((GetAsyncKeyState(32) & 0x8000) > 0)
                    {
                        spaceIsPressed = true;
                        _game.MoveFigureDown(_game.GameField, true);
                    }
                    else
                    {
                        if (spaceIsPressed)
                            spaceIsPressed = false;
                    }
                    Thread.Sleep(200);
                }
            }
            catch (ThreadInterruptedException ex) { }
        }

        private void timer_Tick(object sender, EventArgs e) => _game?.MoveFigureDown(_game.GameField);

        private void btnStop_Click(object sender, EventArgs e) => StopGame();
        #endregion game

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(Constants.ExitGameText, Constants.ExitGameCaption, MessageBoxButtons.YesNo);
            if(result != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }
    }
}
