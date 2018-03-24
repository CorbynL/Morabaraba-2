using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorabarabaV2
{ 
    public class Cow : BaseNotificationClass
    {
        private int _position;
        private char _userId;
        private int _cowNumber;
        private int _Id;
        private string _imageName;

        public int Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }
        public char UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }
        public int CowNumber
        {
            get { return _cowNumber; }
            set
            {
                _cowNumber = value;
                OnPropertyChanged(nameof(CowNumber));
            }
        }
        public string ImageName
        {
            get { return _imageName; }
            set
            {
                _imageName = value;
                OnPropertyChanged(nameof(ImageName));
            }
        }
        public int Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public Cow(int Position = -1, char UserId = ' ', int CowNumber = -1, int Id = -1, string ImageName = "/Gui;component/Images/deadCow.png")
        {
            this.Position = Position;
            this.UserId = UserId;
            this.CowNumber = CowNumber;
            this.Id = Id;
            this.ImageName = ImageName;
        }


        //Do things


    }
}
