using Mastermind.enums;
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
using System.Windows.Shapes;

namespace Mastermind
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public static GameMode GameMode { get; set; }
        private string firstUsername;
        private string secondUsername;
        private int roundCount;

        public GameWindow(string firstUsername, string secondUsername, int roundCount)
        {
            InitializeComponent();

            this.firstUsername = firstUsername;
            this.secondUsername = secondUsername;
            this.roundCount = roundCount;

            testLbl.Content = $"GameMode: {GameMode}\n" +
                $"RoundCount: {roundCount}\n" +
                $"FirstUser: {firstUsername}\n" +
                $"SecoundUser: {secondUsername}";
        }
    }
}
