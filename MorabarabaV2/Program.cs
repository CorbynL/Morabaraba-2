using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorabarabaV2
{
    public class Program
    {

        #region The Global variables!!!
        static private Board gameBoard;
        
        #endregion


        #region Writing to the screen

        static void printCenterLine(string line)
        {
            Console.SetCursorPosition((Console.WindowWidth - line.Length) / 2, Console.CursorTop);
            Console.WriteLine(line);

        }

        static void startUpPrompt()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            printCenterLine("   *                                                        ");
            printCenterLine(" (  `                       )                      )        ");
            printCenterLine(" )\\))(        (       )  ( /(     )  (       )  ( /(     )  ");
            printCenterLine("((_)()\\   (   )(   ( /(  )\\()) ( /(  )(   ( /(  )\\()) ( /(  ");
            printCenterLine("(_()((_)  )\\ (()\\  )(_))((_)\\  )(_))(()\\  )(_))((_)\\  )(_)) ");
            printCenterLine("|  \\/  | ((_) ((_)((_)_ | |(_)((_)_  ((_)((_)_ | |(_)((_)_  ");
            printCenterLine("| |\\/| |/ _ \\| '_|/ _` || '_ \\/ _` || '_|/ _` || '_ \\/ _` | ");
            printCenterLine("|_|  |_|\\___/|_|  \\__,_||_.__/\\__,_||_|  \\__,_||_.__/\\__,_| ");
            Console.WriteLine("\n\n\n(");
            printCenterLine(" ------------------- Let the Games Begin! ------------------- ");
            Console.WriteLine("\n\n");
            printCenterLine(" ---- [ Rules ] ---- ");
            Console.WriteLine("");
            printCenterLine("1. Morabaraba consists of a board with pieces which we refer to as 'cows'.");
            printCenterLine("Each player starts with 12 cows and the game consists of 3 phases:");
            printCenterLine("Placing, Moving and Flying.\n");
            printCenterLine("i. Placing - Place one of your 12 cows you start with at a board-position.");
            printCenterLine("ii. Moving - Once you have placed your 12 cows, move your cow to any empty");
            printCenterLine("position neighbouring your cow.");
            printCenterLine("iii. Flying - When you have 3 cows left, you can then move your cow to any");
            printCenterLine("empty position on the board you feel like.\n");
            Console.WriteLine("");
            printCenterLine("2. You can eliminate your opponent's cows using mills.\n");
            printCenterLine(" A mill is when you get 3");
            printCenterLine("of your cows in a row (including straight diagonals).\n");
            printCenterLine("When formed, you will be asked");
            printCenterLine("which of your opponents cows you would like to kill. Enter their");
            printCenterLine("their board-position and");
            printCenterLine("*poof* free Beef-Wellington for dinner. There are some\n");
            printCenterLine("rules to mills, such as:\n");
            printCenterLine("i. You can only form the same mill with the same cows\n");
            printCenterLine("after 2 of your turns has passed.");
            printCenterLine("ii. If you form a mill, you cannot kill cows that are in a\n");
            printCenterLine("mill already i.e. they are safe.");
            printCenterLine("iii. If you form a mill and all of your opponent's cows are in\n");
            printCenterLine("in mills, then you may kill any of cow.\n");
            Console.WriteLine("");
            printCenterLine("3. WIN - When your opponent has only 2 cows left, then you win!\n");
            Console.WriteLine("");
            printCenterLine("4. If no vaild moves are available to any player, the game ends in a DRAW.\n");
            Console.WriteLine("");
            printCenterLine(" --- [ Press Enter to Begin ] --- ");

            Console.ReadKey();
        }

        #endregion

    #region input functions
        // Get Board coordinate from user input
        static private int converToBoardPos(string input)
    {
        switch (input)
        {
            case "a1": return 0;
            case "a4": return 1;
            case "a7": return 2;
            case "b2": return 3;
            case "b4": return 4;
            case "b6": return 5;
            case "c3": return 6;
            case "c4": return 7;
            case "c5": return 8;
            case "d1": return 9;
            case "d2": return 10;
            case "d3": return 11;
            case "d5": return 12;
            case "d6": return 13;
            case "d7": return 14;
            case "e3": return 15;
            case "e4": return 16;
            case "e5": return 17;
            case "f2": return 18;
            case "f4": return 19;
            case "f6": return 20;
            case "g1": return 21;
            case "g4": return 22;
            case "g7": return 23;
            default: return -1;
        }
    }

    #endregion
    #region player functions
    private static char getPlayerChar(int playerID)
    {
        if (playerID == 0) { return 'R'; }
        else return 'B';
    }
    private static int switchPlayer(int playerID)
        {
            if (playerID == 0) { return 1; }
            else return 0;
        }

    #endregion
    #region Cow List functions
        // Place cows on board (Phase 1)
        static private void placeCows(int playerID)
        {
            int i = 0;
            while(i < 24)
            {
                updateMills(playerID);

                Console.WriteLine("Where do you want to place a cow?");
                int input = converToBoardPos(Console.ReadLine().ToLower());
                while (input == -1)
                {
                    gameBoard.drawboard();
                    Console.WriteLine("Incorrect input!");
                    input = converToBoardPos(Console.ReadLine().ToLower());
                }
                if (!(gameBoard.Cows[input].Id == -1) || gameBoard.Cows[input].Id == switchPlayer(playerID))
                {
                    gameBoard.drawboard();
                    Console.WriteLine("Cannot place there!");
                    continue;
                } 

                gameBoard.Cows[input] = new Cow(input, getPlayerChar(playerID), i, playerID);

                getCurrentMills(playerID);

                if (areNewMills(playerID))
                {
                    killCow(playerID);
                }

                playerID = switchPlayer(playerID);
                gameBoard.drawboard();
                i = i + 1;
            }
        }

    
        static private bool canKill(int position, int playerID)
    {
            if (gameBoard.Cows[position].Id == playerID
                || gameBoard.Cows[position].Id == -1) { return false; }
            return true;
    }

        static private void killCow(int playerID)
        {
            Console.WriteLine("Chose a cow to kill");

            int input = converToBoardPos(Console.ReadLine().ToLower());
            while(!canKill(input, playerID))
            {
                Console.WriteLine("Cannot kill that!");
                input = converToBoardPos(Console.ReadLine().ToLower());
            }
            gameBoard.Cows[input].UserId = ' ';
            gameBoard.Cows[input].Id = -1;

        }
    #endregion

    #region Mill functions

    static private void updateMills(int playerID)
    {
        foreach(Mill mill in gameBoard.Mills)
        {
            if(mill.Id == playerID 
                && mill.isNew) { mill.isNew = false; }
        }
    }

static private bool areNewMills(int playerID)
    {
        foreach(Mill mill in gameBoard.Mills)
        {
            if(mill.Id == playerID && mill.isNew) { return true; }
        }
        return false;
    }

static private bool areInMill(int[] cows, int playerID)
    {
        return gameBoard.Cows[cows[0]].Id == playerID
            && gameBoard.Cows[cows[1]].Id == playerID
            && gameBoard.Cows[cows[2]].Id == playerID;
    }

static private void getCurrentMills(int playerID)
    {
        foreach(Mill mill in gameBoard.Mills)
        {
            if (areInMill(mill.Positions, playerID) && mill.Id != playerID)
            {
                mill.isNew = true;
                mill.Id = playerID;
            }
        }
    }
        #endregion

    // Main Loop
    static void startLoop()
    {
        startUpPrompt();
        gameBoard.drawboard();
        //Keep looping the game unless told not to 
        while (true)
        {
            // 0 = player 1
            placeCows(0);
            break; // Just to pause while editing
        }
    }

        #region Main Function

        static void Main(string[] args)
        {
            gameBoard = new Board();
            Console.SetWindowSize(Console.WindowWidth, 50);
            startLoop();

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