using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gui
{
    public class GameSession : BaseNotificationClass
    {
        private State currentState;
        private string _gameMessage;
        private string buttonContent;
        private int playerID;
        private int placeNum;
        private int movePos;
        private string player1Cows;
        private string player2Cows;
        private bool isPlacing;

        public string currentInput { get; set; }
        public Board board { get; set; }
        public string GameMessage
        {
            get { return _gameMessage; }
            set
            {
                _gameMessage = value;
                OnPropertyChanged(nameof(GameMessage));
            }
        }
        public string ButtonContent
        {
            get { return buttonContent; }
            set
            {
                buttonContent = value;
                OnPropertyChanged(nameof(ButtonContent));
            }
        }
        public string Player1Cows
        {
            get { return player1Cows; }
            set
            {
                player1Cows = value;
                OnPropertyChanged(nameof(Player1Cows));
            }
        }
        public string Player2Cows
        {
            get { return player2Cows; }
            set
            {
                player2Cows = value;
                OnPropertyChanged(nameof(Player2Cows));
            }
        }
        public bool IsPlacing
        {
            get { return isPlacing; }
            set
            {
                isPlacing = value;
                OnPropertyChanged(nameof(isPlacing));
            }
        }

        public GameSession()
        {
            board = new Board();
            board.CreateEmptyMills();
            currentState = State.Placing;
            placeNum = 0;
            playerID = 0;
            movePos = -1;
            GameMessage = "Player 1 : Placing";
            ButtonContent = "Place Cow";
            Player1Cows = "12";
            Player2Cows = "12";
            IsPlacing = true;

            // Info window
            Window excuses = new Window();
            excuses.Topmost = true;
            TextBlock box = new TextBlock();
            box.Width = excuses.Width;
            box.Height = excuses.Height;
            box.HorizontalAlignment = HorizontalAlignment.Center;
            box.Text = "Dear, Yusuf.\nTo prevent you from frustion and total anguish, I will list our \"bugs\"\n 1. There is no draw state, so if you reach a draw like position, you will be stuck :D\n 2. If all cows are in a mill, you can't kill any of them. Stuck again :D\n\n You may now close this window and continue... :D";
            box.Background = Brushes.LightBlue;
            excuses.Background = Brushes.Black;
            excuses.FontSize = 20;
            excuses.Content = box;
            excuses.Show();
        }

        private enum State
        {
            Placing,
            Killing,
            Moving1,
            Moving2,
            End
        }

        #region Phase 1 (Placing and Killing Cows

        // Place cows on board (Phase 1)

        private void placeCow()
        {

            if(placeNum < 24)
            {              
                int input = board.converToBoardPos(currentInput);
                if(input == -1)
                {
                    GameMessage = "Incorrect input!";
                    return;
                }
                if (board.canPlace(input,playerID))
                {
                    GameMessage = "Cannot place there!";
                    return;
                }

                else
                {
                    board.updateMills(playerID);                   

                    board.placeCow(playerID, input, placeNum);
                    board.removeBrokenMills(playerID);

                    OnPropertyChanged(nameof(board));
                    
                    board.getCurrentMills(playerID);

                    updatePlayerCows(playerID);

                    if (board.areNewMills(playerID))
                    {
                        currentState = State.Killing;
                        GameMessage = $"Player {playerID + 1} : Killing";
                        return;
                    }
                    
                    playerID = board.switchPlayer(playerID);

                    if (placeNum == 23)
                    {
                        currentState = State.Moving1;
                        GameMessage = $"Player {playerID + 1}: Moving";
                        hidePlacingBar();
                        return;
                    }
                    
                    GameMessage = $"Player {playerID + 1} : Placing";                   
                    placeNum++;                                
                    }
            }            
        }

        #endregion

        #region Phase 2 (Move Cows)

        private void killCow()
        {
            int input = board.converToBoardPos(currentInput);

            if (!board.canKill(input, playerID))
            {
                GameMessage = "Can't kill that one!";
            }

            else
            {
                board.Cows[input] = new Cow(input, ' ', -1, -1); // Put empty cow at crime scene                

                OnPropertyChanged(nameof(board));

                if (placeNum < 23)
                {
                    currentState = State.Placing;
                    playerID = board.switchPlayer(playerID);
                    GameMessage = $"Player {playerID + 1}: Placing";
                    placeNum++;
                }
                else
                {
                    currentState = State.Moving1;
                    playerID = board.switchPlayer(playerID);
                    GameMessage = $"Player {playerID + 1}: Moving";
                }

                // Check win condition
                if (ownedCows(playerID) <= 2 && currentState == State.Moving1)
                {
                    currentState = State.End;
                    playerID = board.switchPlayer(playerID);
                    GameMessage = $"Player {playerID + 1} wins!";
                }                
            }
        }

        //For Board.cs: Get number of cows owned by player

        private int ownedCows (int playerID)
        {
            if (playerID < -1 || playerID > 1)
                return -1;

            else
            {
                int count = 0;
                for (int i = 0; i < board.Cows.Length; i++)
                {
                    if (board.Cows[i].Id == playerID)
                        count++;
                }

                return count;
            }
        }

        private void moveCow()
        {
            if (currentState == State.Moving1)
            {
                movePos = board.converToBoardPos(currentInput);

                if (movePos == -1 || board.Cows[movePos].Id != playerID)
                {
                    GameMessage = "Invalid choice!";
                    return;
                }
                else
                {
                    GameMessage = "Now select where you want to move";
                    currentState = State.Moving2;
                }
            }
            else
            {           
                int newPos = board.converToBoardPos(currentInput);           
                
                if (newPos == -1 || board.Cows[newPos].Id != -1)
                {
                    GameMessage = "Invalid move!";
                    return;
                }

                if (!board.isValidMove(movePos, newPos) && ownedCows(playerID) > 3) // Check if it is a valid move and if it is in flying mode
                {
                    GameMessage = "Invalid move!";
                    return;
                }

                board.updateMills(playerID);

                board.placeCow(playerID, newPos, board.Cows[movePos].CowNumber); // Place cow at new position
                board.Cows[movePos] = new Cow(movePos, ' ', -1, -1); // Put empty cow at original place

                board.removeBrokenMills(playerID);

                OnPropertyChanged(nameof(board));
                
                board.getCurrentMills(playerID);

                if (board.areNewMills(playerID))
                {
                    currentState = State.Killing;
                    GameMessage = $"Player {playerID + 1} : Killing";
                    return;
                }

                playerID = board.switchPlayer(playerID);
                GameMessage = $"Player {playerID + 1} : Moving";
                currentState = State.Moving1;

            }           
          
        }

        #endregion

    #region Gui update functions

        private void updateButtonContent()
        {
            switch (currentState)
            {
                case State.Placing:
                    ButtonContent = "Place Cow";
                    break;

                case State.Killing:
                    ButtonContent = "Kill Cow";
                    break;

                case State.Moving1:
                    ButtonContent = "Choose Cow";
                    break;
                case State.Moving2:
                    ButtonContent = "Place Cow";
                    break;

                case State.End:
                    ButtonContent = "GAME OVER";
                    break;
            }
        }
        private void updatePlayerCows(int playerID)
        {
            if(playerID == 0)
            {
                Player1Cows = Convert.ToString((Convert.ToInt32(player1Cows) - 1));
            }
            else Player2Cows = Convert.ToString((Convert.ToInt32(player2Cows) - 1));
        }

        private void hidePlacingBar()
        {
            IsPlacing = false;
        }

    #endregion

        // Preform action depending on state of program
        public void performAction()
        {
            switch (currentState)
            {
                case State.Placing:
                    placeCow();
                    updateButtonContent();
                    break;

                case State.Killing:
                    killCow();
                    updateButtonContent();
                    break;

                case State.Moving1:
                case State.Moving2:
                    moveCow();
                    updateButtonContent();
                    break;                

                case State.End:
                    //Do nothing
                    updateButtonContent();
                    break;
            }
        }
    }
}

