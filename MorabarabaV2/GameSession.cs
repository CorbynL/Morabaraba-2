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
       public Board board
        {
            get { return _board; }
            set
            {
                _board = value;
                OnPropertyChanged(nameof(board));
            }
        }

        public GameSession()
        {
            board = new Board();
            board.CreateEmptyMills();
        }
    }
}
