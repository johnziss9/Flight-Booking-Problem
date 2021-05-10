using System;
using System.Linq;
using System.Collections.Generic;

namespace FlightBooking.Core
{
    public class ScheduledFlight
    {
        private readonly string _verticalWhiteSpace = Environment.NewLine + Environment.NewLine;
        private readonly string _newLine = Environment.NewLine;
        private string result = "";
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
        
        public string GetSummary(List<Plane> planes)
        {
            double costOfFlight = 0;
            double profitFromFlight = 0;
            var totalLoyaltyPointsAccrued = 0;
            var totalLoyaltyPointsRedeemed = 0;
            var totalExpectedBaggage = 0;
            var seatsTaken = 0;

            result = "Flight summary for " + FlightRoute.Title;

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
                    case (PassengerType.Discounted):
                        {
                            profitFromFlight += FlightRoute.BasePrice / 2;
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

            // Added each rule in a separate method as this will help with manipulating
            // the options given as well as being able to add further rules in the future.

            var checkSeatsTaken = CheckSeatsTaken(seatsTaken, planes);
            var checkMinPercentageExceeded = CheckMinPercentageExceeded(seatsTaken);
            var checkProfitSurplus = CheckProfitSurplus(profitSurplus);

            if (checkSeatsTaken == false || checkMinPercentageExceeded == false || checkProfitSurplus == false)
            {
                result += "THIS FLIGHT MAY NOT PROCEED.";
                result += _newLine;
            }

            return result;
        }

        public bool CheckProfitSurplus(double profitSurplus)
        {
            // If profitSurplus is less than 0 then
            // get number of employees on board and
            // get min percentage required for flight to take off

            // Check employees exceed min percentage
            // If so, return true else return false

            if (profitSurplus > 0)
                return true;
            else if (Passengers.Count(p => p.Type == PassengerType.AirlineEmployee) / (double)Aircraft.NumberOfSeats >
                    FlightRoute.MinimumTakeOffPercentage)
            {
                result += "THE REVENUE IS LESS THAN THE COST OF FLIGHT BUT FLIGHT MAY PROCEED";
                result += _newLine;

                return true;
            }
            else
                return false;
        }

        public bool CheckSeatsTaken(int seatsTaken, List<Plane> planes)
        {
            if (seatsTaken < Aircraft.NumberOfSeats)
                return true;
            else
            {
                // Check if another aircraft is available
                var availablePlanes = planes.Where(p => p.NumberOfSeats > seatsTaken);
                var noOfAvailablePlanes = availablePlanes.Count();

                // If available then suggest available aircrafts
                if (noOfAvailablePlanes > 0)
                {
                    result += "THIS FLIGHT MAY NOT PROCEED.";
                    result += _newLine;
                    result += "Other more suitable aircrafts are:";

                    foreach (var plane in availablePlanes)
                    {
                        result += _newLine;
                        result += plane.Name + "could handle this flight.";
                    }
                }

                return false;
            }
        }

        public bool CheckMinPercentageExceeded(int seatsTaken)
        {
            if (seatsTaken / (double)Aircraft.NumberOfSeats > FlightRoute.MinimumTakeOffPercentage)
                return true;
            else
                return false;
        }

    }
}
