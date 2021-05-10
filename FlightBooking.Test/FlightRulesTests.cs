using Xunit;
using FlightBooking.Core;
using System.Linq;
using System.Collections.Generic;

namespace FlightBooking.Test
{
    public class FlightRulesTests
    {
        [Fact]
        public void Check_Seat_Availability()
        {
            List<Plane> planes = new List<Plane>();

            planes.Add(new Plane { Id = 123, Name = "Golden C3P-O", NumberOfSeats = 3 });
            planes.Add(new Plane { Id = 124, Name = "Stormtrooper FN-11", NumberOfSeats = 5 });

            var madridToPrague = new FlightRoute("Madrid", "Prague")
            {
                BaseCost = 80,
                BasePrice = 150,
                LoyaltyPointsGained = 10,
                MinimumTakeOffPercentage = 0.5
            };

            var scheduledFlight = new ScheduledFlight(madridToPrague);

            scheduledFlight.SetAircraftForRoute(planes[0]);

            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "George", Age = 23, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Luke", Age = 33, LoyaltyPoints = 1500, IsUsingLoyaltyPoints = true, AllowedBags = 2 });

            var seatsTaken = scheduledFlight.Passengers.Count();

            Assert.True(scheduledFlight.CheckSeatsTaken(seatsTaken, planes));
        }

        [Fact]
        public void Check_If_Plane_Is_Full()
        {
            List<Plane> planes = new List<Plane>();

            planes.Add(new Plane { Id = 123, Name = "Golden C3P-O", NumberOfSeats = 3 });
            planes.Add(new Plane { Id = 124, Name = "Stormtrooper FN-11", NumberOfSeats = 5 });

            var madridToPrague = new FlightRoute("Madrid", "Prague")
            {
                BaseCost = 80,
                BasePrice = 150,
                LoyaltyPointsGained = 10,
                MinimumTakeOffPercentage = 0.5
            };

            var scheduledFlight = new ScheduledFlight(madridToPrague);

            scheduledFlight.SetAircraftForRoute(planes[0]);

            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "George", Age = 23, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "Steph", Age = 25, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "Alex", Age = 50, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Luke", Age = 33, LoyaltyPoints = 1500, IsUsingLoyaltyPoints = true, AllowedBags = 2 });

            var seatsTaken = scheduledFlight.Passengers.Count();

            Assert.False(scheduledFlight.CheckSeatsTaken(seatsTaken, planes));
        }

        [Fact]
        public void Minimum_Percentage_Exceeded()
        {
            List<Plane> planes = new List<Plane>();

            planes.Add(new Plane { Id = 123, Name = "Golden C3P-O", NumberOfSeats = 6 });
            planes.Add(new Plane { Id = 124, Name = "Stormtrooper FN-11", NumberOfSeats = 8 });

            var madridToPrague = new FlightRoute("Madrid", "Prague")
            {
                BaseCost = 80,
                BasePrice = 150,
                LoyaltyPointsGained = 10,
                MinimumTakeOffPercentage = 0.5
            };

            var scheduledFlight = new ScheduledFlight(madridToPrague);

            scheduledFlight.SetAircraftForRoute(planes[0]);

            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "George", Age = 23, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "Steph", Age = 25, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "Alex", Age = 50, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Luke", Age = 33, LoyaltyPoints = 1500, IsUsingLoyaltyPoints = true, AllowedBags = 2 });

            var seatsTaken = scheduledFlight.Passengers.Count();

            Assert.True(scheduledFlight.CheckMinPercentageExceeded(seatsTaken));
        }

        [Fact]
        public void Minimum_Percentage_Not_Exceeded()
        {
            List<Plane> planes = new List<Plane>();

            planes.Add(new Plane { Id = 123, Name = "Golden C3P-O", NumberOfSeats = 6 });
            planes.Add(new Plane { Id = 124, Name = "Stormtrooper FN-11", NumberOfSeats = 8 });

            var madridToPrague = new FlightRoute("Madrid", "Prague")
            {
                BaseCost = 80,
                BasePrice = 150,
                LoyaltyPointsGained = 10,
                MinimumTakeOffPercentage = 0.5
            };

            var scheduledFlight = new ScheduledFlight(madridToPrague);

            scheduledFlight.SetAircraftForRoute(planes[0]);

            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "George", Age = 23, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "Steph", Age = 25, AllowedBags = 1 });

            var seatsTaken = scheduledFlight.Passengers.Count();

            Assert.False(scheduledFlight.CheckMinPercentageExceeded(seatsTaken));
        }

        //public bool CheckProfitSurplus(double profitSurplus)
        //{
        //    if (profitSurplus > 0)
        //        return true;
        //    else if (Passengers.Count(p => p.Type == PassengerType.AirlineEmployee) / (double)Aircraft.NumberOfSeats >
        //            FlightRoute.MinimumTakeOffPercentage)
        //    {
        //        result += "THE REVENUE IS LESS THAN THE COST OF FLIGHT BUT FLIGHT MAY PROCEED";
        //        result += _newLine;

        //        return true;
        //    }
        //    else
        //        return false;
        //}

        [Fact]
        public void Profit_Surplus_Is_Positive()
        {
            var madridToPrague = new FlightRoute("Madrid", "Prague")
            {
                BaseCost = 80,
                BasePrice = 150,
                LoyaltyPointsGained = 10,
                MinimumTakeOffPercentage = 0.3
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

            var seatsTaken = scheduledFlight.Passengers.Count();

            Assert.True(scheduledFlight.CheckProfitSurplus(profitFromFlight));
        }

        [Fact]
        public void Profit_Surplus_Is_Negative_But_Airline_Employees_Exceed_Minimum_Percentage()
        {
            var madridToPrague = new FlightRoute("Madrid", "Prague")
            {
                BaseCost = 80,
                BasePrice = 150,
                LoyaltyPointsGained = 10,
                MinimumTakeOffPercentage = 0.3
            };

            var scheduledFlight = new ScheduledFlight(madridToPrague);

            scheduledFlight.SetAircraftForRoute(new Plane { Id = 124, Name = "Vader Darth-87", NumberOfSeats = 10 });

            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.AirlineEmployee, Name = "John", Age = 32, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.AirlineEmployee, Name = "Jen", Age = 28, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.AirlineEmployee, Name = "Tom", Age = 47, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Mike", Age = 26, LoyaltyPoints = 1500, IsUsingLoyaltyPoints = true, AllowedBags = 2 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Pan", Age = 56, LoyaltyPoints = 500, IsUsingLoyaltyPoints = false, AllowedBags = 2 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.AirlineEmployee, Name = "Natalie", Age = 30, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.Discounted, Name = "Chris", Age = 38, AllowedBags = 0 });

            var costOfFlight = scheduledFlight.Passengers.Count * madridToPrague.BaseCost;

            var totalPriceForGeneral = scheduledFlight.Passengers.Where(p => p.Type == PassengerType.General).Count() * madridToPrague.BasePrice;
            var totalPriceForNonUsingLoyaltyPoints = scheduledFlight.Passengers.Where(p => p.Type == PassengerType.LoyaltyMember && p.IsUsingLoyaltyPoints == false).Count() * madridToPrague.BasePrice;
            var totalPriceForDiscounted = (scheduledFlight.Passengers.Where(p => p.Type == PassengerType.Discounted).Count() * madridToPrague.BasePrice) / 2;

            var profitFromFlight = (totalPriceForGeneral + totalPriceForNonUsingLoyaltyPoints + totalPriceForDiscounted) - costOfFlight;

            var seatsTaken = scheduledFlight.Passengers.Count();

            Assert.True(scheduledFlight.CheckProfitSurplus(profitFromFlight));
        }

        [Fact]
        public void Profit_Surplus_Is_Nagative_And_Airline_Employees_Do_Not_Exceed_Minimum_Percentage()
        {
            var madridToPrague = new FlightRoute("Madrid", "Prague")
            {
                BaseCost = 80,
                BasePrice = 150,
                LoyaltyPointsGained = 10,
                MinimumTakeOffPercentage = 0.3
            };

            var scheduledFlight = new ScheduledFlight(madridToPrague);

            scheduledFlight.SetAircraftForRoute(new Plane { Id = 124, Name = "Vader Darth-87", NumberOfSeats = 10 });

            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.General, Name = "John", Age = 32, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.Discounted, Name = "Jen", Age = 28, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.LoyaltyMember, Name = "Mike", Age = 26, LoyaltyPoints = 1500, IsUsingLoyaltyPoints = true, AllowedBags = 2 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.AirlineEmployee, Name = "Natalie", Age = 30, AllowedBags = 1 });
            scheduledFlight.Passengers.Add(new Passenger { Type = PassengerType.Discounted, Name = "Chris", Age = 38, AllowedBags = 0 });

            var costOfFlight = scheduledFlight.Passengers.Count * madridToPrague.BaseCost;

            var totalPriceForGeneral = scheduledFlight.Passengers.Where(p => p.Type == PassengerType.General).Count() * madridToPrague.BasePrice;
            var totalPriceForNonUsingLoyaltyPoints = scheduledFlight.Passengers.Where(p => p.Type == PassengerType.LoyaltyMember && p.IsUsingLoyaltyPoints == false).Count() * madridToPrague.BasePrice;
            var totalPriceForDiscounted = (scheduledFlight.Passengers.Where(p => p.Type == PassengerType.Discounted).Count() * madridToPrague.BasePrice) / 2;

            var profitFromFlight = (totalPriceForGeneral + totalPriceForNonUsingLoyaltyPoints + totalPriceForDiscounted) - costOfFlight;

            var seatsTaken = scheduledFlight.Passengers.Count();

            Assert.False(scheduledFlight.CheckProfitSurplus(profitFromFlight));
        }
    }
}
