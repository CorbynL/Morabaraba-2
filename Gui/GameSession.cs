using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

