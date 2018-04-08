using System;

namespace MorabarabaV2
{
    public class Program
    {

        // Main Game Board 
        private static Board board;


        #region Writing to the screen

        // Print in the middle of the console
        static void printCenter(string line)
        {
            Console.SetCursorPosition((Console.WindowWidth - line.Length) / 2, Console.CursorTop);
            Console.WriteLine(line);
            Console.SetCursorPosition((Console.WindowWidth) / 2, Console.CursorTop);
        }

        static void startUpPrompt()
        {
            Console.SetWindowPosition(0,0);
            Console.SetWindowSize(Console.WindowWidth, 60);
            Console.ForegroundColor = ConsoleColor.Green;

            printCenter("   *                                                        ");
            printCenter(" (  `                       )                      )        ");
            printCenter(" )\\))(        (       )  ( /(     )  (       )  ( /(     )  ");
            printCenter("((_)()\\   (   )(   ( /(  )\\()) ( /(  )(   ( /(  )\\()) ( /(  ");
            printCenter("(_()((_)  )\\ (()\\  )(_))((_)\\  )(_))(()\\  )(_))((_)\\  )(_)) ");
            printCenter("|  \\/  | ((_) ((_)((_)_ | |(_)((_)_  ((_)((_)_ | |(_)((_)_  ");
            printCenter("| |\\/| |/ _ \\| '_|/ _` || '_ \\/ _` || '_|/ _` || '_ \\/ _` | ");
            printCenter("|_|  |_|\\___/|_|  \\__,_||_.__/\\__,_||_|  \\__,_||_.__/\\__,_| ");
            Console.WriteLine("\n\n\n");
            printCenter(" ------------------- Let the Games Begin! ------------------- ");
            Console.WriteLine("\n\n");
            printCenter(" ---- [ Rules ] ---- ");
            Console.WriteLine("");
            printCenter("1. Morabaraba consists of a board with pieces which we refer to as 'cows'.");
            printCenter("Each player starts with 12 cows and the game consists of 3 phases:");
            printCenter("Placing, Moving and Flying.\n");
            printCenter("i. Placing - Place one of your 12 cows you start with at a board-position.");
            printCenter("ii. Moving - Once you have placed your 12 cows, move your cow to any empty");
            printCenter("position neighbouring your cow.");
            printCenter("iii. Flying - When you have 3 cows left, you can then move your cow to any");
            printCenter("empty position on the board you feel like.\n");
            Console.WriteLine("");
            printCenter("2. You can eliminate your opponent's cows using mills.\n");
            printCenter(" A mill is when you get 3");
            printCenter("of your cows in a row (including straight diagonals).\n");
            printCenter("When formed, you will be asked");
            printCenter("which of your opponents cows you would like to kill. Enter their");
            printCenter("their board-position and");
            printCenter("*poof* free Beef-Wellington for dinner. There are some\n");
            printCenter("rules to mills, such as:\n");
            printCenter("i. You can only form the same mill with the same cows\n");
            printCenter("after 2 of your turns has passed.");
            printCenter("ii. If you form a mill, you cannot kill cows that are in a\n");
            printCenter("mill already i.e. they are safe.");
            printCenter("iii. If you form a mill and all of your opponent's cows are in\n");
            printCenter("in mills, then you may kill any of cow.\n");
            Console.WriteLine("");
            printCenter("3. WIN - When your opponent has only 2 cows left, then you win!\n");
            Console.WriteLine("");
            printCenter("4. If no vaild moves are available to any player, the game ends in a DRAW.\n");
            Console.WriteLine("");
            printCenter(" --- [ Press Enter to Begin ] --- ");

            Console.ReadKey();
        }

        static void switchcolours()
        {
            if (Console.ForegroundColor == ConsoleColor.Cyan || Console.ForegroundColor == ConsoleColor.Green)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.Cyan;
        }

        static void playerWinsStartAgain(int player)
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\n\n\n\n\n\n");
            printCenter(String.Format("Player {0} wins!!!\n\n",player));
            printCenter("Type 'Yes' to start a new game or push enter to exit");
            string input = Console.ReadLine();
            if (input.ToLower() == "yes")
            {
                board.newCows();
                gameLoop();

            }
            else
                Environment.Exit(0);
        }

        #endregion
        
        #region Phase 1 (Placing and Killing Cows
        // Place cows on board (Phase 1)
        static void placeCows(int playerID = 0)
        {

            int i = 0;
            while(i < 24)
            {
                // Set colour acording to player
                switchcolours();

                board.drawboard();

                board.updateMills(playerID);
          
                printCenter(String.Format("Where do you want to place a cow? (You have {0} cows left to place)\n", (25 -i) / 2 ));
                int input = board.converToBoardPos(Console.ReadLine());
                while (input == -1)
                {
                    printCenter("Incorrect input!\n");
                    input = board.converToBoardPos(Console.ReadLine());
                }
                if (!(board.Cows[input].Id == -1) || board.Cows[input].Id == board.switchPlayer(playerID))
                {
                    printCenter("Cannot place there!\n");
                    continue;
                } 

                board.Cows[input] = new Cow(input, board.getPlayerChar(playerID), i, playerID);

                board.getCurrentMills(playerID);

                if (board.areNewMills(playerID))
                {
                    board.killCow(playerID);
                }

                playerID = board.switchPlayer(playerID);


                if (i == 23)    // If we're on the last cow placement and the board is full, player 1 wins and the game ends
                    if (board.isFullBoard())
                    {
                        playerWinsStartAgain(1);
                        gameLoop();                     // If player wants to start again
                    }

                i++;
            }
        }

        #endregion

        #region Phase 2 (Move Cows)

        static void moveCows()
        {
            switchcolours();

            board.drawboard();


            bool canMove = true;
            int i = 0; // Start with player 1

            while(canMove)
            {
                bool validInput = false;
                int pos = -1;
                while (!validInput)   // Check if a valid input has been recieved
                {
                    printCenter("Please select a cow to move\n");

                    pos = board.converToBoardPos(Console.ReadLine());

                    if (pos != -1 && board.Cows[pos].Id == i % 2)
                    {
                        validInput = true;
                    }
                    else
                        printCenter("Not your Cow!\n");
                }
            }
        }

        #endregion

        #region Game Loop and Main Function

        // Main Loop
        static void gameLoop()
        {
            startUpPrompt();

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


        static void Main(string[] args)
        {
            board = new Board();
            gameLoop();

        }

        #endregion
    }
}




#region Useful information from F# Morabaraba
/*

    
    // Positions a cow can move from at a position
let isValidMove(cow : Cow) (position : int) =
    match cow.Position, position with
    | (0,1) | (0,3) | (0,9)
    | (1,0) | (1,2) | (1,4) 
    | (2,1) | (2,5) | (2,14)
    | (3,0) | (3,4) | (3,6) | (3,10) 
    | (4,1) | (4,3) | (4,5) | (4,7) 
    | (5,2) | (5,4) | (5,8) | (5,13)
    | (6,3) | (6,7) | (6,11) 
    | (7,4) | (7,6) | (7,8) 
    | (8,5) | (8,7) | (8,12) 
    | (9,0) | (9,10)| (9,21) 
    | (10,3) | (10,9) | (10,11) | (10,18) 
    | (11,6) | (11,10) | (11,15) 
    | (12,8) | (12,13) | (12,17) 
    | (13,5) | (13,12) | (13,14) | (13,20) 
    | (14,2) | (14,13) | (14,23) 
    | (15,11) | (15,16) | (15,18) 
    | (16,15) | (16,17) | (16,19) 
    | (17,12) | (17,16) | (17,20) 
    | (18,10) | (18,15) | (18,19) | (18,21) 
    | (19,16) | (19,18) | (19,20) | (19,22) 
    | (20,13) | (20,17) | (20,19) | (20,23) 
    | (21,9) | (21,18) | (21,22) 
    | (22,19) | (22,21) | (22,23)
    | (23,14) | (23,20) | (23,22) -> position
    | _ -> -1 




    // Seems redundant :? But it is what it is
let possibleMoves cowPos = 
    match cowPos with
    | 0  -> [ 1; 3; 9]     
    | 1  -> [ 0; 2; 4]
    | 2  -> [ 1; 5; 14]
    | 3  -> [ 0; 4; 6; 10]
    | 4  -> [ 1; 3; 5; 7]
    | 5  -> [ 2; 4; 8; 13]
    | 6  -> [ 3; 7; 11]
    | 7  -> [ 4; 6; 8]
    | 8  -> [ 5; 7; 12]
    | 9  -> [ 0; 10; 21]
    | 10 -> [ 3; 9; 11; 18]
    | 11 -> [ 6; 10; 15]
    | 12 -> [ 8; 13; 17]
    | 13 -> [ 5; 12; 14; 20]
    | 14 -> [ 2; 13; 23]
    | 15 -> [ 11; 16; 18]
    | 16 -> [ 15; 17; 19]
    | 17 -> [ 12; 16; 20]
    | 18 -> [ 10; 15; 19; 21]
    | 19 -> [ 16; 18; 20; 22]
    | 20 -> [ 13; 17; 19; 23]
    | 21 -> [ 9; 18; 22]
    | 22 -> [ 19; 21; 23]
    | 23 -> [ 14; 20; 22]
    | _ -> failwith "No no, jail is that way."


    //All possible mill variations
let allMills = [  
    { emptyMill with millPos =    0::1::2::[]} // A1, A4, A7
    { emptyMill with millPos =    3::4::5::[]} // B2, B4, B6    
    { emptyMill with millPos =    6::7::8::[]} // C3, C4, C5
    { emptyMill with millPos =  9::10::11::[]} // D1, D2, D3
    { emptyMill with millPos = 12::13::14::[]} // D5, D6, D7
    { emptyMill with millPos = 15::16::17::[]} // E3, E4, E5
    { emptyMill with millPos = 18::19::20::[]} // F2, F4, F6
    { emptyMill with millPos = 21::22::23::[]} // G1, G4, G7
    { emptyMill with millPos =   0::9::21::[]} // A1, D1, G1
    { emptyMill with millPos =  3::10::18::[]} // B2, D2, F2
    { emptyMill with millPos =  6::11::15::[]} // C3, D3, E3
    { emptyMill with millPos =    1::4::7::[]} // A4, B4, C4
    { emptyMill with millPos = 16::19::22::[]} // E4, F4, G4
    { emptyMill with millPos =  8::12::17::[]} // C5, D5, E5
    { emptyMill with millPos =  5::13::20::[]} // B6, D6, F6
    { emptyMill with millPos =  2::14::23::[]} // A7, D7, G7
    { emptyMill with millPos =    0::3::6::[]} // A1, B2, C3
    { emptyMill with millPos = 15::18::21::[]} // E3, F2, G1
    { emptyMill with millPos =    2::5::8::[]} // C5, B6, A7
    { emptyMill with millPos = 17::20::23::[]} // E5, F6, G7


*/
#endregion