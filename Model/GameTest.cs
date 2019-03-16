using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Battleship
{
    public class GameTest
    {
        [Test]
        public void PlaceShipsTest_RandomShipPlacement()
        {
            for (int i = 0; i < 1000; i++)
            {
                int counter = 0;
                Game game = new Game(5);
                game.PlaceEnemyShips();
                for (int row = 0; row < game.Size; row++)
                {
                    for (int col = 0; col < game.Size; col++)
                    {
                        if (game.enemyBoard[row, col] == Game.PositionState.Filled)
                        {
                            counter++;
                        }   
                    }
                }
                Assert.IsTrue(counter == 5);
            }
        }
    }
}
