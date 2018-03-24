using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorabarabaV2
{
    public class GameSession : BaseNotificationClass
    {
        private Board _board;
        private string _gameMessage;

        public string currentInput { get; set; }
        public Board board
        {
            get { return _board; }
            set
            {
                _board = value;
                OnPropertyChanged(nameof(board));
            }
        }
        public string GameMessage
        {
            get { return _gameMessage; }
            set
            {
                _gameMessage = value;
                OnPropertyChanged(GameMessage);
            }
        }

        public GameSession()
        {
            board = new Board();
            board.CreateEmptyMills();
        }

        #region Phase 1 (Placing and Killing Cows

        // Place cows on board (Phase 1)
        private void placeCows(int playerID = 0)
        {
            int i = 0;
            while (i < 24)
            {
                board.updateMills(playerID);

                GameMessage = "Where do you want to place a cow?";
                int input = board.converToBoardPos(Console.ReadLine());
                while (input == -1)
                {
                    GameMessage = "Incorrect input!";
                    input = board.converToBoardPos(Console.ReadLine());
                }
                if (!(board.Cows[input].Id == -1) || board.Cows[input].Id == board.switchPlayer(playerID))
                {
                    GameMessage = "Cannot place there!";
                    continue;
                }

                board.Cows[input] = new Cow(input, board.getPlayerChar(playerID), i, playerID);

                board.getCurrentMills(playerID);

                if (board.areNewMills(playerID))
                {
                    board.killCow(playerID);
                }

                playerID = board.switchPlayer(playerID);
                i++;
            }
        }

        #endregion

        #region Phase 2 (Move Cows)

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

        // Main Loop
         private void gameLoop()
        {

            //Keep looping the game unless told not to 
            while (true)
            {
                // Phase 1 (Place and kill Cows
                placeCows();

                // Phase 2 (Move and kill Cows
                moveCows();

                break; // Just to pause while editing
            }
        }
    }
}

