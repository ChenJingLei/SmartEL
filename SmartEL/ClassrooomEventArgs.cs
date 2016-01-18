using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassRoomPlan;

namespace SmartEL
{
    public class ClassrooomEventArgs : EventArgs
    {
        private List<Classroom> _rooms;
        private int _num;

        public List<Classroom> Rooms
        {
            get
            {
                return _rooms;
            }

            set
            {
                _rooms = value;
            }
        }

        public int Num
        {
            get
            {
                return _num;
            }

            set
            {
                _num = value;
            }
        }
    }
}
