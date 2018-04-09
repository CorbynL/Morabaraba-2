using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorabarabaV2
{
    public class GameSession : BaseNotificationClass
    {
        private State currentState;
        private string _gameMessage;
        private int playerID;
        private int placeNum;

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

        public GameSession()
        {
            board = new Board();
            board.CreateEmptyMills();
            currentState = State.Placing;
            placeNum = 0;
            playerID = 0;
            GameMessage = "Player 1 : Placing";
        }

        private enum State
        {
            Placing,
            Killing,
            Moving,
            End
        }

        #region Phase 1 (Placing and Killing Cows

        // Place cows on board (Phase 1)

        private void placeCow()
        {

            if(placeNum < 24)
            {

                board.updateMills(playerID);

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
                    board.placeCow(playerID, input, placeNum);
                    OnPropertyChanged(nameof(board));

                    board.getCurrentMills(playerID);

                    if (board.areNewMills(playerID))
                    {
                        currentState = State.Killing;
                        GameMessage = $"Player {playerID + 1} : Killing";
                        return;
                    }

                    playerID = board.switchPlayer(playerID);
                    GameMessage = $"Player {playerID + 1} : Placing";
                    placeNum++;
                }
            }
            else
            {
                currentState = State.Moving;
                playerID = board.switchPlayer(playerID);
                GameMessage = $"Player {playerID + 1}: Moving";
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
                    currentState = State.Moving;
                    playerID = board.switchPlayer(playerID);
                    GameMessage = $"Player {playerID + 1}: Moving";
                }

                // Check win condition
                if (ownedCows(playerID) <= 2 && currentState == State.Moving)
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
            int
                pos = -1,
                newPos = -1;

            if (ownedCows(playerID) >= 2)
            {             
                pos = board.converToBoardPos(currentInput);

                if (pos == -1 || board.Cows[pos].Id != playerID)
                {
                    GameMessage = "Invalid choice!";
                    return;
                }

                GameMessage = "Now select where you want to move";

                newPos = board.converToBoardPos(currentInput);                

                if (newPos == -1 || board.Cows[newPos].Id != -1 || !board.isValidMove(pos, newPos))
                {
                    GameMessage = "Invalid move!";
                    return;
                }

                board.updateMills(playerID);

                board.placeCow(playerID, newPos, board.Cows[pos].CowNumber); // Place cow at new position
                board.Cows[pos] = new Cow(pos, ' ', -1, -1); // Put empty cow at original place

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

            }           
          
        }

        #endregion

    // Preform action depending on state of program
    public void performAction()
        {
            switch (currentState)
            {
                case State.Placing:
                    placeCow();
                    break;

                case State.Killing:
                    killCow();
                    break;

                case State.Moving:
                    moveCow();
                    break;

                case State.End:
                    //
                    // TODO
                    //
                    break;
            }
        }
    }
}

