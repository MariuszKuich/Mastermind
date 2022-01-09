using Mastermind.enums;
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
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public static GameMode GameMode { get; set; }
        private string firstUsername;
        private int firstUserScore;
        private string secondUsername;
        private int secondUserScore;
        private ActivePlayer activePlayer;
        private GamePhase gamePhase;
        private int roundCount;
        private int currentRound;
        private int currentRoundGuess;
        private Dictionary<int, Image[]> pins;
        private Dictionary<int, Image[]> results;
        private BitmapImage[] pinsColors;
        private BitmapImage[] resultsColors;
        private int[] colorSlotIndexes;
        private BitmapImage[] codedSequence;

        public GameWindow(string firstUsername, string secondUsername, int roundCount)
        {
            InitializeComponent();

            this.firstUsername = firstUsername;
            this.secondUsername = secondUsername;
            this.roundCount = roundCount;

            InitializePins();
            InitializeResults();
            InitializePinsColors();
            InitializeResultsColors();

            SetInitialGameState();
            UpdateControls(Messages.SetCode);
        }

        private void InitializePins()
        {
            pins = new Dictionary<int, Image[]>();
            pins.Add(1, new Image[] { pin1_1, pin1_2, pin1_3, pin1_4 });
            pins.Add(2, new Image[] { pin2_1, pin2_2, pin2_3, pin2_4 });
            pins.Add(3, new Image[] { pin3_1, pin3_2, pin3_3, pin3_4 });
            pins.Add(4, new Image[] { pin4_1, pin4_2, pin4_3, pin4_4 });
            pins.Add(5, new Image[] { pin5_1, pin5_2, pin5_3, pin5_4 });
            pins.Add(6, new Image[] { pin6_1, pin6_2, pin6_3, pin6_4 });
            pins.Add(7, new Image[] { pin7_1, pin7_2, pin7_3, pin7_4 });
            pins.Add(8, new Image[] { pin8_1, pin8_2, pin8_3, pin8_4 });
            pins.Add(9, new Image[] { pin9_1, pin9_2, pin9_3, pin9_4 });
            pins.Add(10, new Image[] { pin10_1, pin10_2, pin10_3, pin10_4 });
            pins.Add(11, new Image[] { pin11_1, pin11_2, pin11_3, pin11_4 });
            pins.Add(12, new Image[] { pin12_1, pin12_2, pin12_3, pin12_4 });
        }

        private void InitializeResults()
        {
            results = new Dictionary<int, Image[]>();
            results.Add(1, new Image[] { result1_1, result1_2, result1_3, result1_4 });
            results.Add(2, new Image[] { result2_1, result2_2, result2_3, result2_4 });
            results.Add(3, new Image[] { result3_1, result3_2, result3_3, result3_4 });
            results.Add(4, new Image[] { result4_1, result4_2, result4_3, result4_4 });
            results.Add(5, new Image[] { result5_1, result5_2, result5_3, result5_4 });
            results.Add(6, new Image[] { result6_1, result6_2, result6_3, result6_4 });
            results.Add(7, new Image[] { result7_1, result7_2, result7_3, result7_4 });
            results.Add(8, new Image[] { result8_1, result8_2, result8_3, result8_4 });
            results.Add(9, new Image[] { result9_1, result9_2, result9_3, result9_4 });
            results.Add(10, new Image[] { result10_1, result10_2, result10_3, result10_4 });
            results.Add(11, new Image[] { result11_1, result11_2, result11_3, result11_4 });
            results.Add(12, new Image[] { result12_1, result12_2, result12_3, result12_4 });
        }

        private void InitializePinsColors()
        {
            pinsColors =  new BitmapImage[]
            {
                new BitmapImage(new Uri("/resources/transparent/blue.png", UriKind.Relative)),
                new BitmapImage(new Uri("/resources/transparent/green.png", UriKind.Relative)),
                new BitmapImage(new Uri("/resources/transparent/grey.png", UriKind.Relative)),
                new BitmapImage(new Uri("/resources/transparent/orange.png", UriKind.Relative)),
                new BitmapImage(new Uri("/resources/transparent/pink.png", UriKind.Relative)),
                new BitmapImage(new Uri("/resources/transparent/purple.png", UriKind.Relative)),
                new BitmapImage(new Uri("/resources/transparent/red.png", UriKind.Relative)),
                new BitmapImage(new Uri("/resources/transparent/yellow.png", UriKind.Relative))
            };
        }

        private void InitializeResultsColors()
        {
            resultsColors =  new BitmapImage[]
            {
                new BitmapImage(new Uri("/resources/transparent/red_s.png", UriKind.Relative)),
                new BitmapImage(new Uri("/resources/transparent/pink_s.png", UriKind.Relative))
            };
        }

        private void SetInitialGameState()
        {
            codedSequence = new BitmapImage[4];
            colorSlotIndexes = new int[4];
            RandomizeColorPicker();
            currentRound = 1;
            currentRoundGuess = 1;
            activePlayer = ActivePlayer.FIRST;
            gamePhase = GamePhase.CODING;
        }

        private void RandomizeColorPicker()
        {
            colorSlotIndexes[0] = RandomizeColorSlot(slot1);
            colorSlotIndexes[1] = RandomizeColorSlot(slot2);
            colorSlotIndexes[2] = RandomizeColorSlot(slot3);
            colorSlotIndexes[3] = RandomizeColorSlot(slot4);
        }

        private int RandomizeColorSlot(Image slot)
        {
            Random random = new Random();
            int randomIndex = random.Next(0, pinsColors.Length);
            slot.Source = pinsColors[randomIndex];
            return randomIndex;
        }

        private void UpdateControls(string message)
        {
            lblFirstScore.Content = $"{firstUsername}: {firstUserScore}";
            lblSecondScore.Content = $"{secondUsername}: {secondUserScore}";
            lblRound.Content = $"Runda: {currentRound} / {roundCount}";
            if (activePlayer == ActivePlayer.FIRST)
            {
                lblFirstPlays.Visibility = Visibility.Visible;
                lblSecondPlays.Visibility = Visibility.Hidden;

            }
            else
            {
                lblFirstPlays.Visibility = Visibility.Hidden;
                lblSecondPlays.Visibility = Visibility.Visible;
            }
            lblCommand.Content = message;

            Redraw();
        }

        private void Redraw()
        {
            InvalidateVisual();
            UpdateLayout();
        }

        private void btnSlot1Left_Click(object sender, RoutedEventArgs e)
        {
            UpdateColorPicker(slot1, Direction.LEFT, 0);
        }

        private void btnSlot1Right_Click(object sender, RoutedEventArgs e)
        {
            UpdateColorPicker(slot1, Direction.RIGHT, 0);
        }

        private void btnSlot2Left_Click(object sender, RoutedEventArgs e)
        {
            UpdateColorPicker(slot2, Direction.LEFT, 1);
        }

        private void btnSlot2Right_Click(object sender, RoutedEventArgs e)
        {
            UpdateColorPicker(slot2, Direction.RIGHT, 1);
        }

        private void btnSlot3Left_Click(object sender, RoutedEventArgs e)
        {
            UpdateColorPicker(slot3, Direction.LEFT, 2);
        }

        private void btnSlot3Right_Click(object sender, RoutedEventArgs e)
        {
            UpdateColorPicker(slot3, Direction.RIGHT, 2);
        }

        private void btnSlot4Left_Click(object sender, RoutedEventArgs e)
        {
            UpdateColorPicker(slot4, Direction.LEFT, 3);
        }

        private void btnSlot4Right_Click(object sender, RoutedEventArgs e)
        {
            UpdateColorPicker(slot4, Direction.RIGHT, 3);
        }

        private void UpdateColorPicker(Image colorSlot, Direction direction, int lastColorDataIndex)
        {
            int index = colorSlotIndexes[lastColorDataIndex];
            if (direction == Direction.LEFT)
            {
                index--; 
            }
            else
            {
                index++;
            }
            index = ValidateIndex(index);

            colorSlot.Source = pinsColors[index];
            colorSlotIndexes[lastColorDataIndex] = index;

            Redraw();
        }

        private int ValidateIndex(int index)
        {
            if (index < 0)
            {
                return pinsColors.Length - 1;
            }
            if (index > pinsColors.Length - 1)
            {
                return 0;
            }
            return index;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (gamePhase == GamePhase.CODING)
            {
                SaveCodedSequence();
                activePlayer = GetOtherPlayer();
                gamePhase = GamePhase.DECODING;
                RandomizeColorPicker();
                UpdateControls(Messages.BreakCode);
            }
            else
            {

            }
        }

        private ActivePlayer GetOtherPlayer()
        {
            return activePlayer == ActivePlayer.FIRST ? ActivePlayer.SECOND : ActivePlayer.FIRST;
        }

        private void SaveCodedSequence()
        {
            codedSequence[0] = (BitmapImage)slot1.Source;
            codedSequence[1] = (BitmapImage)slot2.Source;
            codedSequence[2] = (BitmapImage)slot3.Source;
            codedSequence[3] = (BitmapImage)slot4.Source;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
