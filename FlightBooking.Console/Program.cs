using System;
using FlightBooking.Core;

namespace FlightBooking.Console
{
    internal class Program
    {
        private static ScheduledFlight _scheduledFlight ;

        private static void Main(string[] args)
        {
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
                    System.Console.WriteLine(_scheduledFlight.GetSummary());
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

            _scheduledFlight.SetAircraftForRoute(new Plane { Id = 123, Name = "Antonov AN-2", NumberOfSeats = 12 });
        }
    }
}
