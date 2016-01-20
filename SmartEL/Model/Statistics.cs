using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartEL.Model
{
    public class Statistics
    {
        private int _id;
        private string _classroomId;
        private string _classroomName;
        private DateTime _date;
        private double _temperature;
        private double _humidity;

        public Statistics(string classroomId, DateTime date, double temperature, double humidity)
        {
            _classroomId = classroomId;
            _date = date;
            _temperature = temperature;
            _humidity = humidity;
        }

        public Statistics()
        {
        }

        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
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

        public double Temperature
        {
            get
            {
                return _temperature;
            }

            set
            {
                _temperature = value;
            }
        }

        public double Humidity
        {
            get
            {
                return _humidity;
            }

            set
            {
                _humidity = value;
            }
        }

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

        public string ClassroomName
        {
            get
            {
                return _classroomName;
            }

            set
            {
                _classroomName = value;
            }
        }
    }
}
