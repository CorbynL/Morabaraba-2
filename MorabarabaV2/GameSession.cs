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
            GameMessage = "Placing : Player 0";
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
                        GameMessage = $"player {playerID} : Killing";
                        return;
                    }

                    playerID = board.switchPlayer(playerID);
                    GameMessage = $"Player {playerID} : Placing";
                    placeNum++;
                }
            }
            else currentState = State.Moving;
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
                board.killCow(input);
                OnPropertyChanged(nameof(board));

                if (placeNum < 23)
                {
                    currentState = State.Placing;
                    playerID = board.switchPlayer(playerID);
                    GameMessage = $"player {playerID}: Placing";
                }
                
                // Check win condition
                if (ownedCows(playerID) <= 2)
                {
                    currentState = State.End;
                    playerID = board.switchPlayer(playerID);
                    GameMessage = $"Player {playerID} wins!";
                }                

                else currentState = State.Moving;
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

        private void moveCows()
        {
            bool canMove = true;
            int i = 0; // Start with player 1

            while (canMove)
            {
                bool validInput = false;
                int pos = -1;
                while (!validInput)   // Check if a valid input has been recieved
                {
                    GameMessage = "Please select a cow to move";

                    pos = board.converToBoardPos(Console.ReadLine());

                    if (pos != -1 && board.Cows[pos].Id == i % 2)
                    {
                        validInput = true;
                    }
                    else
                        GameMessage = "Not your Cow!";
                }
                
            }
        }

        #endregion

    // Preform action depending on state of program
    public void preformAction()
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
                    //
                    // TODO
                    //
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

