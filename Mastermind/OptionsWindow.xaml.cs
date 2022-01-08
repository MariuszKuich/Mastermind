using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        private bool secondPlayerPresent;

        public OptionsWindow(bool secondPlayerPresent)
        {
            InitializeComponent();

            this.secondPlayerPresent = secondPlayerPresent;
            if (!secondPlayerPresent)
            {
                lblSecondPlayer.Visibility = Visibility.Hidden;
                textSecondPlayer.Visibility = Visibility.Hidden;
                
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (AnyOfTextBoxesIsEmpty())
            {
                lblAllFieldsMandatory.Visibility = Visibility.Visible;
                return;
            }

            CreateGameWindow();
        }

        private bool AnyOfTextBoxesIsEmpty()
        {
            if (secondPlayerPresent)
            {
                return string.IsNullOrEmpty(textFirstPlayer.Text) || string.IsNullOrEmpty(textSecondPlayer.Text) || string.IsNullOrEmpty(textRoundCount.Text);
            }
            
            return string.IsNullOrEmpty(textFirstPlayer.Text) || string.IsNullOrEmpty(textRoundCount.Text);
        }

        private void CreateGameWindow()
        {
            int roundCount = Int32.Parse(textRoundCount.Text);
            GameWindow gameWindow = new GameWindow(textFirstPlayer.Text, textSecondPlayer.Text, roundCount == 0 ? 1 : roundCount);
            gameWindow.Show();

            this.Close();
        }
    }
}
