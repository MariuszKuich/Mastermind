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
        private Dictionary<EvaluationPin, BitmapImage> resultsColors;
        private int[] colorSlotIndexes;
        private ImageSource[] codedSequence;
        private int decodedCount;

        private readonly int MAX_EVALUATION_PINS_COUNT = 4;
        private readonly int MAX_ATTEMPTS_COUNT = 12;

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
            resultsColors = new Dictionary<EvaluationPin, BitmapImage>();
            resultsColors.Add(EvaluationPin.INCORRECT_PLACE, new BitmapImage(new Uri("/resources/transparent/pink_s.png", UriKind.Relative)));
            resultsColors.Add(EvaluationPin.CORRECT_PLACE, new BitmapImage(new Uri("/resources/transparent/red_s.png", UriKind.Relative)));
        }

        private void SetInitialGameState()
        {
            codedSequence = new ImageSource[4];
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
                SetSequenceOnPeeks();
                activePlayer = GetOtherPlayer();
                gamePhase = GamePhase.DECODING;
                RandomizeColorPicker();
                UpdateControls(Messages.BreakCode);
            }
            else
            {
                PlaceSequenceOnBoard();
                EvaluateRoundBasedOnGameMode();
            }
        }

        private ActivePlayer GetOtherPlayer()
        {
            return activePlayer == ActivePlayer.FIRST ? ActivePlayer.SECOND : ActivePlayer.FIRST;
        }

        private void SaveCodedSequence()
        {
            codedSequence[0] = slot1.Source;
            codedSequence[1] = slot2.Source;
            codedSequence[2] = slot3.Source;
            codedSequence[3] = slot4.Source;
        }

        private void SetSequenceOnPeeks()
        {
            peek1.Source = codedSequence[0];
            peek2.Source = codedSequence[1];
            peek3.Source = codedSequence[2];
            peek4.Source = codedSequence[3];
        }

        private void PlaceSequenceOnBoard()
        {
            Image[] boardsRow = pins[currentRoundGuess];
            boardsRow[0].Source = slot1.Source;
            boardsRow[1].Source = slot2.Source;
            boardsRow[2].Source = slot3.Source;
            boardsRow[3].Source = slot4.Source;
        }

        private void EvaluateRoundBasedOnGameMode()
        {
            if (GameMode == GameMode.DoubleManual)
            {
                SwitchColorButtons(false);
                PrepareEvaluationBoard();
                gridEvaluate.Visibility = Visibility.Visible;
                Redraw();
                return;
            }
            if (GameMode == GameMode.DoubleAutomatic)
            {
                bool decoded = EvaluateRoundTwoPlayers();
                SumUpAttemptTwoPlayers(decoded);
                return;
            }
            if (GameMode == GameMode.Single)
            {

            }
        }

        private void SwitchColorButtons(bool isEnabled)
        {
            btnSubmit.IsEnabled = isEnabled;
            btnSlot1Left.IsEnabled = isEnabled;
            btnSlot1Right.IsEnabled = isEnabled;
            btnSlot2Left.IsEnabled = isEnabled;
            btnSlot2Right.IsEnabled = isEnabled;
            btnSlot3Left.IsEnabled = isEnabled;
            btnSlot3Right.IsEnabled = isEnabled;
            btnSlot4Left.IsEnabled = isEnabled;
            btnSlot4Right.IsEnabled = isEnabled;
        }

        private void PrepareEvaluationBoard()
        {
            textEvaluation1.Text = string.Empty;
            textEvaluation2.Text = string.Empty;

            ChangeCodePeekVisibility(Visibility.Hidden);
        }

        private bool EvaluateRoundTwoPlayers()
        {
            int pinsIncorrectPlace = 0;
            int pinsCorrectPlace = 0;
            bool decoded = false;

            Image[] guessedColors = { slot1, slot2, slot3, slot4 };
            for (int i = 0; i < MAX_EVALUATION_PINS_COUNT; i++)
            {
                if (guessedColors[i].Source.Equals(codedSequence[i]))
                {
                    pinsCorrectPlace++;
                }
                else if (codedSequence.Contains(guessedColors[i].Source))
                {
                    pinsIncorrectPlace++;
                }
            }

            if (pinsCorrectPlace == MAX_EVALUATION_PINS_COUNT)
            {
                decoded = true;
            }

            SetEvaluationPins(pinsIncorrectPlace, pinsCorrectPlace);

            return decoded;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnSubmitEvaluation_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textEvaluation1.Text) || string.IsNullOrEmpty(textEvaluation2.Text))
            {
                lblIncorrectData.Visibility = Visibility.Visible;
                Redraw();
                return;
            }
            
            int pinsIncorrectPlace = Int32.Parse(textEvaluation1.Text);
            int pinsCorrectPlace = Int32.Parse(textEvaluation2.Text);
            bool decoded = false;

            if (pinsIncorrectPlace + pinsCorrectPlace > MAX_EVALUATION_PINS_COUNT)
            {
                lblIncorrectData.Visibility = Visibility.Visible;
                Redraw();
                return;
            }

            if (pinsCorrectPlace == MAX_EVALUATION_PINS_COUNT)
            {
                decoded = true;
            }

            SetEvaluationPins(pinsIncorrectPlace, pinsCorrectPlace);

            gridEvaluate.Visibility = Visibility.Hidden;
            lblIncorrectData.Visibility = Visibility.Hidden;
            SwitchColorButtons(true);

            SumUpAttemptTwoPlayers(decoded);
        }

        private void SetEvaluationPins(int pinsIncorrectPlace, int pinsCorrectPlace)
        {
            int index = 0;
            Image[] pins = results[currentRoundGuess];

            while (pinsIncorrectPlace > 0)
            {
                pins[index].Source = resultsColors[EvaluationPin.INCORRECT_PLACE];
                index++;
                pinsIncorrectPlace--;
            }
            while (pinsCorrectPlace > 0)
            {
                pins[index].Source = resultsColors[EvaluationPin.CORRECT_PLACE];
                index++;
                pinsCorrectPlace--;
            }
        }

        private void SumUpAttemptTwoPlayers(bool decoded)
        {
            if (decoded)
            {
                HandleCurrentGuessEndTwoPlayers(Messages.Decoded, currentRoundGuess);
            }
            else if (currentRoundGuess == MAX_ATTEMPTS_COUNT)
            {
                HandleCurrentGuessEndTwoPlayers(Messages.NotDecoded, currentRoundGuess + 1);
            }
            else
            {
                UpdateControls(Messages.NextAttempt);
                currentRoundGuess++;
            } 
        }

        private void HandleCurrentGuessEndTwoPlayers(String message, int points)
        {
            if (activePlayer == ActivePlayer.FIRST)
            {
                secondUserScore += points;
            }
            else
            {
                firstUserScore += points;
            }

            ClearBoard(currentRoundGuess);
            currentRoundGuess = 1;
            decodedCount++;
            if (decodedCount == 2)
            {
                currentRound++;
                decodedCount = 0;
                if (currentRound > roundCount)
                {
                    currentRound--;
                    SwitchColorButtons(false);
                    if (firstUserScore == secondUserScore)
                    {
                        UpdateControls("Koniec gry! Remis!");
                    }
                    else
                    {
                        string winner = firstUserScore > secondUserScore ? firstUsername : secondUsername;
                        UpdateControls($"Koniec gry! Wygrał gracz {winner}!");
                    }
                    return;
                }
            }
            gamePhase = GamePhase.CODING;
            UpdateControls(message);
        }

        private void SumUpAttemptOnePlayer()
        {

        }

        private void ClearBoard(int numberOfGuesses)
        {
            while (numberOfGuesses > 0)
            {
                Image[] pinsToClear = pins[numberOfGuesses];
                foreach (Image pinToClear in pinsToClear)
                {
                    pinToClear.Source = null;
                }

                Image[] resultsToClear = results[numberOfGuesses];
                foreach (Image resultToClear in resultsToClear)
                {
                    resultToClear.Source = null;
                }

                numberOfGuesses--;
            }
        }

        private void btnPeekCode_Click(object sender, RoutedEventArgs e)
        {
            Visibility visibility = peek1.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;

            ChangeCodePeekVisibility(visibility);
        }

        private void ChangeCodePeekVisibility(Visibility visibility)
        {
            peek1.Visibility = visibility;
            peek2.Visibility = visibility;
            peek3.Visibility = visibility;
            peek4.Visibility = visibility;
        }
    }
}
