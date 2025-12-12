using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_4
{
    internal class Ride
    {
        private string _name;
        private int _frightFactor;
        private double _costToEnter;
        private int _visitorsToday;

        public Ride() { }

        public Ride(string name, int frightFactor, double costToEnter, int visitorsToday)
        {
            Name = name;
            FrightFactor = frightFactor;
            CostToEnter = costToEnter;
            VisitorsToday = visitorsToday;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ride name cannot be empty.");

                _name = value.Trim();
            }
        }

        public int FrightFactor
        {
            get { return _frightFactor; }
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("Fright factor must be between 0 and 100.");

                _frightFactor = value;
            }
        }

        public double CostToEnter
        {
            get { return _costToEnter; }
            set
            {
                if (value < 1.00)
                    throw new ArgumentException("Cost must be at least $1.00.");

                _costToEnter = value;
            }
        }

        public int VisitorsToday
        {
            get { return _visitorsToday; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Visitors must be 0 or greater.");

                _visitorsToday = value;
            }
        }

        public double PopularityScore
        {
            get { return (FrightFactor / 10.0) * VisitorsToday; }
        }

        public string ThrillLevel
        {
            get
            {
                if (FrightFactor <= 20) return "Mild";
                if (FrightFactor <= 60) return "Exciting";
                if (FrightFactor <= 90) return "Thrilling";
                return "Extreme";
            }
        }

        public string RideDetails()
        {
            return
                $"Name:            {Name}\n" +
                $"Fright Factor:   {FrightFactor}\n" +
                $"Cost to Enter:   {CostToEnter:C}\n" +
                $"Visitors Today:  {VisitorsToday}\n" +
                $"Thrill Level:    {ThrillLevel}\n" +
                $"PopularityScore:{PopularityScore:F2}\n";
        }

        public string ToCSVString()
        {
            return $"{Name},{FrightFactor},{CostToEnter},{VisitorsToday}";
        }

        public override string ToString()
        {
            return $"{Name,-12}{FrightFactor,-8}{CostToEnter,-8:F2}{VisitorsToday}";
        }
    }
}
