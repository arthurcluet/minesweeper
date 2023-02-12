using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Image = System.Drawing.Image;
using Rectangle = System.Windows.Shapes.Rectangle;
using System.Diagnostics;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Game game;
        Button newGame;
        Label remaining;
        Label mainLabel;
        ComboBox select;
        const int height = 20;
        const int width = 20;
        const int squareSize = 30;
        const int paddingTop = 40;

        public MainWindow()
        {
            InitializeComponent();

            // Window
            mainWindowPp.Height = (squareSize + 2) * height + 45 + paddingTop;
            mainWindowPp.Width = (squareSize + 2) * width + 30;

            topBar.Height = paddingTop;
            topBar.Width = (squareSize + 2) * width + 2;

            mainDisplay.Height = (squareSize + 2) * height + 2;
            mainDisplay.Width = (squareSize + 2) * width + 2;

            // New game button
            newGame = new Button();
            newGame.Content = "New game";
            newGame.Height = 30;
            newGame.Width = 100;
            newGame.Click += NewGameButtonClick;
            Canvas.SetLeft(newGame, 0);
            Canvas.SetTop(newGame, 5);

            // Labels
            remaining = new Label();
            remaining.Content = "0";
            remaining.Height = 30;
            remaining.VerticalAlignment = VerticalAlignment.Center;
            remaining.FontSize = 16;
            Canvas.SetRight(remaining, 0);
            Canvas.SetTop(remaining, 5);

            mainLabel = new Label();
            mainLabel.Content = "Click to start a game";
            mainLabel.Height = 30;
            mainLabel.Width = 400;
            mainLabel.VerticalContentAlignment = VerticalAlignment.Center;
            mainLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
            mainLabel.FontSize = 16;
            Canvas.SetTop(mainLabel, 5);
            Canvas.SetLeft(mainLabel, (mainDisplay.Width - 400) / 2);

            // Form
            select = new ComboBox();
            select.Items.Add("5%");
            select.Items.Add("10%");
            select.Items.Add("15%");
            select.Items.Add("20%");
            select.Items.Add("25%");
            select.Width = 60;
            select.Height = 30;
            select.VerticalContentAlignment = VerticalAlignment.Center;
            select.SelectedIndex = 3;
            Canvas.SetTop(select, 5);
            Canvas.SetLeft(select, 105);

            // Adding elements again
            topBar.Children.Add(newGame);
            topBar.Children.Add(mainLabel);
            topBar.Children.Add(remaining);
            topBar.Children.Add(select);


            // Background
            SolidColorBrush backgroundColor = new SolidColorBrush();
            backgroundColor.Color = System.Windows.Media.Color.FromArgb(255, 239, 239, 239);

            // Setting window background
            mainDisplay.Background = backgroundColor;

            NewGameButtonClick();
        }


        private void NewGameButtonClick(object sender = null, RoutedEventArgs e = null)
        {
            mainDisplay.Children.Clear();
            // Creating game
            
            game = new Game(squareSize, paddingTop, width, height, mainLabel, remaining, select);
            // Updating display
            for (int i = 0; i < game.Grid.GetLength(0); i++)
            {
                for (int j = 0; j < game.Grid.GetLength(1); j++)
                {
                    mainDisplay.Children.Add(game.Grid[i, j].Display);
                }
            }
            
        } 
    }
}
