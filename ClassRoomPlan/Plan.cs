using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassRoomPlan
{
    public class Plan
    {
        private Classroom _room;
        private DateTime _time;
        private int _one;
        private int _two;
        private int _three;
        private int _four;
        private int _five;

        public Classroom Room
        {
            get
            {
                return _room;
            }

            set
            {
                _room = value;
            }
        }

        public DateTime Time
        {
            get
            {
                return _time;
            }

            set
            {
                _time = value;
            }
        }

        public int One
        {
            get
            {
                return _one;
            }

            set
            {
                _one = value;
            }
        }

        public int Two
        {
            get
            {
                return _two;
            }

            set
            {
                _two = value;
            }
        }

        public int Three
        {
            get
            {
                return _three;
            }

            set
            {
                _three = value;
            }
        }

        public int Four
        {
            get
            {
                return _four;
            }

            set
            {
                _four = value;
            }
        }

        public int Five
        {
            get
            {
                return _five;
            }

            set
            {
                _five = value;
            }
        }
    }
}
