//-----------------------------------------------------------
//File:   Prog4.cs
//Desc:  This program is the battleship game in its most simplest form.
//-----------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Battleship
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        Game game;

        public Button[,] enemyShips;
        public Button[,] playerShips;
        public List<Button> recentlyHit;
        int size;

        public GameWindow(int size)
        {
            game = new Game(size);
            InitializeComponent();
            lblGameTimer.DataContext = game;
            lblGameTimer.SetBinding(TextBox.TextProperty, "Time");
            this.size = size;
            enemyShips = new Button[size, size];
            playerShips = new Button[size, size];
            recentlyHit = new List<Button>();
        }
        //Game timer that allows enemy to attack if player takes too long.
        public void StartGameTimer()
        {
            game.gameTimer = new DispatcherTimer();
            game.gameTimer.Interval = new TimeSpan(0, 0, 1);
            game.gameTimer.Tick += TimerTick;
            game.gameTimer.Start();
        }

        public void TimerTick(object sender, EventArgs e)
        {
            int time = --game.time;
            lblGameTimer.Text = time.ToString();

            if (game.time == 0)
            {
                game.gameTimer.Stop();
                game.time = 11;
                game.PlayerTurn = false;
                EnemyAttack();
            }
        }  
        
        //Sets enemy attack and gets the position and sets it to the appropiate color.
        public void EnemyAttack()
        {
            game.EnemyAttack();

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (game.PlayerBoard[row, col] == Game.PositionState.Hit)
                    {
                        playerShips[row, col].Background = Brushes.Red;
                        game.hit.Play();
                        
                        if (Game.playerShipsHit == 5)
                        {
                            game.gameTimer.Stop();

                            MessageBoxResult result = MessageBox.Show("The Enemy Won!");
                            if (result == MessageBoxResult.OK)
                            {
                                System.Environment.Exit(1);

                            }
                        }

                        recentlyHit.Add(playerShips[row, col]);
                        game.PlayerTurn = true;

                    }

                    if (game.PlayerBoard[row, col] == Game.PositionState.Missed)
                    {
                        playerShips[row, col].Background = Brushes.Blue;
                        recentlyHit.Add(playerShips[row, col]);
                        game.miss.Play();
                        game.PlayerTurn = true;
                    }
                    
                }
            }
            StartGameTimer();
        }
        //Enables players to attack.
        public void btnClick_Attack(object sender, RoutedEventArgs e)
        {     
            // Player Attacking
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (enemyShips[row, col] == sender as Button){
                        if (game.EnemyBoard[row, col] == Game.PositionState.Filled && game.PlayerTurn == true)
                        {
                            game.EnemyBoard[row, col] = Game.PositionState.Hit;
                            enemyShips[row, col].Background = Brushes.Red;
                            game.hit.Play();                         
                            Game.enemyShipsHit++;
                            game.PlayerTurn = false;
                            game.gameTimer.Stop();
                            game.time = 6;
                        }
                        else
                        {
                            game.EnemyBoard[row, col] = Game.PositionState.Missed;
                            enemyShips[row, col].Background = Brushes.Blue;
                            game.miss.Play();

                            game.PlayerTurn = false;
                            
                            game.gameTimer.Stop();
                            game.time = 6;
                        }
                    }
                    
                    if (recentlyHit.Contains(playerShips[row, col]) && playerShips[row, col].Background != Brushes.Black)
                    {
                        playerShips[row, col].Background = Brushes.Gray;
                        game.PlayerBoard[row, col] = Game.PositionState.Colorless;
                        
                    }
                    
                }
            }

            // Enemy Attacking
            WpfApp1.Model.TimedAction.ExecuteWithDelay(new Action(delegate { EnemyAttack(); }), TimeSpan.FromSeconds(2));

            //End game checking for player.
            if (Game.enemyShipsHit == 5)
            {
                game.gameTimer.Stop();
                MessageBoxResult result = MessageBox.Show("You Won!");
                if (result == MessageBoxResult.OK)
                {
                    System.Environment.Exit(1);                    
                }
            }
        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Start the game timer when the window opens
            StartGameTimer();

            //Player Board
            for (int row = 0; row < size; row++)
            {
                var horizontalPanel = new StackPanel();
                horizontalPanel.Orientation = Orientation.Horizontal;
                horizontalPanel.HorizontalAlignment = HorizontalAlignment.Center;
                panelPlayer.Children.Add(horizontalPanel);

                for (int col = 0; col < size; col++)
                {
                    playerShips[row, col] = new Button();

                    var button = playerShips[row, col];

                    button.Margin = new Thickness(5);
                    button.Padding = new Thickness(10);
                    button.Background = Brushes.Gray;
                    game.PlayerBoard[row, col] = Game.PositionState.Open;
                    horizontalPanel.Children.Add(button);
                }
            }

            // This section of code radomly sets filled positions and sets there color to black.
            game.PlacePlayerShips();        

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (game.isPlayerFilled(row, col))
                    {
                        playerShips[row, col].Background = Brushes.Black;
                    }
                }
            }
            // --------------

            //Enemy Board

            for (int row = 0; row < size; row++)
            {
                var horizontalPanel = new StackPanel();
                horizontalPanel.Orientation = Orientation.Horizontal;
                horizontalPanel.HorizontalAlignment = HorizontalAlignment.Center;
                panelEnemy.Children.Add(horizontalPanel);

                for (int col = 0; col < size; col++)
                {

                    enemyShips[row, col] = new Button();

                    enemyShips[row, col].Background = Brushes.Gray;
                    enemyShips[row, col].Margin = new Thickness(5);
                    enemyShips[row, col].Padding = new Thickness(10);
                    enemyShips[row, col].Click += btnClick_Attack;
                    game.EnemyBoard[row, col] = Game.PositionState.Open;
                    horizontalPanel.Children.Add(enemyShips[row, col]);
                }
            }

            // This section of code places the enemy ships and sets there color to black.


            game.PlaceEnemyShips();

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (game.isEnemyFilled(row, col) && Game.IsCheatOn == true) 
                    {
                        enemyShips[row, col].Background = Brushes.Black;

                    }
                }
            }

            
        }

        private void Window_Closing(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(1);
        }

    }
}
