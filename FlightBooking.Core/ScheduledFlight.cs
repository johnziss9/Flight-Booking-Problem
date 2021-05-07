using System;
using System.Linq;
using System.Collections.Generic;

namespace FlightBooking.Core
{
    public class ScheduledFlight
    {
        private readonly string _verticalWhiteSpace = Environment.NewLine + Environment.NewLine;
        private readonly string _newLine = Environment.NewLine;
        private const string Indentation = "    ";

        public ScheduledFlight(FlightRoute flightRoute)
        {
            FlightRoute = flightRoute;
            Passengers = new List<Passenger>();
        }

        public FlightRoute FlightRoute { get; }
        public Plane Aircraft { get; private set; }
        public List<Passenger> Passengers { get; }

        public void AddPassenger(Passenger passenger)
        {
            Passengers.Add(passenger);
        }

        public void SetAircraftForRoute(Plane aircraft)
        {
            Aircraft = aircraft;
        }
        
        public string GetSummary()
        {
            double costOfFlight = 0;
            double profitFromFlight = 0;
            var totalLoyaltyPointsAccrued = 0;
            var totalLoyaltyPointsRedeemed = 0;
            var totalExpectedBaggage = 0;
            var seatsTaken = 0;

            var result = "Flight summary for " + FlightRoute.Title;

            foreach (var passenger in Passengers)
            {
                switch (passenger.Type)
                {
                    case(PassengerType.General):
                        {
                            profitFromFlight += FlightRoute.BasePrice;
                            totalExpectedBaggage++;
                            break;
                        }
                    case(PassengerType.LoyaltyMember):
                        {
                            if (passenger.IsUsingLoyaltyPoints)
                            {
                                var loyaltyPointsRedeemed = Convert.ToInt32(Math.Ceiling(FlightRoute.BasePrice));
                                passenger.LoyaltyPoints -= loyaltyPointsRedeemed;
                                totalLoyaltyPointsRedeemed += loyaltyPointsRedeemed;
                            }
                            else
                            {
                                totalLoyaltyPointsAccrued += FlightRoute.LoyaltyPointsGained;
                                profitFromFlight += FlightRoute.BasePrice;                           
                            }
                            totalExpectedBaggage += 2;
                            break;
                        }
                    case(PassengerType.AirlineEmployee):
                        {
                            totalExpectedBaggage += 1;
                            break;
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                costOfFlight += FlightRoute.BaseCost;
                seatsTaken++;
            }

            result += _verticalWhiteSpace;
            
            result += "Total passengers: " + seatsTaken;
            result += _newLine;
            result += Indentation + "General sales: " + Passengers.Count(p => p.Type == PassengerType.General);
            result += _newLine;
            result += Indentation + "Loyalty member sales: " + Passengers.Count(p => p.Type == PassengerType.LoyaltyMember);
            result += _newLine;
            result += Indentation + "Airline employee comps: " + Passengers.Count(p => p.Type == PassengerType.AirlineEmployee);
            
            result += _verticalWhiteSpace;
            result += "Total expected baggage: " + totalExpectedBaggage;

            result += _verticalWhiteSpace;

            result += "Total revenue from flight: " + profitFromFlight;
            result += _newLine;
            result += "Total costs from flight: " + costOfFlight;
            result += _newLine;

            var profitSurplus = profitFromFlight - costOfFlight;

            result += (profitSurplus > 0 ? "Flight generating profit of: " : "Flight losing money of: ") + profitSurplus;

            result += _verticalWhiteSpace;

            result += "Total loyalty points given away: " + totalLoyaltyPointsAccrued + _newLine;
            result += "Total loyalty points redeemed: " + totalLoyaltyPointsRedeemed + _newLine;

            result += _verticalWhiteSpace;

            if (profitSurplus > 0 &&
                seatsTaken < Aircraft.NumberOfSeats &&
                seatsTaken / (double) Aircraft.NumberOfSeats > FlightRoute.MinimumTakeOffPercentage)
            {
                result += "THIS FLIGHT MAY PROCEED";
            }
            else
            {
                result += "FLIGHT MAY NOT PROCEED";
            }
            
            return result;
        }
    }
}
