using Mastermind.enums;
using System.Windows;

namespace Mastermind
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

        private void btnDoubleManual_Click(object sender, RoutedEventArgs e)
        {
            GameWindow.GameMode = GameMode.DoubleManual;
            DisplayOptionsWindow(true);
        }

        private void btnDoubleAutomatic_Click(object sender, RoutedEventArgs e)
        {
            GameWindow.GameMode = GameMode.DoubleAutomatic;
            DisplayOptionsWindow(true);
        }

        private void btnSingle_Click(object sender, RoutedEventArgs e)
        {
            GameWindow.GameMode = GameMode.Single;
            DisplayOptionsWindow(false);
        }

        private void DisplayOptionsWindow(bool secondPlayerPresent)
        {
            OptionsWindow optionsWindow = new OptionsWindow(secondPlayerPresent);
            optionsWindow.Show();

            this.Close();
        }
    }
}
