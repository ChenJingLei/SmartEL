using System;

namespace SmartEL.Model
{
    class RoomControl
    {
        private string _classroomId = "0000";
        private DateTime _date;
        private string _plan12 = "L01#0/L02#0/L03#0/T01#0.0/T02#0.0/H01#0.0/F01#0/F02#0/I01#0.0";
        private string _plan34 = "L01#0/L02#0/L03#0/T01#0.0/T02#0.0/H01#0.0/F01#0/F02#0/I01#0.0";
        private string _plan56 = "L01#0/L02#0/L03#0/T01#0.0/T02#0.0/H01#0.0/F01#0/F02#0/I01#0.0";
        private string _plan78 = "L01#0/L02#0/L03#0/T01#0.0/T02#0.0/H01#0.0/F01#0/F02#0/I01#0.0";
        private string _plan912 = "L01#0/L02#0/L03#0/T01#0.0/T02#0.0/H01#0.0/F01#0/F02#0/I01#0.0";

        public string ClassroomId
        {
            get
            {
                return _classroomId;
            }

            set
            {
                _classroomId = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return _date;
            }

            set
            {
                _date = value;
            }
        }

        public string Plan12
        {
            get
            {
                return _plan12;
            }

            set
            {
                _plan12 = value;
            }
        }

        public string Plan34
        {
            get
            {
                return _plan34;
            }

            set
            {
                _plan34 = value;
            }
        }

        public string Plan56
        {
            get
            {
                return _plan56;
            }

            set
            {
                _plan56 = value;
            }
        }

        public string Plan78
        {
            get
            {
                return _plan78;
            }

            set
            {
                _plan78 = value;
            }
        }

        public string Plan912
        {
            get
            {
                return _plan912;
            }

            set
            {
                _plan912 = value;
            }
        }
    }
}
