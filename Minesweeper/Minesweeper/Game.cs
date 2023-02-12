using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Rectangle = System.Windows.Shapes.Rectangle;
using System.Windows;

namespace Minesweeper
{
    class Game
    {
        Case[,] grid;
        bool win;
        bool lose;
        ImageBrush[] numbersImg;
        ImageBrush mineImg;
        ImageBrush flagImg;
        ImageBrush defaultImg;
        ImageBrush explodedImg;
        ImageBrush crossImg;
        bool generated;
        Label mainLabel;
        Label remainingLabel;
        int remainingMines;
        int target;
        int revealed;
        double minesRate;
        ComboBox select;

        public Game(int squareSize, int paddingTop, int width, int height, Label label, Label rmn, ComboBox sct = null)
        {
            minesRate = 0.03;
            mainLabel = label;
            select = sct;
            mainLabel.Content = "Click to start a game";
            remainingLabel = rmn;
            remainingLabel.Content = "0";
            generated = false;
            win = false;
            lose = false;

            // Images
            numbersImg = new ImageBrush[9];
            for (int i = 0; i < numbersImg.Length; i++)
                numbersImg[i] = new ImageBrush(new BitmapImage(new Uri($"pack://application:,,,/images/N_{i}.png", UriKind.Absolute)));
            mineImg = new ImageBrush(new BitmapImage(new Uri($"pack://application:,,,/images/Mine.png", UriKind.Absolute)));
            flagImg = new ImageBrush(new BitmapImage(new Uri($"pack://application:,,,/images/Flag.png", UriKind.Absolute)));
            defaultImg = new ImageBrush(new BitmapImage(new Uri($"pack://application:,,,/images/Default.png", UriKind.Absolute)));
            explodedImg = new ImageBrush(new BitmapImage(new Uri($"pack://application:,,,/images/Exploded.png", UriKind.Absolute)));
            crossImg = new ImageBrush(new BitmapImage(new Uri($"pack://application:,,,/images/Cross.png", UriKind.Absolute)));

            // Grid
            grid = new Case[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Height = 30;
                    rect.Width = 30;
                    Canvas.SetLeft(rect, 2 + i * (squareSize + 2));
                    Canvas.SetTop(rect, 2 + j * (squareSize + 2) + 0); // No padding top this time
                    rect.Fill = defaultImg;
                    rect.Tag = new int[] { i, j };
                    rect.MouseLeftButtonUp += OnCaseClick;
                    rect.MouseRightButtonUp += OnCaseRightClick;
                    grid[i, j] = new Case(rect);
                }
            }

            // No mines yet
        }
        
        void GenerateMines(int sX, int sY)
        {
            mainLabel.Content = "";

            // Generated on first click
            int width = this.grid.GetLength(0);
            int height = this.grid.GetLength(1);

            // Generating mines
            Random rnd = new Random();
            if(select != null) minesRate = (5.0 + select.SelectedIndex * 5.0) / 100.0;
            int nbMines = (int)(height * width * minesRate);
            remainingMines = nbMines;
            target = height * width - nbMines;
            revealed = 0;
            remainingLabel.Content = remainingMines;
            while(nbMines > 0)
            {
                int a = rnd.Next(width);
                int b = rnd.Next(height);
                if(!grid[a, b].HasMine && !(a == sX & b == sY) && !( Math.Abs(a - sX) < 2 && Math.Abs(b - sY) < 2))
                {
                    grid[a, b].HasMine = true;
                    nbMines--;
                }
            }
        }

        void OnCaseRightClick(object sender, MouseButtonEventArgs e)
        {
            if (generated && !win && !lose)
            {
                Rectangle rec = (Rectangle)sender;
                int[] coord = (int[])rec.Tag;
                int x = coord[0];
                int y = coord[1];
                if(!grid[x, y].IsRevealed)
                {
                    if(grid[x, y].IsFlagged)
                    {
                        grid[x, y].IsFlagged = false;
                        grid[x, y].Display.Fill = defaultImg;
                        remainingMines++;
                    } else
                    {
                        grid[x, y].IsFlagged = true;
                        grid[x, y].Display.Fill = flagImg;
                        remainingMines--;
                    }
                    remainingLabel.Content = remainingMines;
                }
            }
        }
        void OnCaseClick(object sender, MouseButtonEventArgs e)
        {
            Rectangle rec = (Rectangle)sender;
            int[] coord = (int[])rec.Tag;
            int x = coord[0];
            int y = coord[1];

            if (!generated)
            {
                GenerateMines(x, y);
                generated = true;
            }

            if(!grid[x, y].IsFlagged && !grid[x, y].IsRevealed && !win & !lose)
            {
                if(grid[x, y].HasMine)
                {
                    // Perdu
                    lose = true;
                    grid[x, y].Display.Fill = explodedImg;
                    for(int i = 0; i < grid.GetLength(0); i++)
                    {
                        for(int j = 0; j < grid.GetLength(1); j++)
                        {
                            
                            if (grid[i, j].HasMine && !grid[i, j].IsFlagged && !(i == x && j == y))
                            {
                                grid[i, j].Display.Fill = mineImg;
                            }
                            if(!grid[i, j].HasMine && grid[i, j].IsFlagged)
                            {
                                grid[i, j].Display.Fill = crossImg;
                            }
                        }
                    }
                    mainLabel.Content = "You lose";
                    MessageBox.Show("You lose, better luck next time!", "Minesweeper", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                } else{
                    UpdateMines(x, y);
                }
            }
        }

        void UpdateMines(int x, int y)
        {
            int c = 0;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i >= 0 && j >= 0 && i < grid.GetLength(0) && j < grid.GetLength(1) && !(i == x && j == y))
                    {
                        // On compte les mines autour
                        if (grid[i, j].HasMine) c++;
                    }
                }
            }
            // Updating current case
            grid[x, y].Display.Fill = numbersImg[c];
            grid[x, y].IsRevealed = true;
            revealed++;

            if(c == 0)
            {
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {

                        if (i >= 0 && j >= 0 && i < grid.GetLength(0) && j < grid.GetLength(1) && !(i == x && j == y))
                        {
                            if (!grid[i, j].IsRevealed) UpdateMines(i, j);
                        }
                    }
                }
            }

            if(revealed == target)
            {
                win = true;
                mainLabel.Content = "Congratulations!";
                MessageBox.Show("Congratulations, you win!", "Minesweeper", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        public Case[,] Grid
        {
            get => grid;
        }
    }
}
