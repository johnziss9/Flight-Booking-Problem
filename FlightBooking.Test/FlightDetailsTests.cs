using Xunit;
using FlightBooking.Core;
using System.Linq;

namespace FlightBooking.Test
{
    public class FlightDetailsTests
    {
        [Fact]
        public void Calculate_Cost_And_Profit_Of_Flight()
        {
            var madridToPrague = new FlightRoute("Madrid", "Prague")
            {
                BaseCost = 80,
                BasePrice = 150,
                LoyaltyPointsGained = 10,
                MinimumTakeOffPercentage = 0.5
            };

            var scheduledFlight = new ScheduledFlight(madridToPrague);

            scheduledFlight.SetAircraftForRoute(new Plane { Id = 124, Name = "Vader Darth-87", NumberOfSeats = 10 });

            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "John", Age = 32, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "Jen", Age = 28, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "Tom", Age = 47, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Mike", Age = 26, LoyaltyPoints = 1500, IsUsingLoyaltyPoints = true, AllowedBags = 2 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Pan", Age = 56, LoyaltyPoints = 500, IsUsingLoyaltyPoints = false, AllowedBags = 2 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.AirlineEmployee, Name = "Natalie", Age = 30, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.Discounted, Name = "Chris", Age = 38, AllowedBags = 0 });

            var costOfFlight = scheduledFlight.Passengers.Count * madridToPrague.BaseCost;

            var totalPriceForGeneral = scheduledFlight.Passengers.Where(p => p.Type == PassengerType.General).Count() * madridToPrague.BasePrice;
            var totalPriceForNonUsingLoyaltyPoints = scheduledFlight.Passengers.Where(p => p.Type == PassengerType.LoyaltyMember && p.IsUsingLoyaltyPoints == false).Count() * madridToPrague.BasePrice;
            var totalPriceForDiscounted = (scheduledFlight.Passengers.Where(p => p.Type == PassengerType.Discounted).Count() * madridToPrague.BasePrice) / 2;

            var profitFromFlight = (totalPriceForGeneral + totalPriceForNonUsingLoyaltyPoints + totalPriceForDiscounted) - costOfFlight;

            Assert.Equal(560, costOfFlight);
            Assert.Equal(115, profitFromFlight);
        }

        [Fact]
        public void Calculate_Loyalty_Points()
        {
            var madridToPrague = new FlightRoute("Madrid", "Prague")
            {
                BaseCost = 80,
                BasePrice = 150,
                LoyaltyPointsGained = 10,
                MinimumTakeOffPercentage = 0.5
            };

            var scheduledFlight = new ScheduledFlight(madridToPrague);

            scheduledFlight.SetAircraftForRoute(new Plane { Id = 124, Name = "Vader Darth-87", NumberOfSeats = 10 });

            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "John", Age = 32, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "Jen", Age = 28, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "Tom", Age = 47, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Mike", Age = 26, LoyaltyPoints = 1500, IsUsingLoyaltyPoints = true, AllowedBags = 2 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Pan", Age = 56, LoyaltyPoints = 500, IsUsingLoyaltyPoints = false, AllowedBags = 2 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.AirlineEmployee, Name = "Natalie", Age = 30, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.Discounted, Name = "Chris", Age = 38, AllowedBags = 0 });

            var totalLoyaltyPointsAccrued = scheduledFlight.Passengers.Where(p => p.Type == PassengerType.LoyaltyMember && p.IsUsingLoyaltyPoints == false).Count() * madridToPrague.LoyaltyPointsGained;
            var totalLoyaltyPointsRedeemed = scheduledFlight.Passengers.Where(p => p.Type == PassengerType.LoyaltyMember && p.IsUsingLoyaltyPoints == true).Count() * madridToPrague.BasePrice; ;
            
            Assert.Equal(10, totalLoyaltyPointsAccrued);
            Assert.Equal(150, totalLoyaltyPointsRedeemed);
        }

        [Fact]
        public void Calculate_Expected_Baggage()
        {
            var madridToPrague = new FlightRoute("Madrid", "Prague")
            {
                BaseCost = 80,
                BasePrice = 150,
                LoyaltyPointsGained = 10,
                MinimumTakeOffPercentage = 0.5
            };

            var scheduledFlight = new ScheduledFlight(madridToPrague);

            scheduledFlight.SetAircraftForRoute(new Plane { Id = 124, Name = "Vader Darth-87", NumberOfSeats = 10 });

            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "John", Age = 32, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "Jen", Age = 28, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "Tom", Age = 47, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Mike", Age = 26, LoyaltyPoints = 1500, IsUsingLoyaltyPoints = true, AllowedBags = 2 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Pan", Age = 56, LoyaltyPoints = 500, IsUsingLoyaltyPoints = false, AllowedBags = 2 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.AirlineEmployee, Name = "Natalie", Age = 30, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.Discounted, Name = "Chris", Age = 38, AllowedBags = 0 });

            var totalExpectedBaggage = (scheduledFlight.Passengers.Where(p => p.Type == PassengerType.LoyaltyMember).Count() * 2) + scheduledFlight.Passengers.Where(p => p.Type != PassengerType.LoyaltyMember).Count();

            Assert.Equal(9, totalExpectedBaggage);
        }

        [Fact]
        public void Calculate_Seats_Taken()
        {
            var madridToPrague = new FlightRoute("Madrid", "Prague")
            {
                BaseCost = 80,
                BasePrice = 150,
                LoyaltyPointsGained = 10,
                MinimumTakeOffPercentage = 0.5
            };

            var scheduledFlight = new ScheduledFlight(madridToPrague);

            scheduledFlight.SetAircraftForRoute(new Plane { Id = 124, Name = "Vader Darth-87", NumberOfSeats = 10 });

            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "John", Age = 32, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "Jen", Age = 28, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "Tom", Age = 47, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Mike", Age = 26, LoyaltyPoints = 1500, IsUsingLoyaltyPoints = true, AllowedBags = 2 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Pan", Age = 56, LoyaltyPoints = 500, IsUsingLoyaltyPoints = false, AllowedBags = 2 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.AirlineEmployee, Name = "Natalie", Age = 30, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.Discounted, Name = "Chris", Age = 38, AllowedBags = 0 });

            var seatsTaken = scheduledFlight.Passengers.Count();

            Assert.Equal(7, seatsTaken);
        }
    }
}
