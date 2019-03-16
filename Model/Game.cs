//-----------------------------------------------------------
//File:   Prog4.cs
//Desc:  This program is the battleship game in its most simplest form.
//-----------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace Battleship
{
    public class Game
    {
        public List<ImageSource> images;

        public DispatcherTimer gameTimer;
        public int time = 6;

        public int size;

        public static int playerShipsHit = 0;
        public static int enemyShipsHit = 0;

        public static bool isCheatOn = true;
        public  bool playerTurn = true;
        public static bool resetColor = false;

        public SoundPlayer hit = new SoundPlayer(WpfApp1.Properties.Resources.explosion);
        public SoundPlayer miss = new SoundPlayer(WpfApp1.Properties.Resources.miss);

        static Random rand = new Random();

        public enum PositionState { Open, Hit, Filled, Missed, Colorless }

        public PositionState[,] playerBoard;
        public PositionState[,] enemyBoard;
        public PositionState[,] PlayerBoard { get { return playerBoard; } set { playerBoard = value; } }
        public PositionState[,] EnemyBoard
        {
            get { return enemyBoard; }
            set { enemyBoard = value; }
        }

        public static Random Random { get { return rand; } }
        
        public static bool IsCheatOn { get { return isCheatOn; } set { isCheatOn = value; } }
        public bool PlayerTurn { get { return playerTurn; } set { playerTurn = value; } }
        public static bool ResetColor { get { return resetColor; } set { resetColor = value; } }
        public int Size { get { return size; } set { size = value; } }
        public string Time { get { return time.ToString(); } set { time = Convert.ToInt32(value); } }
        public Game(int s)
        {
            Size = s;
            playerBoard = new PositionState[s, s];
            enemyBoard = new PositionState[s, s];  
        }

        //Updates the 2d array of position states.
        public void EnemyAttack()
        {
            int row = Random.Next(size);
            int col = Random.Next(size);

            while(PlayerBoard[row, col] != PositionState.Hit || PlayerBoard[row, col] != PositionState.Missed)
            {
                row = Random.Next(size);
                col = Random.Next(size);
                if (PlayerBoard[row, col] == PositionState.Open)
                {
                    PlayerBoard[row, col] = PositionState.Missed;
                    break;
                }
                if(PlayerBoard[row, col] == PositionState.Filled)
                {
                    PlayerBoard[row, col] = PositionState.Hit;
                    Game.playerShipsHit++;
                    break;
                }
            }
            PlayerTurn = true;
        }

        //Places enemy positions in enemy randomly
        public void PlaceEnemyShips()
        {

            for (int i = 0; i < 5; i++)
            {
                int row;
                int col;
                do
                {

                    row = Random.Next(size);
                    col = Random.Next(size);
                } while (enemyBoard[row, col] != PositionState.Open);

                enemyBoard[row, col] = PositionState.Filled;
            }
        }

        //Places the player positions randomly.
        public void PlacePlayerShips()
        {

            for (int i = 0; i < 5; i++)
            {
                int row;
                int col;
                do
                {

                    row = Random.Next(size);
                    col = Random.Next(size);
                } while (PlayerBoard[row, col] != PositionState.Open);

                playerBoard[row, col] = PositionState.Filled;
            }
        }

        
        //Checks if a spot on the enemy board is filled with a ship.
        public bool isEnemyFilled(int row, int col)
        {
            if (enemyBoard[row, col] == PositionState.Filled)
            {
                return true;
            }
            return false;
        }

        //Checks if a spot on the player board is filled.
        public bool isPlayerFilled(int row, int col)
        {
            if (playerBoard[row, col] == PositionState.Filled)
            {
                return true;
            }
            return false;
        }

    }
}
