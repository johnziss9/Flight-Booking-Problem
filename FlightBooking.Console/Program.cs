using System;
using System.Collections.Generic;
using FlightBooking.Core;

namespace FlightBooking.Console
{
    internal class Program
    {
        private static ScheduledFlight _scheduledFlight ;
        private static List<Plane> _planes;

        private static void Main(string[] args)
        {
            GetPlanes();
            SetupAirlineData();
            
            string command;
            do
            {
                System.Console.WriteLine("Please enter command.");
                command = System.Console.ReadLine() ?? "";
                var enteredText = command.ToLower();
                if (enteredText.Contains("print summary"))
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine(_scheduledFlight.GetSummary(_planes));
                }
                else if (enteredText.Contains("add general"))
                {
                    var passengerSegments = enteredText.Split(' ');
                    _scheduledFlight.AddPassenger(new Passenger
                    {
                        Type = PassengerType.General, 
                        Name = passengerSegments[2], 
                        Age = Convert.ToInt32(passengerSegments[3])
                    });
                }
                else if (enteredText.Contains("add loyalty"))
                {
                    var passengerSegments = enteredText.Split(' ');
                    _scheduledFlight.AddPassenger(new Passenger
                    {
                        Type = PassengerType.LoyaltyMember, 
                        Name = passengerSegments[2], 
                        Age = Convert.ToInt32(passengerSegments[3]),
                        LoyaltyPoints = Convert.ToInt32(passengerSegments[4]),
                        IsUsingLoyaltyPoints = Convert.ToBoolean(passengerSegments[5]),
                    });
                }
                else if (enteredText.Contains("add airline"))
                {
                    var passengerSegments = enteredText.Split(' ');
                    _scheduledFlight.AddPassenger(new Passenger
                    {
                        Type = PassengerType.AirlineEmployee, 
                        Name = passengerSegments[2], 
                        Age = Convert.ToInt32(passengerSegments[3]),
                    });
                }
                else if (enteredText.Contains("exit"))
                {
                    Environment.Exit(1);
                }
                else
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine("UNKNOWN INPUT");
                    System.Console.ResetColor();
                }
            } while (command != "exit");
        }

        // Created a new method in which planes can be added into a list
        // which is called before SetupAirlineData()
        private static void GetPlanes()
        {
            _planes = new List<Plane>();

            _planes.Add(new Plane { Id = 123, Name = "Antonov AN-2", NumberOfSeats = 12 });
            _planes.Add(new Plane { Id = 124, Name = "Vader DV-9", NumberOfSeats = 10 });
            _planes.Add(new Plane { Id = 125, Name = "HanSolo HS-3", NumberOfSeats = 15 });
            _planes.Add(new Plane { Id = 126, Name = "Leia LE-1", NumberOfSeats = 13 });
        }

        private static void SetupAirlineData()
        {
            var londonToParis = new FlightRoute("London", "Paris")
            {
                BaseCost = 50, 
                BasePrice = 100, 
                LoyaltyPointsGained = 5,
                MinimumTakeOffPercentage = 0.7
            };

            _scheduledFlight = new ScheduledFlight(londonToParis);

            _scheduledFlight.SetAircraftForRoute(_planes[0]);
        }
    }
}
