using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorabarabaV2
{ 
    public class Cow
    {
        public int Position { get; set; }
        public char UserId { get; set; }
        public int CowNumber { get; set; }
        public int Id { get; set; }

        public Cow(int Position = -1, char UserId = ' ', int CowNumber = -1, int Id = -1)
        {
            this.Position = Position;
            this.UserId = UserId;
            this.CowNumber = CowNumber;
            this.Id = Id;

        }


        //Do things


    }
}
