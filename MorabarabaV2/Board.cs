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
            {
                Cows[i] = new Cow(i);
                Mills[i] = new Mill(new Cow[0]);
            }
            
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

    }
}
