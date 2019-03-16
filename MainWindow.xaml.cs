//-----------------------------------------------------------
//File:   Prog4.cs
//Desc:  This program is the battleship game in its most simplest form.
//-----------------------------------------------------------

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

namespace Battleship
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (cheatMode.IsChecked.HasValue)
            {
                
                if (cheatMode.IsChecked.Value == true)
                {
                    Game.IsCheatOn = true;
                }
                else
                {
                    Game.IsCheatOn = false;
                }
            }

            ValueType val = slValue.Value;
            int size = Convert.ToInt32(val);
            var gameWin = new GameWindow(size);
            gameWin.Show();
            this.Visibility = Visibility.Collapsed;
        }

    }
}
