using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorabarabaV2
{
    public class Mill : BaseNotificationClass
    {
        private bool _isNew;
        private int _id;

        public int[] Positions { get; set; }
        public bool isNew
        {
            get { return _isNew; }
            set
            {
                _isNew = value;
                OnPropertyChanged(nameof(isNew));
            }
        }
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public Mill(int[] Positions, int Id = -1)
        {
            this.Positions = Positions;
            isNew = false;
            this.Id = Id;
        }
    }
}
