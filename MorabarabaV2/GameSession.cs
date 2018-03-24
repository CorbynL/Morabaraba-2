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
        private bool isPlacing;
        private bool isKilling;
        private int playerID;
        private int placeNum;

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
                OnPropertyChanged(nameof(GameMessage));
            }
        }

        public GameSession()
        {
            board = new Board();
            board.CreateEmptyMills();
            isPlacing = true;
            isKilling = false;
            placeNum = 0;
            playerID = 0;
            GameMessage = "Placing : Player 1";
        }

        #region Phase 1 (Placing and Killing Cows

        private string getplayerImageSource(int player)
        {
            if (player == 0) return "/Gui;component/Images/redCow.png";
            else return "/Gui;component/Images/blueCow.png";
        }

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
                if (!(board.Cows[input].Id == -1) || board.Cows[input].Id == board.switchPlayer(playerID))
                {
                    GameMessage = "Cannot place there!";
                    return;
                }

                else
                {
                    board.Cows[input] = new Cow(input, board.getPlayerChar(playerID), placeNum, playerID, getplayerImageSource(playerID));

                    //This shouldnt be needed :/ Dont know what the problem is
                    OnPropertyChanged(nameof(board));

                    board.getCurrentMills(playerID);

                    if (board.areNewMills(playerID))
                    {
                        isKilling = true;
                        isPlacing = false;
                        GameMessage = $"player {playerID} : Killing";
                        return;
                    }

                    playerID = board.switchPlayer(playerID);
                    GameMessage = $"Player {playerID} : Placing";
                    placeNum++;
                }
            }
            else isPlacing = false;
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

                board.Cows[input].UserId = ' ';
                board.Cows[input].Id = -1;
                board.Cows[input].ImageName = "/Gui;component/Images/deadCow.png";
                isKilling = false;
                if(placeNum < 23)
                {
                    isPlacing = true;
                    playerID = board.switchPlayer(playerID);
                    GameMessage = $"player {playerID}: Placing";
                }
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
            if (isPlacing)
            {
                placeCow();
            }
            
            else if (isKilling)
            {
                killCow();
            }
        }
    }
}

