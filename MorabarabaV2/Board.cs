using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorabarabaV2
{
    public class Board
    {
        public Cow[] Cows;
        public Mill[] Mills;


        public Board(Cow[] cows)
        {
            Cows = new Cow[24];
            Mills = new Mill[24];

            for (int i = 0; i < 24; i++)            
                Cows[i] = new Cow(i);               
            
            
        }

        public Board()
        {
            Cows = new Cow[24];
            for (int i = 0; i < 24; i++)
                Cows[i] = new Cow(i);

        }

        // Get an empty cow at a given position (if empty at all)
        public Cow Empty (int i)
        {
            if (i < 0 || 23 < i)
                return null;

            if (Cows[i].Id == -1)
                return Cows[i];

            else return null;
        }


        static void printCenterLine(string line)
        {
            Console.SetCursorPosition((Console.WindowWidth - line.Length) / 2, Console.CursorTop);
            Console.WriteLine(line);

        }

        public void drawboard()  // print the board         
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
            printCenterLine(String.Format(" A   ({0})---------({1})---------({2})    ", Cows[0].UserId, Cows[1].UserId, Cows[2].UserId));
            printCenterLine("     | \\         |         / |    ");
            printCenterLine(String.Format(" B    |  ({0})-----({1})-----({2})  |    ", Cows[3].UserId, Cows[4].UserId, Cows[5].UserId));
            printCenterLine("     |   | \\     |     / |   |    ");
            printCenterLine(String.Format(" C    |   |  ({0})-({1})-({2})  |   |    ", Cows[6].UserId, Cows[7].UserId, Cows[8].UserId));
            printCenterLine("     |   |   |       |   |   |    ");
            printCenterLine(String.Format(" D   ({0})-({1})-({2})     ({3})-({4})-({5})    ", Cows[9].UserId, Cows[10].UserId, Cows[11].UserId, Cows[12].UserId, Cows[13].UserId, Cows[14].UserId));
            printCenterLine("     |   |   |       |   |   |    ");
            printCenterLine(String.Format(" E    |   |  ({0})-({1})-({2})  |   |    ", Cows[15].UserId, Cows[16].UserId, Cows[17].UserId));
            printCenterLine("     |   | /     |     \\ |   |    ");
            printCenterLine(String.Format(" F    |  ({0})-----({1})-----({2})  |    ", Cows[18].UserId, Cows[19].UserId, Cows[20].UserId));
            printCenterLine("     | /         |         \\ |    ");
            printCenterLine(String.Format(" G   ({0})---------({1})---------({2})    ", Cows[21].UserId, Cows[22].UserId, Cows[23].UserId));

        }

    }
}
