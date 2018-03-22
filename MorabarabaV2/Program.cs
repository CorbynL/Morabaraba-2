using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorabarabaV2
{
    class Program
    {
        private Board gameBoard;

        #region The Global variables!!!

        //I'm the global cow list
        public static List<Cow> GlobalCow = new List<Cow>();

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
            printCenterLine("2. You can eliminate your opponent's cows using mills. A mill is when you get 3");
            printCenterLine("of your cows in a row (including straight diagonals). When formed, you will be asked");
            printCenterLine("which of your opponents cows you would like to kill. Enter their board-position and");
            printCenterLine("*poof* free Beef-Wellington for dinner. There are some rules to mills, such as:\n");
            printCenterLine("i. You can only form the same mill with the same cows after 2 of your turns has passed.");
            printCenterLine("ii. If you form a mill, you cannot kill cows that are in a mill already i.e. they are safe.");
            printCenterLine("iii. If you form a mill and all of your opponent's cows are in mills, then you may kill any of cow.\n");
            Console.WriteLine("");
            printCenterLine("3. WIN - When your opponent has only 2 cows left, then you win!\n");
            Console.WriteLine("");
            printCenterLine("4. If no vaild moves are available to any player, the game ends in a DRAW.\n");
            Console.WriteLine("");
            printCenterLine(" --- [ Press Enter to Begin ] --- ");

            Console.ReadKey();
        }



        static void drawboard()  // print the board         
        {
            Console.Clear();
            Console.WriteLine("");
            printCenterLine(" __  __                           _                               _              ");
            printCenterLine("|  \\/  |   ___      _ _   __ _   | |__    __ _      _ _   __ _   | |__    __ _   ");
            printCenterLine("| |\\/| |  / _ \\    | '_| / _` |  | '_ \\  / _` |    | '_| / _` |  | '_ \\  / _` |  ");
            printCenterLine("|_|__|_|  \\___/   _|_|_  \\__,_|  |_.__/  \\__,_|   _|_|_  \\__,_|  |_.__/  \\__,_|  ");
            printCenterLine("_|\"\"\"\"\"|_|\"\"\"\"\"|_|\"\"\"\"\"|_|\"\"\"\"\"|_|\"\"\"\"\"|_|\"\"\"\"\"|_|\"\"\"\"\"|_|\"\"\"\"\"|_|\"\"\"\"\"|_|\"\"\"\"\"| ");
            printCenterLine("\"`-0-0-'\"`-0-0-'\"`-0-0-'\"`-0-0-'\"`-0-0-'\"`-0-0-'\"`-0-0-'\"`-0-0-'\"`-0-0-'\"`-0-0-' ");
            Console.WriteLine("");


            printCenterLine("  1   2   3   4   5   6   7");
            printCenterLine("");
            printCenterLine(String.Format(" A   ({0})---------({1})---------({2})    ", GlobalCow[0].UserId, GlobalCow[1].UserId, GlobalCow[2].UserId));
            printCenterLine("     | \\         |         / |    ");
            printCenterLine(String.Format(" B    |  ({0})-----({1})-----({2})  |    ", GlobalCow[3].UserId, GlobalCow[4].UserId, GlobalCow[5].UserId));
            printCenterLine("     |   | \\     |     / |   |    ");
            printCenterLine(String.Format(" C    |   |  ({0})-({1})-({2})  |   |    ", GlobalCow[6].UserId, GlobalCow[7].UserId, GlobalCow[8].UserId));
            printCenterLine("     |   |   |       |   |   |    ");
            printCenterLine(String.Format(" D   ({0})-({1})-({2})     ({3})-({4})-({5})    ", GlobalCow[9].UserId, GlobalCow[10].UserId, GlobalCow[11].UserId, GlobalCow[12].UserId, GlobalCow[13].UserId, GlobalCow[14].UserId));
            printCenterLine("     |   |   |       |   |   |    ");
            printCenterLine(String.Format(" E    |   |  ({0})-({1})-({2})  |   |    ", GlobalCow[15].UserId, GlobalCow[16].UserId, GlobalCow[17].UserId));
            printCenterLine("     |   | /     |     \\ |   |    ");
            printCenterLine(String.Format(" F    |  ({0})-----({1})-----({2})  |    ", GlobalCow[18].UserId, GlobalCow[19].UserId, GlobalCow[20].UserId));
            printCenterLine("     | /         |         \\ |    ");
            printCenterLine(String.Format(" G   ({0})---------({1})---------({2})    ", GlobalCow[21].UserId, GlobalCow[22].UserId, GlobalCow[23].UserId));

        }

        #endregion

        static void gamePlay()
        {
            //Keep looping the game unless told not to 
            while (true)
            {
                startUpPrompt();

                //Populate the gobal cow list
                for (int i = 0; i < 24; i++)
                {
                    GlobalCow.Add(new Cow());           //int Position, char UserId, int CowNumber, int Id
                }

                drawboard(); //Just a test...


                Console.ReadKey(); // Just to pause while editing
            }
        }

        #region Board cow list manipulation

        // place Cow at given position
        private void placeCow(Cow cow)
        {
            while (true)
            {
                if (gameBoard.Cows[cow.Position].Position == -1) { Console.WriteLine("Cannot place cow there"); }

                else { gameBoard.Cows[cow.Position] = cow; }
                // DrawBoard 
            }
        }

        #endregion

        #region Main Function

        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.WindowWidth, 50);
            gamePlay();

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